using Newtonsoft.Json;
using System;

namespace Network.Msg
{
    [Serializable]
    public class BroadcastSitRoom : BaseMsg
    {
        public int code;
        public string msg;
        public int roomId;
        public int uid;
        public int deskId;

        public BroadcastSitRoom() : base(MsgID.BroadcastSitRoom)
        {
        }

        public override void FromData(byte[] data)
        {
            var jsonString = System.Text.Encoding.UTF8.GetString(data);
            var jsonData = JsonConvert.DeserializeObject<BroadcastSitRoom>(jsonString);
            code = jsonData.code;
            msg = jsonData.msg;
            roomId = jsonData.roomId;
            uid = jsonData.uid;
            deskId = jsonData.deskId;
        }

        public override byte[] ToData()
        {
            return new byte[0];
        }
    }
}