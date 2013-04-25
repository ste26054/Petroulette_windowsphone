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


namespace petroulette.parser
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
            catch (Exception)
            {
                error_encountered = true;
                System.Diagnostics.Debug.WriteLine("An exception occured while downloading /api/random Json !");
           
            }

          
        }
        private void processJsonPet(object sender, DownloadStringCompletedEventArgs e) //method that is invoked when download is finished
        {

            if (!e.Cancelled && e.Error == null)
            {
                System.Diagnostics.Debug.WriteLine("GOT PET JSON !!");
                string jsonString = (string)e.Result;
                this.downloadedJsonPet = jsonString; //Saves the downloaded Json

                Urls.setCookie(randomPet.getCookieContainer()); //keeps the session cookie

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
            jsonProcesserThread.Start();

            this.downloadNextJsonPet();

            jsonProcesserThread.Join();
            System.Diagnostics.Debug.WriteLine("Next finished !");
        }

        public void random() //method to call when we want to perform random
        {
            error_encountered = false;
            jsonProcesserThread = new System.Threading.Thread(process);
            jsonProcesserThread.Start();

            this.downloadJsonPet();

            jsonProcesserThread.Join();
            System.Diagnostics.Debug.WriteLine("Random finished !");
        }

        public Parser()
        {       }


    }

}
