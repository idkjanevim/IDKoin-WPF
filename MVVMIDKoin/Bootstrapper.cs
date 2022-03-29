using Caliburn.Micro;
using IDKoin.Properties;
using MVVMIDKoin.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MVVMIDKoin
{
    public class Bootstrapper : BootstrapperBase
    {
        private SimpleContainer _container = new SimpleContainer();
        public Bootstrapper()
        {
            Initialize();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<ShellViewModel>();
        }


        //Setting up dependency injection
        protected override void Configure()
        {
            _container.Singleton<IWindowManager, WindowManager>();
            _container.Singleton<ShellViewModel>();
            _container.Singleton<LoginPageViewModel>();
            _container.Singleton<IEventAggregator, EventAggregator>();
            _container.Singleton<MongoHandler>();
            _container.Singleton<HomePageViewModel>();
            _container.Singleton<TransactionsPageViewModel>();
            _container.Singleton<LeaderBoardPageViewModel>();
            _container.Singleton<ProfilePageViewModel>();
            _container.Singleton<Miner>();
        }
        protected override void BuildUp(object instance)
        {
            _container.BuildUp(instance);
        }
        protected override object GetInstance(Type service, string key)
        {
            return _container.GetInstance(service, key);
        }
        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _container.GetAllInstances(service);
        }
    }
}
