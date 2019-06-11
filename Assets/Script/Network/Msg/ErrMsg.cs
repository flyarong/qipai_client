﻿using Newtonsoft.Json;

namespace Network.Msg
{
    public class ErrMsg : BaseMsg
    {
        public int code;
        public string msg;
        
        public ErrMsg(MsgID Id) : base(Id)
        {
        }

        public override void FromData(byte[] data)
        {
            var jsonString = System.Text.Encoding.UTF8.GetString(data);
            ResLogin jsonData = JsonConvert.DeserializeObject<ResLogin>(jsonString);
            code = jsonData.code;
            msg = jsonData.msg;
        }

        public override byte[] ToData()
        {
            return new byte[0];
        }
    }
}