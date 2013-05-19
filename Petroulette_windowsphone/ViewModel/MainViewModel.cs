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
        public string NetworkErrorMessage { get { return "An error occured. Please check your internet connection !"; } }
        public Parser theParser { get; set; }

        public Pet thePet { get; set; }
        public Appointment theAppointment { get; set; }
        public int error_count { get; set; }
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

        private string _petShelterName;
        private string _petShelterAddress;
        private string _petShelterEmail;
        private string _petShelterPhoneNumber;

        private string _appointmentName;
        private string _appointmentEmail;
        private string _appointmentPhoneNumber;
        private DateTime _appointmentDate;
        private bool _appointmentOk;
        private string _appointmentButtonContent;

       // private string _videoThumbnail;


        WebClient wc = new WebClient();
    

        public string PetName
        {
            get { return  _petName; }
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
            get { return _petNextCounts; }
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
            get { return  _petSpecie; }
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
            get { return  _petRace; }
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
            get { return _petDescription; }
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
            get { return  _petBirthDate; }
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
            get { return  _AnounceCreationDate; }
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
            get { return  _AnounceExpirationDate; }
            set
            {
                if (value.Equals("1/1/2013"))
                    _AnounceExpirationDate = "-";
                else
                    _AnounceExpirationDate = value;

                DispatcherHelper.CheckBeginInvokeOnUI(() => { RaisePropertyChanged("AnounceExpirationDate"); });

            }
        }

        public string ShelterName
        {
            get { return _petShelterName; }
            set
            {
                if (value.Equals(""))
                    _petShelterName = "-";
                else
                    _petShelterName = value;

                DispatcherHelper.CheckBeginInvokeOnUI(() => { RaisePropertyChanged("ShelterName"); });

            }
        }

        public string ShelterAddress
        {
            get { return _petShelterAddress; }
            set
            {
                if (value.Equals(""))
                    _petShelterAddress = "-";
                else
                    _petShelterAddress = value;

                DispatcherHelper.CheckBeginInvokeOnUI(() => { RaisePropertyChanged("ShelterAddress"); });

            }
        }

        public string ShelterPhoneNumber
        {
            get { return _petShelterPhoneNumber; }
            set
            {
                if (value.Equals(""))
                    _petShelterPhoneNumber = "-";
                else
                    _petShelterPhoneNumber = value;

                DispatcherHelper.CheckBeginInvokeOnUI(() => { RaisePropertyChanged("ShelterPhoneNumber"); });

            }
        }


        public string ShelterEmail
        {
            get { return _petShelterEmail; }
            set
            {
                if (value.Equals(""))
                    _petShelterEmail = "-";
                else
                    _petShelterEmail = value;

                DispatcherHelper.CheckBeginInvokeOnUI(() => { RaisePropertyChanged("ShelterEmail"); });

            }
        }


        public string AppointmentName
        {
            get { return _appointmentName; }
            set
            {
                _appointmentName = value;
                CheckAppointment();
                DispatcherHelper.CheckBeginInvokeOnUI(() => { RaisePropertyChanged("AppointmentName"); });

            }
        }

        public string AppointmentEmail
        {
            get { return _appointmentEmail; }
            set
            {
                _appointmentEmail = value;
                CheckAppointment();
                DispatcherHelper.CheckBeginInvokeOnUI(() => { RaisePropertyChanged("AppointmentEmail"); });

            }
        }


        public bool AppointmentOk
        {
            get { return _appointmentOk; }
            set
            {
                _appointmentOk = value;
                DispatcherHelper.CheckBeginInvokeOnUI(() => { RaisePropertyChanged("AppointmentOk"); });

            }
        }

        public string AppointmentPhoneNumber
        {
            get { return _appointmentPhoneNumber; }
            set
            {
                _appointmentPhoneNumber = value;
                CheckAppointment();
                DispatcherHelper.CheckBeginInvokeOnUI(() => { RaisePropertyChanged("AppointmentPhoneNumber"); });

            }
        }

        public DateTime AppointmentDate
        {
            get { return _appointmentDate; }
            set
            {
                _appointmentDate = value;
                CheckAppointment();
                DispatcherHelper.CheckBeginInvokeOnUI(() => { RaisePropertyChanged("AppointmentDate"); });

            }
        }

        public void CheckAppointment()
        {
            if (Checker.check_user_email(AppointmentEmail) && Checker.check_user_name(AppointmentName) && Checker.check_user_phone_number(AppointmentPhoneNumber) && Checker.check_user_requested_date(AppointmentDate))
                DispatcherHelper.CheckBeginInvokeOnUI(() => 
                { 
                    AppointmentOk = true;
                    SetAppointmentButtonText();
                });

            else
                DispatcherHelper.CheckBeginInvokeOnUI(() => { 
                    AppointmentOk = false;
                    SetAppointmentButtonText();
                });
            
        }
        public string AppointmentButtonContent
        {
            get
            {
        return _appointmentButtonContent;
         }
            set
            {
                _appointmentButtonContent = value;
                DispatcherHelper.CheckBeginInvokeOnUI(() => { RaisePropertyChanged("AppointmentButtonContent"); });
            }

        }
   

        public void SetAppointmentButtonText()
        {
            if (AppointmentOk == true)
            {
                AppointmentButtonContent = "Confirm !";
            }
            else
            {
                AppointmentButtonContent = "Some fields are incorrect";
            }
        }

        public void ReinitializeAppointment()
        {
            AppointmentDate = System.DateTime.Now;
            AppointmentEmail = "";
            AppointmentName = "";
            AppointmentPhoneNumber = "";
        }



        public MainViewModel()
        {
            if (!ViewModelBase.IsInDesignModeStatic)
            {
                this.error_count = 0;
                Messenger.Default.Register<Parser>(this, process_fill);
                Messenger.Default.Register<string>("MEDIA_ENDED", process_random);

                Messenger.Default.Register<string>("NEXT_CLICKED", process_next);
                Messenger.Default.Register<string>("GOT_URI", process_play);
                Messenger.Default.Register<string>("EXCEPTION", process_exception);
                Messenger.Default.Register<Video>(this, process_video_changed);

                Messenger.Default.Register<string>("APPOINTMENT_CLICKED", process_appointment);
                Messenger.Default.Register<string>("APPOINTMENT_SUCCESS", process_appointment_failed);
                Messenger.Default.Register<string>("APPOINTMENT_FAILED", process_appointment_success);

                

                _appointmentDate = System.DateTime.Now;
                _appointmentEmail = "";
                _appointmentName = "";
                _appointmentPhoneNumber = "";
               
           

                theParser = new Parser(1);
                
                // DispatcherHelper.CheckBeginInvokeOnUI(() => { item.next();  });
                // WelcomeTitle = item.currentPet.pet_currentVideo.video_url;
            }
            else
            {
                AppointmentDate = System.DateTime.Now;
            }
                 
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
                    System.Diagnostics.Debug.WriteLine("EXCEPTION !" + e.Message);
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
                this.error_count++;

                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    if (error_count >= 3)
                    {
                        MessageBox.Show(NetworkErrorMessage);
                        error_count = 0;
                    }
                    else
                        Messenger.Default.Send<string>("MEDIA_ENDED"); 
                });

            }
        }

        void process_fill(Parser _parser)
        {
            if (!theParser.error_encountered)
            {

                ReinitializeAppointment();
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

                ShelterAddress = thePet.shelter_adress;
                ShelterEmail = thePet.shelter_email;
                ShelterName = thePet.shelter_name;
                ShelterPhoneNumber = thePet.shelter_phoneNumber;


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
                        System.Diagnostics.Debug.WriteLine("EXCEPTION !" + e.Message);
                        i++;
                        error = true;
                    }
                }
                while (error == true && i < 5);
            }
            else
            {
                this.error_count++;
                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    if (error_count >= 3)
                    {
                        MessageBox.Show(NetworkErrorMessage);
                        error_count = 0;
                    }
                    else
                        Messenger.Default.Send<string>("MEDIA_ENDED");
                });
            }

        }

        void process_random(string str)
        {
            if(str.Equals("MEDIA_ENDED"))
            {
            System.Threading.ThreadPool.QueueUserWorkItem(delegate
            {
                theParser.random();
            }, null);
            System.Diagnostics.Debug.WriteLine("Random requested");
            }
        }


        void process_appointment(string str)
        {
            if(str.Equals("APPOINTMENT_CLICKED"))
            {
                theAppointment = new Appointment(AppointmentName, AppointmentEmail, AppointmentPhoneNumber, AppointmentDate, thePet);
                theParser.post(theAppointment);

            }
        }

        void process_appointment_success(string str)
        {
            if (str.Equals("APPOINTMENT_SUCCESS"))
            {
                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                  {
                      MessageBox.Show("Your appointment was successfully set !");
                  });
            }

        }

        void process_appointment_failed(string str)
        {
            if (str.Equals("APPOINTMENT_FAILED"))
            {
                DispatcherHelper.CheckBeginInvokeOnUI(() =>
               {
                   MessageBox.Show("We could not set your appointment, please retry later");
               });
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