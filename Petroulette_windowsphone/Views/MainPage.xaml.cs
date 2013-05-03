using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Threading;
using System.ComponentModel;
using System.Windows.Shapes;
using System.Windows.Media;
using Windows.Phone;

using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using Petroulette_windowsphone.Resources;

using MyToolkit.Multimedia;
using MyToolkit.Networking;
using MyToolkit.Paging;
using MyToolkit.UI;

using petroulette.parser;
using System.Windows.Threading;

namespace Petroulette_windowsphone  //MVVM pattern wasn't used for this preview, I will implement it later
{
    public partial class MainPage : PhoneApplicationPage
    {
        BackgroundWorker bw = new BackgroundWorker();
        Parser pet_parser = new Parser();
        bool video_finished;
        bool next_requested;
        DispatcherTimer timer;
      

        void MainPage_OrientationChanged(object sender, OrientationChangedEventArgs e) //To do when app will detect phone orientations
        {
         /*   if (e.Orientation == PageOrientation.LandscapeLeft || e.Orientation == PageOrientation.LandscapeRight)
            {


                SystemTray.IsVisible = false;
                TitlePanel.Visibility = System.Windows.Visibility.Collapsed;
                loading.Visibility = System.Windows.Visibility.Collapsed;
                app_page.Visibility = System.Windows.Visibility.Collapsed;
                app_title.Visibility = System.Windows.Visibility.Collapsed;

                player.Height = Double.NaN;
                player.Width = Double.NaN;

                player.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
                player.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;

                

            }
            else
            {

                SystemTray.IsVisible = true;
            }*/
        }

        void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) //Job done once Worker thread has finished
        {
            if (!pet_parser.error_encountered) //If no error was encountered
            {
                //we directly set view pet attributes
                Pet_name.Text = "Pet name : " + pet_parser.currentPet.pet_name;
                Pet_specie.Text = "Specie : " + pet_parser.currentPet.pet_specie;
                Pet_description.Text = "Description : " + pet_parser.currentPet.pet_description;
                Pet_shelter.Text = "Shelter : " + pet_parser.currentPet.shelter_name;
                Pet_next_counts.Text = "Next counts : " + pet_parser.currentPet.pet_nextCounts;
                //debug
                System.Diagnostics.Debug.WriteLine("Video URI is :");
                System.Diagnostics.Debug.WriteLine(pet_parser.currentPet.pet_currentVideo.video_uri);

                //Player controls
                player.Stop();
                player.Source = pet_parser.currentPet.pet_currentVideo.video_uri;
                
                player.Play();
               
            }
            else //If an error happened (e.g. network connection error) we try again
            {
                while (bw.IsBusy)
                    Thread.Sleep(100);

                bw.RunWorkerAsync();
            }

            Next_button.IsEnabled = true;
        }

        private void Media_State_Changed(object sender, EventArgs e) //Happens when the video stops playing because of insufficient bandwidth
        {
            if ((player.CurrentState.ToString() == "Opening") || (player.CurrentState.ToString() == "Buffering"))
                mediaStateTextBlock.Text = player.CurrentState.ToString();

            else
                mediaStateTextBlock.Text = "";
        }
  

        void bw_DoWork(object sender, DoWorkEventArgs e) //the main method for background worker
        {
           
          
            bw.ReportProgress(0); 

            if (video_finished) //if the video has finished, an new random is made
            {
                pet_parser.random();
                video_finished = false;
            }

            else if (next_requested) //If next button was pressed
            {
                pet_parser.next();
                next_requested = false;
            }

            else { //If the app was just launched
                pet_parser.random();
            }

            if (!pet_parser.error_encountered)  //if no error
                {
                    bw.ReportProgress(75);
                    pet_parser.currentPet.pet_currentVideo.getVideoUri(); //We get the video mp4 link
                    bw.ReportProgress(100);
                }
                else
                    bw.ReportProgress(-1);
        }


        void MediaElement_MediaFailed(object sender, ExceptionRoutedEventArgs e)
        {

            var errorException = e.ErrorException;
            MessageBox.Show("An error occured with mediaElement");
        }

       

        // Constructor of the main page
        public MainPage()
        {

           
            InitializeComponent();
           
            video_finished = false;
            next_requested = false;
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(200);

            bw.DoWork += new DoWorkEventHandler(bw_DoWork);
            bw.ProgressChanged += new ProgressChangedEventHandler(bw_ProgressChanged);
            bw.WorkerReportsProgress = true;
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
          

            player.MediaEnded += new RoutedEventHandler(player_MediaEnded);
            player.MediaOpened += new RoutedEventHandler(player_MediaOpened);
            player.MediaFailed += new EventHandler<ExceptionRoutedEventArgs>(MediaElement_MediaFailed);

            timer.Tick += new EventHandler(timer_tick);


            while (bw.IsBusy)
                Thread.Sleep(100);

            bw.RunWorkerAsync(); //running the background worker asynchronously so we keep control on the UI


            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        void timer_tick(object sender, EventArgs e)
        {
            loading.Value = player.Position.TotalSeconds;
        }

        void bw_ProgressChanged(object sender, ProgressChangedEventArgs e) //method that makes some changes to the UI while we download stuff
        {
            loading.Maximum = 100;
            if (e.ProgressPercentage == -1)
                MessageBox.Show("An error occured. Please check that your phone is connected to internet.");
            
            else
            {
                Pet_name.Text = "Pet name : ";
                Pet_specie.Text = "Specie : ";
                Pet_description.Text = "Description : ";
                Pet_shelter.Text = "Shelter : ";
               
                loading.Value = e.ProgressPercentage; //Updating the loading bar UI

                if (e.ProgressPercentage == 0)
                    mediaStateTextBlock.Text = "Querying server...";

                if (e.ProgressPercentage == 75)
                    mediaStateTextBlock.Text = "Getting video Url...";

                if (loading.Visibility == Visibility.Collapsed && e.ProgressPercentage < 100)
                    loading.Visibility = Visibility.Visible;

                if (e.ProgressPercentage == 100)
                {
                    //loading.Visibility = Visibility.Collapsed;
                    loading.Value = 0;
                    
                    mediaStateTextBlock.Text = "Done ! Opening video...";
                }
            }
        }
        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            if (YouTube.CancelPlay())
                e.Cancel = true;
            base.OnBackKeyPress(e);
          
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            YouTube.CancelPlay();
       
        }

        void player_MediaOpened(object sender, RoutedEventArgs e)
        {
            if (player.NaturalDuration.HasTimeSpan)
            {
                TimeSpan ts = player.NaturalDuration.TimeSpan;
                loading.Maximum = ts.TotalSeconds;
                loading.SmallChange = 1;
                loading.LargeChange = Math.Min(10, ts.Seconds / 10);
                timer.Start();
            }
     //       
        }

        void player_MediaEnded(object sender, RoutedEventArgs e)
        {

            timer.Stop();
            System.Diagnostics.Debug.WriteLine("MEDIA ENDED !");
          //  player.MediaEnded -= player_MediaEnded;
          //  player.MediaOpened -= player_MediaOpened;
          //  player.MediaFailed -= MediaElement_MediaFailed;

            if(!next_requested)
            video_finished = true;

            Next_button.IsEnabled = false;

           
            while (bw.IsBusy)
                Thread.Sleep(100);
               
               bw.RunWorkerAsync();
       
        }

        private void Next_button_Click(object sender, RoutedEventArgs e)
        {
            Next_button.IsEnabled = false;
            next_requested = true;
            player_MediaEnded(sender, e);
            
           // bw.RunWorkerAsync();
        }


  

   

        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}