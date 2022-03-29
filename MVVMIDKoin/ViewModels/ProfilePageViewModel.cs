using Caliburn.Micro;
using IDKoin.Properties;
using Microsoft.Win32;
using MVVMIDKoin.EventModels;
using MVVMIDKoin.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace MVVMIDKoin.ViewModels
{
    public class ProfilePageViewModel : Screen, IHandle<LogOnEventModel>
    {
        private readonly IEventAggregator _eventAggregator;
        string username;
        private readonly MongoHandler _mongoHandler;
        public ProfilePageViewModel(IEventAggregator eventAggregator, MongoHandler mongoHandler)
        {
            _eventAggregator = eventAggregator;
            _mongoHandler = mongoHandler;
            _eventAggregator.SubscribeOnUIThread(this);
        }

        private string _name;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = "Name: " + value;
                NotifyOfPropertyChange(() => Name);
            }
        }

        private string _koins;

        public string Koins
        {
            get { return _koins; }
            set 
            {
                _koins = "Balance: " + value;
                NotifyOfPropertyChange(() => Koins);
            }
        }

        private BitmapImage _img;

        public BitmapImage Img
        {
            get { return _img; }
            set 
            { 
                _img = value;
                NotifyOfPropertyChange(() => Img);
            }
        }

        public BitmapImage ImageFromBuffer(Byte[] bytes)
        {
            MemoryStream stream = new MemoryStream(bytes);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = stream;
            image.EndInit();
            return image;
        }


        public Task HandleAsync(LogOnEventModel message, CancellationToken cancellationToken)
        {
            Name = message.person.Name;
            username = message.person.Name;
            Koins = message.person.Koins.ToString();
            Img = ImageFromBuffer(_mongoHandler.GetImage(username));
            return Task.CompletedTask;
        }

        public void LoadImage()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title = "Select Image";
            dlg.Filter = "png files (*.png)|*.png|jpg files (*.jpg)|*.jpg";
            if(dlg.ShowDialog() == true)
            {
                Img = new BitmapImage(new Uri(dlg.FileName));
                byte[] buffer = File.ReadAllBytes(dlg.FileName);
                _mongoHandler.AddImage(username,buffer);
            }
        }



    }
}
