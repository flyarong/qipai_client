using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Network.Msg
{

    [Serializable]
    public class ResExitClub : BaseMsg
    {
        public int code;
        public string msg;
        public int clubId;
        public int uid;

        public ResExitClub() : base(MsgID.ResExitClub)
        {
        }

        public override void FromData(byte[] data)
        {
            var jsonString = System.Text.Encoding.UTF8.GetString(data);
            var jsonData = JsonConvert.DeserializeObject<ResExitClub>(jsonString);
            this.code = jsonData.code;
            this.msg = jsonData.msg;
            this.clubId = jsonData.clubId;
            uid = jsonData.uid;
        }

        public override byte[] ToData()
        {
            return new byte[0];
        }
    }
}