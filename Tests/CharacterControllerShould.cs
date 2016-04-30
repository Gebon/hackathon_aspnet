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
        private Mock<ICartProvider> mockCartProvider;

        public void UpdateCartProvider(Cart cart)
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
            var mockUserProvider = new Mock<IUserProvider>();
            var genericPrincipal = new GenericPrincipal(new GenericIdentity(name), new string[0]);
            mockUserProvider.Setup(x => x.GetUser(It.IsAny<Controller>())).Returns(genericPrincipal);
            kernel.Bind<IUserProvider>().ToConstant(mockUserProvider.Object);

            var mockUsersRepository = new Mock<IRepository<ApplicationUser>>();
            mockUsersRepository.Setup(x => x.Items).Returns(new List<ApplicationUser>()
            {
                new ApplicationUser
                {
                    UserName = name,
                    Id = genericPrincipal.Identity.GetUserId()
                }
            }.AsQueryable());
            kernel.Bind<IRepository<ApplicationUser>>().ToConstant(mockUsersRepository.Object);
        }

        [SetUp]
        public void SetUp()
        {
            kernel = new StandardKernel();
         
            var mockCharacterRepository = new Mock<IRepository<Character>>();
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

            UpdateIWeekProvider(1);

            UpdateCartProvider(new Cart(new HashSet<int> { 1, 2, 4 }, 15));
            SetUser("");
        }

        private void UpdateIWeekProvider(int week)
        {
            var mockWeekProvider = new Mock<IWeekProvider>();
            mockWeekProvider.Setup(x => x.GetWeek()).Returns(week);
            kernel.Bind<IWeekProvider>().ToConstant(mockWeekProvider.Object);
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

            var cart = kernel.Get<ICartProvider>().GetCart(characterController);
            CollectionAssert.AreEqual(cart.Votes.OrderBy(x => x), new HashSet<int> {1,2,3,4});
        }

        [Test]
        public void CorrectlyVote()
        {
            var cartProvider = kernel.Get<ICartProvider>();
            var characterController = kernel.Get<CharacterController>();
            var points = cartProvider.GetCart(characterController).Points;
            characterController.Vote(3);
            var cart = cartProvider.GetCart(characterController);
            CollectionAssert.AreEqual(cart.Votes.OrderBy(x => x), new HashSet<int> { 1, 2, 3, 4 });
            Assert.AreEqual(points, cart.Points + kernel.Get<IRepository<Character>>().Items.First(x => x.Id == 3).Cost);
        }

        [Test]
        public void CorrectlyUnvote()
        {
            var cartProvider = kernel.Get<ICartProvider>();
            var characterController = kernel.Get<CharacterController>();
            var points = cartProvider.GetCart(characterController).Points;
            characterController.Unvote(2);

            var cart = cartProvider.GetCart(characterController);
            CollectionAssert.AreEqual(cart.Votes.OrderBy(x => x), new HashSet<int> { 1, 4 });
            Assert.AreEqual(points, cart.Points - kernel.Get<IRepository<Character>>().Items.First(x => x.Id == 2).Cost);
        }

        [Test]
        public void RedirectNotAuthorized()
        {
            UpdateCartProvider(new Cart());
            var characterController = kernel.Get<CharacterController>();

            characterController.Vote(2);
            Assert.Throws<NullReferenceException>(() => characterController.Submit());
        }

        [Test]
        public void ProceedFirstSubmit()
        {
            UpdateCartProvider(new Cart());
            SetUser("kek");
            var characterController = kernel.Get<CharacterController>();
            characterController.Vote(1);
            characterController.Submit();
            mockCartProvider.Verify(x => x.SetCart(It.IsAny<Controller>(), It.IsAny<Cart>()), () => Times.Exactly(1));
        }

        [Test]
        public void NotSubmitTwiceForAWeek()
        {
            
        }
    }
}
