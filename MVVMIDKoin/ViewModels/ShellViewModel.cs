using Caliburn.Micro;
using MVVMIDKoin.EventModels;
using Squirrel;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MVVMIDKoin.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<LogOnEventModel>
    {
        private readonly LoginPageViewModel _loginPageVM;
        private readonly HomePageViewModel _homePageVM;
        private readonly ProfilePageViewModel _profilePageVM;
        private readonly TransactionsPageViewModel _transactionsPageVM;
        private readonly LeaderBoardPageViewModel _leaderBoardPageVM;
        private readonly IEventAggregator _eventAggregator;

        public ShellViewModel(LoginPageViewModel loginPageVM, IEventAggregator eventAggregator, HomePageViewModel homePageVM, ProfilePageViewModel profilePageVM,
            TransactionsPageViewModel transactionsPageVM, LeaderBoardPageViewModel leaderBoardVM)
        {
            _eventAggregator = eventAggregator;
            _homePageVM = homePageVM;
            _loginPageVM = loginPageVM;
            _leaderBoardPageVM = leaderBoardVM;
            _profilePageVM = profilePageVM;
            _transactionsPageVM = transactionsPageVM;
            _eventAggregator.SubscribeOnUIThread(this);
            ActivateItemAsync(_loginPageVM);
            Update();
        }

        private async Task Update()
        {
            using(var manager = new UpdateManager(@"C:\Temp\Releases"))
            {
                await manager.UpdateApp();
            }
        }

        public void Exit()
        {
            Environment.Exit(0);
        }
        private bool _logged = false;

        //Logged property for enabling/disabling menu buttons
        public bool Logged
        {
            get { return _logged; }
            set 
            {
                _logged = value; 
                NotifyOfPropertyChange(() => Logged);
            }
        }


        //Methods for loading diferent views
        public async void LoadHome()
        {
            await ActivateItemAsync(_homePageVM);
        }
        public async void LoadProfile()
        {
            await ActivateItemAsync(_profilePageVM);
        }
        public async void LoadTransactions()
        {
            await ActivateItemAsync(_transactionsPageVM);
        }
        public async void LoadLeaderBoard()
        {
            await ActivateItemAsync(_leaderBoardPageVM);
        }

        //Handeling event from LoginPageViewModel (Displaying HomePageView and Enabling menu buttons)
        public Task HandleAsync(LogOnEventModel message, CancellationToken cancellationToken)
        {
            LoadHome();
            Logged = true;
            return Task.CompletedTask;
        }
        public void Off()
        {
            Environment.Exit(0);
        }
    }
}