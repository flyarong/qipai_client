using Network.Msg;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Utils
{
    public class Msg
    {
        Dictionary<string, object> dic;
        MsgID msgID;

        public Msg(MsgID msgID)
        {
            this.msgID = msgID; ;
            this.dic = new Dictionary<string, object>();
        }

        public Msg Add(string key, object obj)
        {
            dic.Add(key, obj);
            return this;
        }

        private string toJson()
        {
            return JsonConvert.SerializeObject(dic);
        }

        private Network.Message toMessage()
        {
            var json = toJson();
            var data = System.Text.Encoding.UTF8.GetBytes(json);
            return new Network.Message(Convert.ToInt32(msgID), data);
        }

        /// <summary>
        /// 把消息封打包并发送
        /// </summary>
        public void Send()
        {
            Network.Manager.Inst.SendMessage(toMessage());
        }
    }
}