using Newtonsoft.Json;
using System;

namespace Network.Msg
{
    [Serializable]
    public class BroadcastGameOver : BaseMsg
    {
        public int code;
        public string msg;
        public int roomId;

        public BroadcastGameOver() : base(MsgID.BroadcastGameOver)
        {
        }

        public override void FromData(byte[] data)
        {
            var jsonString = System.Text.Encoding.UTF8.GetString(data);
            var jsonData = JsonConvert.DeserializeObject<BroadcastGameOver>(jsonString);
            code = jsonData.code;
            msg = jsonData.msg;
            roomId = jsonData.roomId;
        }

        public override byte[] ToData()
        {
            return new byte[0];
        }
    }
}