using System;
using System.Web.Mvc;
using System.Web.Routing;
using DependencyInjection.Models;
using DepenedcyInjection.Repositories;
using Domain;
using Ninject;

namespace DepenedcyInjection.Infrastructure
{
    public class NinjectControllerFactory : DefaultControllerFactory
    {
        private IKernel ninjectKernel;

        public NinjectControllerFactory()
        {
            // создание контейнера    
            ninjectKernel = new StandardKernel();
            AddBindings();
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            // получение объекта контроллера из контейнера        
            // используя его тип       
            return controllerType == null ? null : (IController)ninjectKernel.Get(controllerType);
        }

        private void AddBindings()
        {
            ninjectKernel.Bind<IRepository<ApplicationUser>>().To<UsersRepository>();
            ninjectKernel.Bind<IRepository<VoteItem>>().To<VoteItemsRepository>();
            ninjectKernel.Bind<IRepository<Vote>>().To<VotesRepository>();
            ninjectKernel.Bind<IRepository<Character>>().To<CharactersRepository>();
            ninjectKernel.Bind<ICartProvider>().To<CartProvider>();
            ninjectKernel.Bind<IUserProvider>().To<UserProvider>();
        }
    }
}