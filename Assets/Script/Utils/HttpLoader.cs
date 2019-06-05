using FairyGUI;
using UnityEngine;
using Api;
using System.IO;
using System.Net.Http;

namespace Utils
{
    public class HttpLoader : GLoader
    {
        
        protected override void LoadExternal()
        {
            // 这里主要是区分在家本地资源和网络资源，
            // 因为我的网络url是 /static开头的，所以在这里做个判断。
            // 如果不判断，本地资源就无法加载，导致场景无法正确加载
            if (!url.Contains("/static/"))
            {
                base.LoadExternal();
                return;
            }

            var client = new HttpClient();
            byte[] imgBytes = client.GetAsync("http://192.168.1.103:9988" + url).Result.Content.ReadAsByteArrayAsync().Result;
            Texture2D tex = new Texture2D(0, 0);
            tex.LoadImage(imgBytes);
            tex.Apply();
            if (tex != null)
                onExternalLoadSuccess(new NTexture(tex));
            else
                onExternalLoadFailed();
        }
    }
}