using Caliburn.Micro;
using IDKoin.Properties;
using MVVMIDKoin.EventModels;
using System;
using System.Windows.Media;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace MVVMIDKoin.ViewModels
{
    
    public class HomePageViewModel : Screen, IHandle<LogOnEventModel>
    {
        private readonly Miner _miner;
        Label _label;
        private readonly IEventAggregator _aggregator;
        string username;
        public HomePageViewModel(Miner miner, IEventAggregator eventAggregator)
        {
            _miner = miner;
            _aggregator = eventAggregator;
            _aggregator.SubscribeOnUIThread(this);
            BrColor = Brushes.Red;
        }


        private Brush _brcolor;

        public Brush BrColor
        {
            get { return _brcolor; }
            set 
            {
                _brcolor = value; 
                NotifyOfPropertyChange(() => BrColor);
            }
        }

        private string _bal;

        public string Balance
        {
            get { return _bal; }
            set 
            {
                _bal = value;
                NotifyOfPropertyChange(() => Balance);
            }
        }



        public void Mine()
        {
            if (BrColor == Brushes.Red)
                BrColor = Brushes.Green;
            else
                BrColor = Brushes.Red;

            _miner.ToggleMine(username, _label);

        }
        public void Msg(object view)
        {
            var frameworkElement = view as FrameworkElement;
            _label = frameworkElement.FindName("Balance") as Label;
        }

        public Task HandleAsync(LogOnEventModel message, CancellationToken cancellationToken)
        {
            username = message.person.Name;
            Balance = "Balance: " + message.person.Koins.ToString();
            return Task.CompletedTask;
        }
    }
}
