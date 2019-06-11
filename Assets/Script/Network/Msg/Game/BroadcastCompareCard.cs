using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Network.Msg
{
    [Serializable]
    public class BroadcastCompareCard : BaseMsg
    {
        public int code;
        public string msg;
        public List<GameInfo> games;

        public BroadcastCompareCard() : base(MsgID.BroadcastCompareCard)
        {
        }

        public override void FromData(byte[] data)
        {
            var jsonString = System.Text.Encoding.UTF8.GetString(data);
            var jsonData = JsonConvert.DeserializeObject<BroadcastCompareCard>(jsonString);
            this.code = jsonData.code;
            this.msg = jsonData.msg;
            this.games = jsonData.games;
        }

        public override byte[] ToData()
        {
            return new byte[0];
        }
    }
}