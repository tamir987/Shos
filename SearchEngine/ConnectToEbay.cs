using SearchEngine.com.ebay.developer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace SearchEngine
{
    public class ConnectToEbay : FindingService
    {
        private readonly string m_APPID = "ErezDama-Shos-PRD-45d80d3bd-0db6688b";

        protected override System.Net.WebRequest GetWebRequest(Uri uri)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)base.GetWebRequest(uri);
                request.Headers.Add("X-EBAY-SOA-SECURITY-APPNAME", m_APPID);
                request.Headers.Add("X-EBAY-SOA-OPERATION-NAME", "findItemsByKeywords");
                request.Headers.Add("X-EBAY-SOA-GLOBAL-ID", "EBAY-GB");
                return request;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
