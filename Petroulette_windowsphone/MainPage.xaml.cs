using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using MvvmLight4.Resources;
using petroulette.model.parser;
using GalaSoft.MvvmLight.Threading;
using GalaSoft.MvvmLight.Messaging;
using System.Windows.Threading;

namespace MvvmLight4
{
    public partial class MainPage : PhoneApplicationPage
    {
        DispatcherTimer timer;

        public MainPage()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(200);
            timer.Tick += new EventHandler(timer_tick);
            Messenger.Default.Register<Uri>("PLAY_REQUESTED", play);

        }

        public void ResetLoadingPreferences()
        {
            DispatcherHelper.CheckBeginInvokeOnUI(() =>
            {
                Loading.Value = 0;
                Loading.Minimum = 0;
                Loading.Maximum = 100;
                Loading.LargeChange = 1;
                Loading.SmallChange = 0.1;
            });
         
        }

        public void play(Uri url)
        {
            DispatcherHelper.CheckBeginInvokeOnUI(() =>
            {
                Next_button.IsEnabled = true;
                player.Stop();
                player.Source = url;
                player.Play();
            });
        }

        private void PhoneApplicationPage_Loaded_1(object sender, RoutedEventArgs e)
        {

        }

        private void player_MediaEnded(object sender, RoutedEventArgs e)
        {
            Next_button.IsEnabled = false;
            Messenger.Default.Send<string>("MEDIA_ENDED"); //BOTH ARE CALLED
            ResetLoadingPreferences();
        }

        private void Next_button_Click(object sender, RoutedEventArgs e)
        {
            player.Stop();
            ResetLoadingPreferences();
            Messenger.Default.Send<string>("NEXT_CLICKED"); //BOTH ARE CALLED
            Next_button.IsEnabled = false;
        }

        void timer_tick(object sender, EventArgs e)
        {
            Loading.Value = player.Position.TotalSeconds;
        }

        private void player_MediaOpened(object sender, RoutedEventArgs e)
        {
     
            if (player.NaturalDuration.HasTimeSpan)
            {
                Loading.Value = 0;
                TimeSpan ts = player.NaturalDuration.TimeSpan;
                Loading.Maximum = ts.TotalSeconds;
                Loading.SmallChange = 1;
                Loading.LargeChange = Math.Min(10, ts.Seconds / 10);
                timer.Start();
            }
         
        
        }
       
    }
}