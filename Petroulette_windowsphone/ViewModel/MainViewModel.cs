using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Threading;
using GalaSoft.MvvmLight.Messaging;
using System.ComponentModel;

using petroulette.model.parser;
using petroulette.model;
using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Windows;

namespace MvvmLight4.ViewModel
{
 
    
    public class MainViewModel : ViewModelBase
    {
        public ObservableCollection<Video> Videos
        {
            get
            {
                return _videos;
            }
            set
            {
                _videos = value;
                DispatcherHelper.CheckBeginInvokeOnUI(() => { RaisePropertyChanged("Videos"); });
            }
        }

        public string ApplicationName { get { return "PETROULETTE MOBILE APP"; } }
        public string PageName { get { return "random pet"; } }

        public Parser theParser { get; set; }

        public Pet thePet
        {
            get;
            set;
        }

       /* public string VideoThumbnail
        {
            get
            {
                return _videoThumbnail;
            }
            set
            {
                _videoThumbnail = value;
                DispatcherHelper.CheckBeginInvokeOnUI(() => { RaisePropertyChanged("VideoThumbnail"); });
            }
        }*/

        public Uri video_url { get; set; }

        public ObservableCollection<Video> _videos;
       // private double _currentProgress;
        private string _petName;
        private string _petNextCounts;
        private string _petSpecie;
        private string _petRace;
        private string _petDescription;
        private string _petBirthDate;
        private string _AnounceCreationDate;
        private string _AnounceExpirationDate;
       // private string _videoThumbnail;


        WebClient wc = new WebClient();
    

        public string PetName
        {
            get { return "Pet name :  " + _petName; }
            set
            {
                if (value.Equals(""))
                    _petRace = "Undefined";

                else
               _petName = value;

               DispatcherHelper.CheckBeginInvokeOnUI(() => { RaisePropertyChanged("PetName"); });
               
            }
        }
        public string PetNextCounts
        {
            get { return "Pet Next Counts :  " + _petNextCounts; }
            set
            {
                if (value.Equals(""))
                    _petRace = "-";
                else
                _petNextCounts = value;
                
                DispatcherHelper.CheckBeginInvokeOnUI(() => { RaisePropertyChanged("PetNextCounts"); });
            }
        }
        public string PetSpecie
        {
            get { return "Pet Specie :  " + _petSpecie; }
            set
            {
                if (value.Equals(""))
                    _petSpecie = "Undefined";
                
                else
                _petSpecie = value;
                DispatcherHelper.CheckBeginInvokeOnUI(() => { RaisePropertyChanged("PetSpecie"); });

            }
        }

        public string PetRace
        {
            get { return "Pet Race :  " + _petRace; }
            set
            {

                if (value.Equals(""))
                    _petRace = "Undefined";

                else
                    _petRace = value;

                DispatcherHelper.CheckBeginInvokeOnUI(() => { RaisePropertyChanged("PetRace"); });

            }
        }

        public string PetDescription
        {
            get { return "Pet Description :  " + _petDescription; }
            set
            {
                if (value.Equals(""))
                    _petDescription = "-";

                else
                _petDescription = value;

                DispatcherHelper.CheckBeginInvokeOnUI(() => { RaisePropertyChanged("PetDescription"); });

            }
        }

        public string PetBirthDate
        {
            get { return "Pet Birthdate :  " + _petBirthDate; }
            set
            {
                if (value.Equals("1/1/2013"))
                    _petBirthDate = "-";
                else
                _petBirthDate = value;

                DispatcherHelper.CheckBeginInvokeOnUI(() => { RaisePropertyChanged("PetBirthDate"); });

            }
        }

        public string AnounceCreationDate
        {
            get { return "Anounce creation date :  " + _AnounceCreationDate; }
            set
            {
                if (value.Equals("1/1/2013"))
                    _AnounceCreationDate = "-";
                else
                _AnounceCreationDate = value;

                DispatcherHelper.CheckBeginInvokeOnUI(() => { RaisePropertyChanged("AnounceCreationDate"); });

            }
        }

