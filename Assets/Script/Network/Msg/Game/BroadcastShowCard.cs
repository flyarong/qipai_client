using System;
using Newtonsoft.Json;

namespace Network.Msg
{
    [Serializable]
    public class BroadcastShowCard : BaseMsg
    {
        public int code;
        public string msg;
        public GameInfo game;

        public BroadcastShowCard() : base(MsgID.BroadcastShowCard)
        {
        }

        public override void FromData(byte[] data)
        {
            var jsonString = System.Text.Encoding.UTF8.GetString(data);
            var jsonData = JsonConvert.DeserializeObject<BroadcastShowCard>(jsonString);
            this.code = jsonData.code;
            this.msg = jsonData.msg;
            this.game = jsonData.game;
        }

        public override byte[] ToData()
        {
            return new byte[0];
        }
    }
}