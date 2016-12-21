using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Ninject;
using SportsStore.Domain.Db;
using SportsStore.Domain.Interfaces;
using SportsStore.Domain.Processors;
using SportsStore.WebUI.Infrastructure.Interfaces;

namespace SportsStore.WebUI.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private readonly IKernel _kernel;

        public NinjectDependencyResolver(IKernel kernel)
        {
            _kernel = kernel;
            AddBindings();
        }

        public object GetService(Type serviceType)
        {
            return _kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            _kernel.Bind<IProductRepository>().To<EfProductRepository>();

            var emailSettings = new EmailSettings();

            _kernel.Bind<IOrderProcessor>().To<EmailOrderProcessor>().WithConstructorArgument("settings", emailSettings);

            _kernel.Bind<IAuthProvider>().To<FormsAuthProvider>();
        }
    }
}