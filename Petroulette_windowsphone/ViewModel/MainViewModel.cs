using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Threading;
using GalaSoft.MvvmLight.Messaging;
using System.ComponentModel;

using petroulette.model.parser;
using petroulette.model;
using System;
namespace MvvmLight4.ViewModel
{
 
    
    public class MainViewModel : ViewModelBase
    {

        public string ApplicationName { get { return "PETROULETTE MOBILE APP"; } }
        public string PageName { get { return "random pet"; } }

        public Parser theParser { get; set; }

        public Pet thePet
        {
            get;
            set;
        }

        public Uri video_url { get; set; }
        private double _currentProgress;
        private string _petName;
        private string _petNextCounts;
        private string _petSpecie;
        private string _petRace;
        private string _petDescription;
        public string PetName
        {
            get { return "Pet name : " + _petName; }
            set
            {
               _petName = value;
               DispatcherHelper.CheckBeginInvokeOnUI(() => { RaisePropertyChanged("PetName"); });
               
            }
        }
        public string PetNextCounts
        {
            get { return "Pet Next Counts : " + _petNextCounts; }
            set
            {
                _petNextCounts = value;
                DispatcherHelper.CheckBeginInvokeOnUI(() => { RaisePropertyChanged("PetNextCounts"); });

            }
        }
        public string PetSpecie
        {
            get { return "Pet Specie : " + _petSpecie; }
            set
            {
                _petSpecie = value;
                DispatcherHelper.CheckBeginInvokeOnUI(() => { RaisePropertyChanged("PetSpecie"); });

            }
        }

        public string PetRace
        {
            get { return "Pet Race : " + _petRace; }
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
            get { return "Pet Description : " + _petDescription; }
            set
            {
                _petDescription = value;
                DispatcherHelper.CheckBeginInvokeOnUI(() => { RaisePropertyChanged("PetDescription"); });

            }
        }
        
        public double CurrentProgress
        {
            get { return _currentProgress; }
            set
            {
                _currentProgress = value;
                DispatcherHelper.CheckBeginInvokeOnUI(() => { RaisePropertyChanged(""); });
            }
        }
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            if (!ViewModelBase.IsInDesignModeStatic)
            {
                
                Messenger.Default.Register<Parser>(this, process_fill);
                Messenger.Default.Register<string>("MEDIA_ENDED", process_random);
                //Messenger.Default.Register<string>("NEXT_FINISHED", process_random);
                Messenger.Default.Register<string>("NEXT_CLICKED", process_next);
                Messenger.Default.Register<string>("GOT_URI", process_play);
               // Messenger.Default.Register<Parser>("NEXT_FINISHED", process_fill);
                theParser = new Parser(1);
                CurrentProgress = 0;
                // DispatcherHelper.CheckBeginInvokeOnUI(() => { item.next();  });
                // WelcomeTitle = item.currentPet.pet_currentVideo.video_url;
            }
            else
            {     }
                 
        }

        void process_fill(Parser _parser)
        {
            CurrentProgress = 50;
            Messenger.Default.ToString();
            // System.Diagnostics.Debug.WriteLine("FINISHED ! " + _parser.currentPet.pet_currentVideo.video_uri);
            thePet = theParser.currentPet;
            PetNextCounts = Convert.ToString(thePet.pet_nextCounts);
            PetName = thePet.pet_name;
            PetDescription = thePet.pet_description;
            PetRace = thePet.pet_race;
            PetSpecie = thePet.pet_specie;
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
            //video_url = thePet.pet_currentVideo.video_uri;
            // Messenger.Default.Send<Uri>(this.video_url);
        }

        void process_random(string str)
        {
          //  DispatcherHelper.RunAsync(() => { theParser.random(); });
           // theParser.random();
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
                CurrentProgress = 100;
   
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