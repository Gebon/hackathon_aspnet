using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using DepenedcyInjection.Controllers;
using DepenedcyInjection.Infrastructure;
using Domain;
using Moq;
using Ninject;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class CharacterControllerShould
    {
        [Test]
        public void returnCorrectlyFilteredItems()
        {
            var mock = new Mock<ICharactersRepository>();
            mock.Setup(m => m.Characters).Returns(new List<Character>() {new Character {Born = "In 103 DC", Died = false, Gender = Gender.Male, Name = "Eddard Stark", Id = 1},
                new Character {Born = "In 102 DC", Died = true, Gender = Gender.Female, Name = "Sylwa Paege", Id = 2},
                new Character {Born = "In 105 DC", Died = false, Gender = Gender.Male, Name = "Terrence Toyne", Id = 3},
                new Character {Born = "In 101 DC", Died = false, Gender = Gender.Female, Name = "Tanda Stokeworth", Id = 4}}.AsQueryable());
            var kernel = new StandardKernel();
            kernel.Bind<ICharactersRepository>().ToConstant(mock.Object);
            kernel.Bind<Controller>().To<CharacterController>();
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
            var mock = new Mock<ICartProvider>();
            mock.Setup(x => x.GetCart).Returns(new List<int>
            {
                1,2,3,4
            });
            var kernel = new StandardKernel();
            kernel.Bind<ICartProvider>().ToConstant(mock.Object);
            var sessionProvider = kernel.Get<ICartProvider>();
            Assert.AreEqual(sessionProvider.GetCart, new List<int>{1,2,3,4});
        }
    }
}
