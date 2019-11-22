﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PracticalDebugingDemos.Demos.Performance
{
    class LongSynchronousNetworkRequest : DemoBase
    {
        public override void Start()
        {
            var url = "https://edition.cnn.com/";
            var sw = new Stopwatch();
            sw.Start();

            while (sw.Elapsed.TotalSeconds < 20)
            {
                SyncWebRequest(url);
            }
        }

        private async Task<int> GetAndCountInfo(string url) //Executes on UI threads
        {
            var client = new HttpClient();
            var resp = await client.GetAsync(new Uri(url)).ConfigureAwait(false);
            var content = await resp.Content.ReadAsStringAsync().ConfigureAwait(false);
            return content.Length;
        }

        private static void SyncWebRequest(string url)
        {
            HttpWebRequest request = WebRequest.CreateHttp(url);
            request.Method = "GET"; // or "POST", "PUT", "PATCH", "DELETE", etc.
            using (HttpWebResponse response = (HttpWebResponse) request.GetResponse())
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    // Get a reader capable of reading the response stream
                    using (StreamReader myStreamReader = new StreamReader(responseStream, Encoding.UTF8))
                    {
                        // Read stream content as string
                        string responseJSON = myStreamReader.ReadToEnd();

                        // Assuming the response is in JSON format, deserialize it
                        // creating an instance of TData type (generic type declared before).
                        //data = JsonConvert.DeserializeObject<TData>(responseJSON);
                    }
                }
            }
        }
    }
}
