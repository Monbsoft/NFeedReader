using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml;

namespace NFeedReader.Services
{
    public class RssReader
    {
        private static ILogger<RssReader> _logger;


        public RssReader(ILogger<RssReader> logger)
        {
            _logger = logger;
        }

        public XmlNode Open(string uri)
        {
            try
            {
                _logger.LogDebug($"Opening rss {uri}...");
                var client = new WebClient();
                client.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows NT 10.0; WOW64; Trident/7.0; rv:11.0) like Gecko");
                using (var reader = new XmlTextReader(client.OpenRead(uri)))
                {
                    XmlDocument document = new XmlDocument();
                    document.Load(reader);
                    XmlNamespaceManager nsmgr = new XmlNamespaceManager(document.NameTable);
                    nsmgr.AddNamespace("media", "urn:newbooks-schema");
                    return document;
                }
            }
            catch(WebException ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }
    }
}
