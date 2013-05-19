using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Threading;
using System.ComponentModel;
using System.Windows.Input;

using Newtonsoft.Json;

using petroulette.model;
using petroulette.model.api;
using petroulette.model.parser;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.ObjectModel;


namespace petroulette.model.parser
{


  public  class Parser 
    {

      public Thread jsonProcesserThread;


      public Pet currentPet { get; set; } //The current Pet

      public string downloadedJsonPet { get; set; } //String of the Json Pet
      public string downloadedJsonPetDetails { get; set; } //String of the Json details

      public bool error_encountered { get; set; }
      public Generic_RandomPet generic_randomPet { get; set; } //A generic pet
      public Generic_PetDetails generic_petDetails { get; set; } //And his generic details
      public Generic_Appointment generic_Appointment { get; set; }

      CookieAwareWebClient randomPet; 
      CookieAwareWebClient nextPet;


      //Semaphores
      static readonly object _locker = new object();
    
      static bool _go;

      
        public void downloadJsonPet()
        {

            randomPet = new CookieAwareWebClient(); //creates a WebClient with cookie support
            
           
            Uri URL = new Uri("http://" + Urls.getUrl + Urls.getApiRandom); //Builds the url to server
            randomPet.DownloadStringCompleted += new DownloadStringCompletedEventHandler(processJsonPet); //adds new event
            
            try
            {
                randomPet.DownloadStringAsync(URL); //Download randomPetJson asynchronously
                
            }
            catch (Exception e)
            {
                error_encountered = true;
                System.Diagnostics.Debug.WriteLine("An exception occured while downloading /api/random Json !" + e.Message);
                lock (_locker)
                {
                    _go = true;
                    Monitor.Pulse(_locker); //Notify that download is finished so we can start parsing
                }
                throw new Exception("dl_json_pet_exception");
            }

          
        }
        private void processJsonPet(object sender, DownloadStringCompletedEventArgs e) //method that is invoked when download is finished
        {

            if (!e.Cancelled && e.Error == null)
            {
                System.Diagnostics.Debug.WriteLine("GOT PET JSON !!");
                string jsonString = (string)e.Result;
                this.downloadedJsonPet = jsonString; //Saves the downloaded Json

                Urls.setCookie(randomPet.GetCookieContainer()); //keeps the session cookie
               // Urls.setCookie(randomPet.
                this.downloadJsonPetDetails(); //downloads the details part of pet
            }
            else
            {
                error_encountered = true;
                System.Diagnostics.Debug.WriteLine("ERROR IN JSON_PET");
           

                lock (_locker)
                {
                    _go = true;
                    Monitor.Pulse(_locker); //Notify that download is finished so we can start parsing
                }
                _go = false;

                Messenger.Default.Send<string>("EXCEPTION");
                //throw new Exception("process_json_pet_exception");
            }
        }
        public void downloadJsonPetDetails()
        {
            CookieAwareWebClient petDetails = new CookieAwareWebClient(Urls.getCookie); //Creates a new webclient with saved cookie session

            Uri URL = new Uri("http://" + Urls.getUrl + Urls.getApiDetails); //builds url


            petDetails.DownloadStringCompleted += processJsonPetDetails;
            try
            {
                petDetails.DownloadStringAsync(URL);
            }
            catch (Exception)
            {
                error_encountered = true;
                System.Diagnostics.Debug.WriteLine("An exception occured while downloading /api/details Json !");
                lock (_locker)
                {
                    _go = true;
                    Monitor.Pulse(_locker); //Notify that download is finished so we can start parsing
                }
                throw new Exception("dl_json_pet_details_exception");
            }

        }
        private void processJsonPetDetails(object sender, DownloadStringCompletedEventArgs e)
        {
            if (!e.Cancelled && e.Error == null)
            {
                System.Diagnostics.Debug.WriteLine("GOT JSON  FOR DETAILS!!");
                string textString = (string)e.Result;


                this.downloadedJsonPetDetails = textString; //saves the JSonPetDetails


                lock (_locker)
                {
                    _go = true;
                    Monitor.Pulse(_locker); //Notify that download is finished so we can start parsing
                }

            }
            else
            {
                System.Diagnostics.Debug.WriteLine("ERROR IN PROCESS_JSON_DETAILS");
                error_encountered = true;
                lock (_locker)
                {
                    _go = true;
                    Monitor.Pulse(_locker); //Notify that download is finished so we can start parsing
                }
                _go = false;
                //throw new Exception("process_json_pet_details_exception");
                Messenger.Default.Send<string>("EXCEPTION");
            }
        }


        public void downloadNextJsonPet() //Downloads Json with next pet
        {

            nextPet = new CookieAwareWebClient(Urls.getCookie); //Creates a new webClient with the saved cookie

            Uri URL = new Uri("http://" + Urls.getUrl + Urls.getApiNext);   //creates url to /api/next
            nextPet.DownloadStringCompleted += new DownloadStringCompletedEventHandler(processJsonPet);
            try
            {
                nextPet.DownloadStringAsync(URL);
            }
            catch (Exception)
            {
                error_encountered = true;
                System.Diagnostics.Debug.WriteLine("An exception occured while downloading /api/next Json !");
                lock (_locker)
                {
                    _go = true;
                    Monitor.Pulse(_locker); //Notify that download is finished so we can start parsing
                }
                throw new Exception("dl_next_json_pet_exception");
            }

        }



