using Newtonsoft.Json;
using System;

namespace Network.Msg
{
    [Serializable]
    public class ResDeleteRoom : BaseMsg
    {
        public int code;
        public string msg;
        public int roomId;

        public ResDeleteRoom() : base(MsgID.ResDeleteRoom)
        {
        }

        public override void FromData(byte[] data)
        {
            var jsonString = System.Text.Encoding.UTF8.GetString(data);
            var jsonData = JsonConvert.DeserializeObject<ResDeleteRoom>(jsonString);
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