        public string AnounceExpirationDate
        {
            get { return "Anounce expiration date :  " + _AnounceExpirationDate; }
            set
            {
                if (value.Equals("1/1/2013"))
                    _AnounceExpirationDate = "-";
                else
                    _AnounceExpirationDate = value;

                DispatcherHelper.CheckBeginInvokeOnUI(() => { RaisePropertyChanged("AnounceExpirationDate"); });

            }
        }

        

        public MainViewModel()
        {
            if (!ViewModelBase.IsInDesignModeStatic)
            {
                
                Messenger.Default.Register<Parser>(this, process_fill);
                Messenger.Default.Register<string>("MEDIA_ENDED", process_random);
                //Messenger.Default.Register<string>("NEXT_FINISHED", process_random);
                Messenger.Default.Register<string>("NEXT_CLICKED", process_next);
                Messenger.Default.Register<string>("GOT_URI", process_play);
                Messenger.Default.Register<string>("EXCEPTION", process_exception);
                
                Messenger.Default.Register<Video>(this, process_video_changed);
               // Messenger.Default.Register<Parser>("NEXT_FINISHED", process_fill);


                theParser = new Parser(1);
                
                // DispatcherHelper.CheckBeginInvokeOnUI(() => { item.next();  });
                // WelcomeTitle = item.currentPet.pet_currentVideo.video_url;
            }
            else
            {     }
                 
        }

        void process_video_changed(Video v)
        {
            thePet.pet_currentVideo = v;
            bool error = false;
            int i = 0;
            System.Threading.ThreadPool.QueueUserWorkItem(delegate
            {
                do
            {
                try
                {
                    v.getVideoUri();
                    error = false;
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine("EXCEPTION !");
                    i++;
                    error = true;
                }
            }
            while (error == true && i < 5);
            }, null);
            
        }
        void process_exception(string str)
        {
            if (str.Equals("EXCEPTION"))
            {
                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    MessageBox.Show("An error occured. Please check your internet connection !");
                });

            }
        }

        void process_fill(Parser _parser)
        {
            if (!theParser.error_encountered)
            {
                Messenger.Default.ToString();
                // System.Diagnostics.Debug.WriteLine("FINISHED ! " + _parser.currentPet.pet_currentVideo.video_uri);
                thePet = theParser.currentPet;
                PetNextCounts = Convert.ToString(thePet.pet_nextCounts);
                PetName = thePet.pet_name;
                PetDescription = thePet.pet_description;
                PetRace = thePet.pet_race;
                PetSpecie = thePet.pet_specie;
                PetBirthDate = thePet.pet_birthDate.ToShortDateString();
                AnounceCreationDate = thePet.pet_createdDate.ToShortDateString();
                AnounceExpirationDate = thePet.pet_availableUntilDate.ToShortDateString();
                Videos = new videoCollection(thePet).Videos;
                //VideoThumbnail = thePet.pet_currentVideo.video_thumbnail;
                bool error = false;
                int i = 0;
                do
                {
                    try
                    {
                        thePet.pet_currentVideo.getVideoUri();
                        error = false;
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine("EXCEPTION !");
                        i++;
                        error = true;
                    }
                }
                while (error == true && i < 5);
            }
            else
                MessageBox.Show("error");

        }

        void process_random(string str)
        {
            if(str.Equals("MEDIA_ENDED"))
            {
            System.Threading.ThreadPool.QueueUserWorkItem(delegate
            {
                theParser.random();
            }, null);
            System.Diagnostics.Debug.WriteLine("A VIDEO HAS FINISHED BY ITSELF !");
            }
        }


        void process_next(string str)
        {
            //  DispatcherHelper.RunAsync(() => { theParser.random(); });
            // theParser.random();
            if(str.Equals("NEXT_CLICKED"))
            {
            System.Threading.ThreadPool.QueueUserWorkItem(delegate
            {
                theParser.next();
            }, null);
            System.Diagnostics.Debug.WriteLine("NEXT BUTTON PRESSED !");
            }
        }


        void process_play(string str)
        {

            if (str.Equals("GOT_URI"))
            {
                Messenger.Default.Send<Uri>(this.thePet.pet_currentVideo.video_uri);
            }
        }
        

        public void PrepareNext()
        {
           
        }
        ////public override void Cleanup()
        ////{
        ////    // Clean up if needed

        ////    base.Cleanup();
        ////}
    }
}