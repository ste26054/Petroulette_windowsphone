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
using petroulette.model;
using GalaSoft.MvvmLight.Threading;
using GalaSoft.MvvmLight.Messaging;
using System.Windows.Threading;

namespace MvvmLight4
{
    public partial class MainPage : PhoneApplicationPage
    {

        void MainPage_OrientationChanged(object sender, OrientationChangedEventArgs e) //To do when app will detect phone orientations
        {
               if (e.Orientation == PageOrientation.LandscapeLeft || e.Orientation == PageOrientation.LandscapeRight)
               {
                   TitlePanel.Visibility = System.Windows.Visibility.Collapsed;
                   Buttons.Visibility = System.Windows.Visibility.Collapsed;
                   ControlPanel.Visibility = System.Windows.Visibility.Collapsed;
                   SystemTray.IsVisible = false;



                   player.Height = Double.NaN;
                   player.Width = Double.NaN;

                   player.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
                   player.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;


                

               }
               else
               {
                   TitlePanel.Visibility = System.Windows.Visibility.Visible;
                   Buttons.Visibility = System.Windows.Visibility.Visible;
                   ControlPanel.Visibility = System.Windows.Visibility.Visible;
                   SystemTray.IsVisible = true;
               }
        }

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
            DispatcherHelper.CheckBeginInvokeOnUI(() =>
            {
                LoadingProgress.IsIndeterminate = true;
                player.Stop();
            });
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

        private void player_MediaFailed(object sender, ExceptionRoutedEventArgs e)
        {

        }

        private void player_CurrentStateChanged(object sender, RoutedEventArgs e)
        {
            if ((player.CurrentState.ToString() == "Opening") || (player.CurrentState.ToString() == "Buffering"))
            {
                LoadingProgress.IsIndeterminate = true;
                LoadingProgress.Visibility = Visibility.Visible;
            }

            else
            {
                LoadingProgress.IsIndeterminate = false;
                LoadingProgress.Visibility = Visibility.Collapsed;
            }
        }

        private void player_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(player.CurrentState.ToString());

            if (player.CurrentState.ToString().Equals("Playing"))
                player.Pause();

            else
                player.Play();
            
        }

        private void player_DoubleTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
          
                player.Stop();
                player.Play();
           
        }

        private void ListBox_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("ITEM CLICKED ! " + e.OriginalSource.ToString());
            
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                System.Diagnostics.Debug.WriteLine("TEST !!" + ((Video)VideoList.SelectedItem).video_title);
                Messenger.Default.Send<Video>((Video)VideoList.SelectedItem);
                
            }
        }
       
    }
}