        public void process() //method that will handle downloaded jsons
        {


            lock (_locker)
                while (!_go)
                {
                    Monitor.Wait(_locker);    //Wait for download completion
                }

            if (!error_encountered)
            {

                this.generic_randomPet = new Generic_RandomPet(this.downloadedJsonPet); //Generic pet that fits to it's Json Structure
                this.generic_petDetails = new Generic_PetDetails(this.downloadedJsonPetDetails); //Generic pet details that fits to it's Json Structure



                currentPet = Checker.createPet(generic_randomPet, generic_petDetails);


                System.Diagnostics.Debug.WriteLine("DONE PROCESS 2 !!");


            }
            _go = false;
        }


       

 

        public void next() //method to call when we want to perform next
        {
            error_encountered = false;

            jsonProcesserThread = new System.Threading.Thread(process);
            try
            {
                jsonProcesserThread.Start();//the process method will wait for the following method to finish
            }
            catch (Exception)
            {

            }
            try
            {
                this.downloadNextJsonPet();
            }
            catch (System.Net.WebException e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
                throw new Exception("next_exception");
                
            }


            jsonProcesserThread.Join();//then we wait for the thread itself to finish
            System.Diagnostics.Debug.WriteLine("Next finished !");
           // Messenger.Default.Send<Parser>(this);
           //Messenger.Default.Send<string>("NEXT_FINISHED");
           Messenger.Default.Send<Parser>(this);
        }

        public void random() //method to call when we want to perform random
        {
           // next();
           error_encountered = false;
            jsonProcesserThread = new System.Threading.Thread(process);

            try
            {
                jsonProcesserThread.Start();//the process method will wait for the following method to finish
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Exception !" + e.Message);
            }

            try
            {
                this.downloadJsonPet();
            }
            catch (WebException e)
            {
                System.Diagnostics.Debug.WriteLine(e.Status);
            }

            jsonProcesserThread.Join();//then we wait for the thread itself to finish
            System.Diagnostics.Debug.WriteLine("Random finished !");
            

            //MESSENGER
            Messenger.Default.Send<Parser>(this);
         
        }




        public void post(Appointment a)
        {
            
            randomPet.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
            var uri = new Uri("http://" + Urls.getUrl + Urls.getApiAppointment, UriKind.Absolute);

            StringBuilder postData = new StringBuilder();

            postData.AppendFormat("{0}={1}", "name", HttpUtility.UrlEncode(a.user_name));
            postData.AppendFormat("&{0}={1}", "email", HttpUtility.UrlEncode(a.user_email));
            postData.AppendFormat("&{0}={1}", "contact_number", HttpUtility.UrlEncode(a.user_phoneNumber));
            postData.AppendFormat("&{0}={1}", "time", HttpUtility.UrlEncode("TIME NOW"));                           //TODO
            postData.AppendFormat("&{0}={1}", "date", HttpUtility.UrlEncode(a.requested_date.ToShortDateString())); //TODO

            randomPet.Headers[HttpRequestHeader.ContentLength] = postData.Length.ToString();
            randomPet.UploadStringCompleted += new UploadStringCompletedEventHandler(processAppointmentResult);

            randomPet.UploadStringAsync(uri, "POST", postData.ToString());
        }

        private void processAppointmentResult(object sender, UploadStringCompletedEventArgs e)
        {

            //System.Diagnostics.Debug.WriteLine("APPPOINTMENT SEND, RESPONSE FROM SERVER : " + e.Result);
            this.generic_Appointment = new Generic_Appointment(e.Result);
            string result = generic_Appointment.genericAppointment.data.appointment_result;
            System.Diagnostics.Debug.WriteLine("RESPONSE FROM SERVER : " + result);

            if (result.Equals("False"))
            {
                Messenger.Default.Send<string>("APPOINTMENT_SUCCESS"); //TODO : Change to APPOINTMENT_FALSE When server will be OK
            }
            else if(result.Equals("True"))
            {
                Messenger.Default.Send<string>("APPOINTMENT_SUCCESS");
            }
            else
            {
                Messenger.Default.Send<string>("APPOINTMENT_FAILED");
            }


        }
        
        public Parser(){}

        public Parser(int a)
        {
           Thread thread = new System.Threading.Thread(random);
           thread.Start();
        }

    }


  public class videoCollection
  {
      public ObservableCollection<Video> Videos
      {
          get { return _videos; }
      }
      Pet pet;
      private readonly ObservableCollection<Video> _videos = new ObservableCollection<Video>();

      public videoCollection(Pet _pet)
      {
          pet = _pet;
          var list = new List<Video>();
          list = pet.pet_videoList;
          //var oc = new ObservableCollection<Video>();
          foreach (Video v in list)
          {
              //v.video_thumbnail = "http://img.youtube.com/vi/FMVVDm1mwvM/0.jpg";
              _videos.Add(v);
          }
          
      }
  }

}
