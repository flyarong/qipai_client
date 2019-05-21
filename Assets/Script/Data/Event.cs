

using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace Data
{

    public static class Event
    {
        static List<JSONObject> events = null;
        static Thread thread;
        private static void Add(JSONObject eventJson)
        {
            if (eventJson["data"]["events"].list == null)
            {
                return;
            }
            lock (events)
            {
                foreach (var e in eventJson["data"]["events"].list)
                {
                    events.Add(e);
                }
            }
        }

        public static JSONObject Get()
        {
            lock (events)
            {
                if (events.Count > 0)
                {
                    var e = events[0];
                    events.RemoveAt(0);
                    return e;
                }
                return null;
            }
        }

       

        public static void Start()
        {
            if (events != null)
            {
                return;
            }
            events = new List<JSONObject>();
            thread = new Thread(ThreadMethod);     //执行的必须是无返回值的方法
            thread.Name = "网络事件线程";
            thread.Start();
        }

        public static void Stop()
        {
            events = null;
            thread.Abort();
        }

        private static void ThreadMethod(object parameter) //方法内可以有参数，也可以没有参数
        {
            while (true)
            {
                var j = new Api.Common().GetEvent();
                if (j == null)
                {
                    continue;
                }
                if (j["code"]==null || j["code"].n != 0)
                {
                    continue;
                }
                Add(j);
            }
        }
    }

}