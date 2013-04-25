using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;

namespace petroulette.model.api
{
    public class CookieAwareWebClient : WebClient  //Class that adds cookie support to WebClient.
    {
        private CookieContainer m_container = new CookieContainer();
        protected override WebRequest GetWebRequest(Uri address)
        {

            WebRequest request = base.GetWebRequest(address);
            HttpWebRequest webRequest = request as HttpWebRequest;
            if (webRequest != null)
            {
                webRequest.CookieContainer = m_container;
            }
            return request;
        }

        public CookieContainer getCookieContainer()
        {
            return this.m_container;
        }
        public void setCookieContainer(CookieContainer n)
        {
            this.m_container = n;
        }

        public CookieAwareWebClient() : base()
        {}

        public CookieAwareWebClient(CookieContainer theCookie) : base()
        {
            this.setCookieContainer(theCookie);
        }

    }
}
