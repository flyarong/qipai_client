using UnityEngine;
using System;
using Newtonsoft.Json;

namespace Network.Msg
{
    public class UserInfo
    {
        
        public int id;
        public string nick;
        public string avatar;
        public string ip;
        public string address;
        public int card;
        public DateTime createAt;
    }

    [Serializable]
    public class ResUserInfo : BaseMsg
    {
        public int code;
        public string msg;
        public UserInfo user;

        public ResUserInfo() : base(MsgID.ResUserInfo)
        {
        }

        public override void FromData(byte[] data)
        {
            var jsonString = System.Text.Encoding.UTF8.GetString(data);
            var jsonData = JsonConvert.DeserializeObject< ResUserInfo >(jsonString);
            code = jsonData.code;
            msg = jsonData.msg;
            user = jsonData.user;
        }

        public override byte[] ToData()
        {
            return new byte[0];
        }
    }
}