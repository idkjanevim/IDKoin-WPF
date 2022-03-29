using Caliburn.Micro;
using IDKoin.Properties;
using MVVMIDKoin.EventModels;
using MVVMIDKoin.Models;
using System.Windows.Controls;

namespace MVVMIDKoin.ViewModels
{
    public class LoginPageViewModel : Screen
    {
        private readonly MongoHandler _mongoHandler;
        private readonly IEventAggregator _eventAggregator;

        public LoginPageViewModel(IEventAggregator eventAggregator, MongoHandler mongoHandler)
        {
            _mongoHandler = mongoHandler;
            _eventAggregator = eventAggregator;
        }

        private string _username;
        private string _password;

        public string Username
        {
            get { return _username; }
            set { _username = value; }
        }

        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        //Emmiting event to ShellViewModel that person has logged in
        public void LoginClick()
        {
            PersonModel person = _mongoHandler.Login(Username, Password);
            if (person == null)
                return;
            _eventAggregator.PublishOnUIThreadAsync(new LogOnEventModel(person));
        }
        public void OnPasswordChanged(PasswordBox source)
        {
            Password = source.Password;
        }

    }
}
