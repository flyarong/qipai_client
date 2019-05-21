
namespace Api
{
    using System.Net.Http;
    using System;
    using System.Collections.Generic;
    public class Client : HttpClient
    {
        private static Client inst;
        private string baseUrl = "http://192.168.3.10";

        public static Client Inst
        {
            get
            {
                System.Net.ServicePointManager.Expect100Continue = false;
                string token = Data.User.Token;
                return token == "" ? new Client() : new Client(token);
            }
        }

        public Client()
        {
            this.DefaultRequestHeaders.Add("user-agent", "qipai");
        }

        ~Client()
        {
            this.Dispose();
        }

        public Client(string token)
        {
            this.DefaultRequestHeaders.Add("user-agent", "qipai");
            if (token != "")
            {
                this.DefaultRequestHeaders.Add("token", token);
            }


        }

        /// <summary>
        /// Get请求返回Response
        /// </summary>
        /// <param name="url">要请求的地址</param>
        /// <returns>返回response</returns>
        public HttpResponseMessage Get(string url)
        {
            try
            {
                return this.GetAsync(this.baseUrl + url).Result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// get请求获取返回的内容
        /// </summary>
        /// <param name="url">要请求的地址</param>
        /// <returns>返回网页内容</returns>
        public string GetContent(string url)
        {
            var ret = this.Get(url);
            if (ret == null)
            {
                return null;
            }
            return ret.Content.ReadAsStringAsync().Result;
        }

        public byte[] GetBytes(string url)
        {
            return this.Get(url).Content.ReadAsByteArrayAsync().Result;
        }

        public HttpResponseMessage Post(string url, List<KeyValuePair<string, string>> paramList)
        {
            try
            {
                if (paramList == null)
                {
                    paramList = new List<KeyValuePair<string, string>>();
                    paramList.Add(new KeyValuePair<string, string>("-", ""));
                }
                return this.PostAsync(new Uri(this.baseUrl + url), new FormUrlEncodedContent(paramList)).Result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public string PostContent(string url, List<KeyValuePair<string, string>> paramList)
        {
            var ret = this.Post(url, paramList);
            if (ret == null)
            {
                return null;
            }
            return ret.Content.ReadAsStringAsync().Result;
        }

        public HttpResponseMessage Put(string url, List<KeyValuePair<string, string>> paramList)
        {
            try
            {
                if (paramList == null)
                {
                    paramList = new List<KeyValuePair<string, string>>();
                    paramList.Add(new KeyValuePair<string, string>("", ""));
                }
                return this.PutAsync(new Uri(this.baseUrl + url), new FormUrlEncodedContent(paramList)).Result;
            }
            catch (Exception ex)
            {
                return null;
            }
            
        }

        public string PutContent(string url, List<KeyValuePair<string, string>> paramList)
        {
            var ret = this.Put(url, paramList);
            if (ret == null)
            {
                return null;
            }
            return ret.Content.ReadAsStringAsync().Result;
        }

        public HttpResponseMessage Del(string url)
        {
            return this.DeleteAsync(this.baseUrl + url).Result;
        }

        public string DelContent(string url)
        {
            try
            {
                var ret = this.Del(url);
                if (ret == null)
                {
                    return null;
                }
                return ret.Content.ReadAsStringAsync().Result;
            }
            catch (Exception ex)
            {
                return null;
            }
            
        }

    }
}