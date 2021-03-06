﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Text.RegularExpressions;

using MyToolkit.Multimedia;
using MyToolkit.Networking;
using MyToolkit.Paging;
using MyToolkit.UI;
using GalaSoft.MvvmLight.Messaging;

namespace petroulette.model
{
     public class Video //Class concerning youtube videos
    {
        public string video_url { get; set; }   //original url of the youtube video
        public Uri video_uri { get; set; }  //real url of the youtube video (direct link)
        public string video_thumbnail { get; set; } //url to the video preview picture
        public string video_title { get; set; } //title of the video
        
        static readonly object _locker = new object(); //Semaphore
        static bool _go;    //Semaphore

        Exception exception;

        public Video(string _url, string _title)
        {
            this.video_url = _url;
            this.video_thumbnail = this.getThumbnail();
            this.video_title = _title;
            //this.getVideoUri(); //This method needs bandwidth, consider calling it only if needed

            //For testing purposes
            //this.video_url = "http://www.youtube.com/watch?v=wXw6znXPfy4"; //Really short video    
            //this.video_url = "http://www.youtube.com/watch?v=NauzKqmJ6y8&feature=share"; //A cat.
    
        }

        public string getThumbnail()
        {
            System.Diagnostics.Debug.WriteLine("GENERATED THUMBNAIL");
            return "http://img.youtube.com/vi/" + this.getYoutubeId() + "/0.jpg"; 

        }

        public string getYoutubeId() //Method that uses a regexp to get the video id from any youtube url
        {

            Regex Youtube = new Regex(@"youtu(?:\.be|be\.com)/(?:.*v(?:/|=)|(?:.*/)?)([a-zA-Z0-9-_]+)"); //regex that help to get video id
            Match youtubeMatch = Youtube.Match(video_url);

            string id = "";

            if (youtubeMatch.Success)
            {
                id = youtubeMatch.Groups[1].Value;
                System.Diagnostics.Debug.WriteLine("Video Url : http://www.youtube.com/watch?v=" + id);

            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Youtube id error");
                throw new Exception("get_youtube_id_exception");
            }
            return id;
        }

       public void getVideoUri() //Method that uses MyToolkit to get the actual .mp4 link to the youtube video
        {//There isn't actually others ways to directly play the video within our app (regular methods are using an external player
        // and we don't have any control on the video...)
            exception = null;
            
            Action < YouTube.YouTubeUri, Exception > Completed = downloadLinkCompleted;

            YouTube.GetVideoUri(this.getYoutubeId(), YouTubeQuality.Quality480P,Completed); //asynchronous method that will get the real link given a video quality

            System.Diagnostics.Debug.WriteLine("WAITING FOR VIDEO URI !");

            lock (_locker)
                while (!_go)
                {
                    Monitor.Wait(_locker);    //Wait for action to finish
                }
            if (exception == null)
            {
                System.Diagnostics.Debug.WriteLine("GOT VIDEO URI !");
                _go = false;

                Messenger.Default.Send<string>("GOT_URI"); //Sending message that we got uri
            }
            else
            {
                _go = false;
                throw new Exception("get_video_uri_exception");
            }
        }

       public void downloadLinkCompleted(YouTube.YouTubeUri entry, Exception excep)
       {
          
              if (excep != null)
               {
                  System.Diagnostics.Debug.WriteLine("EXCEPTION_1 IN GET VIDEO URI FROM YOUTUBE !!");
                  exception = new Exception("get_youtube_uri_exception");
                  lock (_locker)
                  {
                      _go = true;
                      Monitor.Pulse(_locker);
                  }
               }
               else
               {
                   if (entry != null)
                   {
                       this.video_uri = new Uri(entry.Uri.AbsoluteUri);
                       System.Diagnostics.Debug.WriteLine(this.video_uri);

                       lock (_locker)
                       {
                           _go = true;
                           Monitor.Pulse(_locker);
                       }


                  }
               }
           
          
       }

        


    }
}
