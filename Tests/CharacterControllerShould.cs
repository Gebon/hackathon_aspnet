using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web.Mvc;
using DependencyInjection.Models;
using DepenedcyInjection.Controllers;
using DepenedcyInjection.Infrastructure;
using DepenedcyInjection.Repositories;
using Domain;
using Microsoft.AspNet.Identity;
using Moq;
using Ninject;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class CharacterControllerShould
    {
        private StandardKernel kernel;
        private Mock<IRepository<Character>> mockCharacterRepository;
        private Mock<ICartProvider> mockCartProvider;
        private Mock<IUserProvider> mockUserProvider;

        public void SetCart(Cart cart)
        {
            kernel.Unbind<ICartProvider>();
            mockCartProvider = new Mock<ICartProvider>();
            mockCartProvider.Setup(x => x.GetCart(It.IsAny<Controller>())).Returns(cart);
            kernel.Bind<ICartProvider>().ToConstant(mockCartProvider.Object);
        }

        public void SetUser(string name)
        {
            kernel.Unbind<IPrincipal>();
            kernel.Unbind<IUserProvider>();
            kernel.Unbind<IRepository<ApplicationUser>>();
            mockUserProvider = new Mock<IUserProvider>();
            var genericPrincipal = new GenericPrincipal(new GenericIdentity(name), new string[0]);
            mockUserProvider.Setup(x => x.GetUser(It.IsAny<Controller>())).Returns(genericPrincipal);
            kernel.Bind<IUserProvider>().ToConstant(mockUserProvider.Object);

            var mockUsersRepository = new Mock<IRepository<ApplicationUser>>();
            mockUsersRepository.Setup(x => x.Items).Returns(new List<ApplicationUser>()
            {
                new ApplicationUser
                {
                    UserName = name,
                    Id = IdentityExtensions.FindFirstValue(genericPrincipal.Identity as ClaimsIdentity, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")
                }
            }.AsQueryable());
            kernel.Bind<IRepository<ApplicationUser>>().ToConstant(mockUsersRepository.Object);
        }

        [SetUp]
        public void SetUp()
        {
            kernel = new StandardKernel();
         
            mockCharacterRepository = new Mock<IRepository<Character>>();
            mockCharacterRepository.Setup(x => x.Items).Returns(new List<Character>
            {
                new Character {Born = "In 103 DC", Died = false, Gender = Gender.Male, Name = "Eddard Stark", Id = 1,Cost = 10},
                new Character {Born = "In 102 DC", Died = true, Gender = Gender.Female, Name = "Sylwa Paege", Id = 2, Cost = 2},
                new Character {Born = "In 105 DC", Died = false, Gender = Gender.Male, Name = "Terrence Toyne", Id = 3, Cost = 7},
                new Character {Born = "In 101 DC", Died = false, Gender = Gender.Female, Name = "Tanda Stokeworth", Id = 4, Cost = 3}
            }.AsQueryable());
            kernel.Bind<IRepository<Character>>().ToConstant(mockCharacterRepository.Object);

            var mockVotesRepository = new Mock<IRepository<Vote>>();
            mockVotesRepository.Setup(x => x.Items).Returns(new List<Vote>()
            {
            }.AsQueryable());
            kernel.Bind<IRepository<Vote>>().ToConstant(mockVotesRepository.Object);


            var mockVoteItemsRepository = new Mock<IRepository<VoteItem>>();
            mockVoteItemsRepository.Setup(x => x.Items).Returns(new List<VoteItem>()
            {
            }.AsQueryable());
            kernel.Bind<IRepository<VoteItem>>().ToConstant(mockVoteItemsRepository.Object);

            SetCart(new Cart(new HashSet<int> { 1, 2, 4 }, 15));
            SetUser("");
        }
        [Test]
        public void ReturnCorrectlyFilteredItems()
        {
            var controller = kernel.Get<CharacterController>();
            var result = controller.FilterInternal(nameof(Character.Gender), Gender.Female.ToString());
            Assert.AreEqual(result, new List<Character>
            {
                new Character { Born = "In 102 DC", Died = true, Gender = Gender.Female, Name = "Sylwa Paege", Id = 2 },
                new Character {Born = "In 101 DC", Died = false, Gender = Gender.Female, Name = "Tanda Stokeworth", Id = 4}
            });
        }

        [Test]
        public void ReturnCorrectSession()
        {
            var controller = kernel.Get<CharacterController>();
            var sessionProvider = kernel.Get<ICartProvider>();
            Assert.AreEqual(sessionProvider.GetCart(controller).Votes, new List<int> { 1, 2, 4 });
        }

        [Test]
        public void PreventMultipleVotingForOneCharacter()
        {
            var characterController = kernel.Get<CharacterController>();
            characterController.Vote(3);
            characterController.Vote(3);

            var cart = mockCartProvider.Object.GetCart(characterController);
            CollectionAssert.AreEqual(cart.Votes.OrderBy(x => x), new HashSet<int> {1,2,3,4});
        }

        [Test]
        public void CorrectlyVote()
        {
            var characterController = kernel.Get<CharacterController>();
            var points = mockCartProvider.Object.GetCart(characterController).Points;
            characterController.Vote(3);
            var cart = mockCartProvider.Object.GetCart(characterController);
            CollectionAssert.AreEqual(cart.Votes.OrderBy(x => x), new HashSet<int> { 1, 2, 3, 4 });
            Assert.AreEqual(points, cart.Points + mockCharacterRepository.Object.Items.First(x => x.Id == 3).Cost);
        }

        [Test]
        public void CorrectlyUnvote()
        {
            var characterController = kernel.Get<CharacterController>();
            var points = mockCartProvider.Object.GetCart(characterController).Points;
            characterController.Unvote(2);

            var cart = mockCartProvider.Object.GetCart(characterController);
            CollectionAssert.AreEqual(cart.Votes.OrderBy(x => x), new HashSet<int> { 1, 4 });
            Assert.AreEqual(points, cart.Points - mockCharacterRepository.Object.Items.First(x => x.Id == 2).Cost);
        }

        [Test]
        public void RedirectNotAuthorized()
        {
            SetCart(new Cart());
            var characterController = kernel.Get<CharacterController>();

            characterController.Vote(2);
            Assert.Throws<NullReferenceException>(() => characterController.Submit());
        }

        [Test]
        public void AddPoints()
        {
            SetCart(new Cart());
            mockCartProvider.Verify(x => x.SetCart(It.IsAny<Controller>(), It.IsAny<Cart>()), () => Times.Exactly(1));
            SetUser("kek");
            var characterController = kernel.Get<CharacterController>();
            characterController.Vote(1);
            characterController.Submit();
            Console.WriteLine(string.Join(",", mockCartProvider.Object.GetCart(characterController).Votes));

        }
    }
}
