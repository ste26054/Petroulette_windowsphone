using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;
using System.IO;

namespace petroulette.model.api
{
    public class CookieAwareWebClient : WebClient//Class that adds cookie support to WebClient.
    {
        private CookieContainer m_container = new CookieContainer();

        protected override WebRequest GetWebRequest(Uri address)
        {
            WebRequest request = base.GetWebRequest(address);
            if (request is HttpWebRequest)
            {
                (request as HttpWebRequest).CookieContainer = this.m_container;
            }
            HttpWebRequest httpRequest = (HttpWebRequest)request;
            
            return httpRequest;
        }

        public CookieContainer GetCookieContainer()
        {
            return this.m_container;
        }
        public void SetCookieContainer(CookieContainer n)
        {
            this.m_container = n;
        }

        public CookieAwareWebClient() : base()
        {}

        public CookieAwareWebClient(CookieContainer theCookie) : base()
        {
            this.SetCookieContainer(theCookie);
        }

    }
}
