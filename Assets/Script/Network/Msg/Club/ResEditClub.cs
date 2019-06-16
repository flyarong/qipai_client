using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Network.Msg
{

    [Serializable]
    public class BroadcastEditClub : BaseMsg
    {
        public int code;
        public string msg;
        public int clubId;

        public BroadcastEditClub() : base(MsgID.BroadcastEditClub)
        {
        }

        public override void FromData(byte[] data)
        {
            var jsonString = System.Text.Encoding.UTF8.GetString(data);
            var jsonData = JsonConvert.DeserializeObject<BroadcastEditClub>(jsonString);
            this.code = jsonData.code;
            this.msg = jsonData.msg;
            this.clubId = jsonData.clubId;
        }

        public override byte[] ToData()
        {
            return new byte[0];
        }
    }
}