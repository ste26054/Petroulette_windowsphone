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
        public string video_url { get; set; }
        public Uri video_uri { get; set; }

        static readonly object _locker = new object();
        static bool _go;
        Exception exception;

        public Video(string _url)
        {
            this.video_url = _url;
            
            //this.getVideoUri(); //This method needs bandwidth, maybe consider calling it only if needed ?


            //this.video_url = "http://www.youtube.com/watch?v=wXw6znXPfy4"; //Really short video    
            //this.video_url = "http://www.youtube.com/watch?v=NauzKqmJ6y8&feature=share"; //A cat.
    
        }

        public string getYoutubeId() //Method that uses a regexp to get the video id from any youtube url
        {

            Regex Youtube = new Regex(@"youtu(?:\.be|be\.com)/(?:.*v(?:/|=)|(?:.*/)?)([a-zA-Z0-9-_]+)");
            Match youtubeMatch = Youtube.Match(video_url);

            string id = "";

            if (youtubeMatch.Success)
                id = youtubeMatch.Groups[1].Value;
            else
                throw new Exception("get_youtube_id_exception");
            
            return id;
          //  return "1234";
        }

       public void getVideoUri() //Method that uses MyToolkit to get the actual .mp4 link to the youtube video
        {//There isn't actually others ways to directly play the video within our app (regular methods are using an external player
        // and we don't have any control on the video...
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
                Messenger.Default.Send<string>("GOT_URI");
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
