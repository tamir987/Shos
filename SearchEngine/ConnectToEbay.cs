using SearchEngine.com.ebay.developer;
using System;
using System.Net;

namespace SearchEngine
{
    public class ConnectToEbay : FindingService
    {
        private readonly string m_APPID = "ErezDama-Shos-SBX-e5d7504c4-1ac6a8f7";

        protected override WebRequest GetWebRequest(Uri i_Uri)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)base.GetWebRequest(i_Uri);
                request.Headers.Add("X-EBAY-SOA-OPERATION-NAME", "findItemsByKeywords");
                request.Headers.Add("X-EBAY-SOA-SERVICE-VERSION", "1.0.0");
                request.Headers.Add("X-EBAY-SOA-SECURITY-APPNAME", m_APPID);
                request.Headers.Add("X-EBAY-SOA-RESPONSE-DATA-FORMAT", "XML");
                //request.Headers.Add("X-EBAY-SOA-REST", "PAYLOAD");
                request.Headers.Add("X_EBAY_SOA-SERVICE_NAME", "FindingService");
                request.Headers.Add("X-EBAY-SOA-MESSAGE-PROTOCOL", "SOAP11");
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
