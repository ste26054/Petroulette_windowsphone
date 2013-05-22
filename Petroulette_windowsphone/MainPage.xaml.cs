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
using System.Windows.Media;
using System.Windows.Controls.Primitives;

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


                


                   player.MaxWidth = System.Windows.Application.Current.Host.Content.ActualHeight;
                   player.MaxHeight = System.Windows.Application.Current.Host.Content.ActualWidth;

                   player.Width = System.Windows.Application.Current.Host.Content.ActualHeight;
                   player.Height = System.Windows.Application.Current.Host.Content.ActualWidth;
                   shape.MaxHeight = System.Windows.Application.Current.Host.Content.ActualWidth;

                   shape.Height = System.Windows.Application.Current.Host.Content.ActualWidth;

               }
               else
               {
                   player.MaxWidth = System.Windows.Application.Current.Host.Content.ActualWidth;
                   player.MaxHeight = 280;
                   shape.MaxHeight = 280;
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
            Next_button.IsEnabled = false;
            pic_adopt.Opacity = 0.3;
            pic_next.Opacity = 0.3;
            //Next_button.Visibility = System.Windows.Visibility.Collapsed;
            Adopt_button.IsEnabled = false;
            //Adopt_button.Visibility = System.Windows.Visibility.Collapsed;
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
                //Next_button.Visibility = System.Windows.Visibility.Visible;
                Adopt_button.IsEnabled = true;
                //Adopt_button.Visibility = System.Windows.Visibility.Visible;
                pic_adopt.Opacity = 1;
                pic_next.Opacity = 1;
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
            //Next_button.Visibility = System.Windows.Visibility.Collapsed;
            Adopt_button.IsEnabled = false;
            pic_adopt.Opacity = 0.3;
            pic_next.Opacity = 0.3;
            //Adopt_button.Visibility = System.Windows.Visibility.Collapsed;
            Messenger.Default.Send<string>("MEDIA_ENDED"); 
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
            Messenger.Default.Send<string>("NEXT_CLICKED");
            Next_button.IsEnabled = false;
            //Next_button.Visibility = System.Windows.Visibility.Collapsed;
            Adopt_button.IsEnabled = false;
            pic_adopt.Opacity = 0.3;
            pic_next.Opacity = 0.3;
            //Adopt_button.Visibility = System.Windows.Visibility.Collapsed;
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
            System.Diagnostics.Debug.WriteLine("MEDIA FAILED !");

        }

        private void player_CurrentStateChanged(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Player current state : " + player.CurrentState);

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

        private void ApplicationBarMenuItem_Click(object sender, EventArgs e)
        {//TODO

            Next_button.IsEnabled = false;
            //Next_button.Visibility = System.Windows.Visibility.Collapsed;
            Adopt_button.IsEnabled = false;
            //Adopt_button.Visibility = System.Windows.Visibility.Collapsed;
            Messenger.Default.Send<string>("MEDIA_ENDED");
            ResetLoadingPreferences();
       
        }

        private void Hints_button_click(object sender, EventArgs e)
        {
            DispatcherHelper.CheckBeginInvokeOnUI(() =>
            {
                
                MessageBox.Show("Hints :\n- Tap to play/pause the video\n- Double tap to restart the video\n- Tap and hold to hide/show the bottom bar !");
            });
        }

    

        private void PhoneApplicationPage_Hold(object sender, System.Windows.Input.GestureEventArgs e)
        {
            this.ApplicationBar.IsVisible = !this.ApplicationBar.IsVisible;
        }

       
        private void Adopt_button_click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage2.xaml", UriKind.Relative));

        }

        private void Donate_button_click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage3.xaml", UriKind.Relative));
        }

        private void Adopt_button_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {

        }
       
        
    }
}