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
        protected override System.Net.WebRequest GetWebRequest(Uri uri)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)base.GetWebRequest(uri);
                request.Headers.Add("X-EBAY-SOA-SECURITY-APPNAME", "ErezDama-Shos-PRD-45d80d3bd-0db6688b");
                request.Headers.Add("X-EBAY-SOA-OPERATION-NAME", "findItemsByKeywords");
                request.Headers.Add("X-EBAY-SOA-SERVICE-NAME", "FindingService");
                request.Headers.Add("X-EBAY-SOA-MESSAGE-PROTOCOL", "SOAP11");
                request.Headers.Add("X-EBAY-SOA-SERVICE-VERSION", "1.0.0");
                request.Headers.Add("X-EBAY-SOA-GLOBAL-ID", "EBAY-US");
                return request;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
