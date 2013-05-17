using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace petroulette.model.api
{
    static class Urls
    {
        private static string _url = "78.193.45.72:20081"; //server host + port (optionnal)


        private static string _apiRandom = "/api/random/";
        private static string _apiDetails = "/api/details/";
        private static string _apiNext = "/api/next/";
        private static string _apiAppointment = "/api/appointment/";

        private static CookieContainer _cookie = new CookieContainer();

        public static CookieContainer getCookie
        {
            get
            {
                return _cookie;
            }
        }

        public static void setCookie( CookieContainer lecookie)
        {
            _cookie = lecookie;
        }

        public static string getUrl
        {
            get
            {
                return _url;
            }
        }


        public static string getApiRandom
        {
            get
            {
                return _apiRandom;
            }
        }

        public static string getApiDetails
        {
            get
            {
                return _apiDetails;
            }
        }

        public static string getApiNext
        {
            get
            {
                return _apiNext;
            }
        }

        public static string getApiAppointment
        {
            get
            {
                return _apiAppointment;
            }
        }


    }
}
