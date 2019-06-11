using System;
using Newtonsoft.Json;

namespace Network.Msg
{
    [Serializable]
    public class BroadcastScore : BaseMsg
    {
        public int code;
        public string msg;
        public GameInfo game;

        public BroadcastScore() : base(MsgID.BroadcastScore)
        {
        }

        public override void FromData(byte[] data)
        {
            var jsonString = System.Text.Encoding.UTF8.GetString(data);
            var jsonData = JsonConvert.DeserializeObject<BroadcastScore>(jsonString);
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