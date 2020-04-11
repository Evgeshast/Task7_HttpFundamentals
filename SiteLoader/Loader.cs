using System;
using System.Net.Http;

namespace SiteLoader
{
    public class Loader
    {
        public HttpResponseMessage GetResponse(string url)
        {
            HttpResponseMessage response;
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(url);
                response = httpClient.GetAsync(new Uri(url)).Result;
            }

            return response;
        }
    }
}