using SnapsLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;

namespace XAMLSnaps
{
    public partial class SnapsManager 
    {
        public async static Task<string> GetPageAsString(string url)
        {

            string result = "";
            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromMilliseconds(5000);
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                // Add a user agent setting that allows us to scrape web pages
                request.Headers.Add("User-Agent", "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)");
                using (var response = await client.SendAsync(request, HttpCompletionOption.ResponseContentRead))
                {
                    using (var body = await response.Content.ReadAsStreamAsync())
                    using (var reader = new StreamReader(body))
                    {
                        result = reader.ReadToEnd();
                    }
                }
            }
            return result;
        }

        public string GetWebPageAsString(string url)
        {
            AutoResetEvent GotWebPage = new AutoResetEvent(false);
            Exception errorException = null;

            string webPageText="";

            IAsyncAction asyncAction = Windows.System.Threading.ThreadPool.RunAsync(
            async (workItem) =>
            {
                try
                {
                    webPageText = await GetPageAsString(url);
                }
                catch (Exception e)
                {
                    errorException = e;
                }
                finally
                {
                    GotWebPage.Set();
                }
            });

            GotWebPage.WaitOne();

            if (errorException != null)
                throw errorException;

            return webPageText;
        }
    }
}
