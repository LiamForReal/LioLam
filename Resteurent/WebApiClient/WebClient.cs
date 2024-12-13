using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WebApiClient
{
    public class WebClient<T> : IWebClient<T>
    {
        UriBuilder uriBuilder; // duty to create get/post requests
        HttpClient client;
        HttpRequestMessage request;
        HttpResponseMessage response;

        public WebClient()
        {
            this.uriBuilder = new UriBuilder();
            this.client = new HttpClient();
            this.request = new HttpRequestMessage();
        }
        public string Scheme //protocol http in this case
        {
            set { this.uriBuilder.Scheme = value; }
        }

        public string host
        {
            set { this.uriBuilder.Host = value; }
        }

        public int port
        {
            set { this.uriBuilder.Port = value; }
        }

        public string path
        {
            set { this.uriBuilder.Path = value; }
        }

        public void AddParameter(string name, string value)
        {
            if(this.uriBuilder.Query == string.Empty)
            {
                this.uriBuilder.Query = "?";
            }
            else this.uriBuilder.Query += "&";
            this.uriBuilder.Query += $"{name}={value}";
        }
        public Task<T> Get()
        {
            throw new NotImplementedException();
        }

        public Task<bool> Post(T model)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Post(T model, Stream file)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Post(T model, List<Stream> file)
        {
            throw new NotImplementedException();
        }
    }
}
