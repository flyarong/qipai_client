using Newtonsoft.Json;
using System;

namespace Network.Msg
{

    [Serializable]
    public class ResLeaveRoom : BaseMsg
    {
        public int code;
        public string msg;
        public int roomId;
        public int uid;
        public int newBoss;

        public ResLeaveRoom() : base(MsgID.ResLeaveRoom)
        {
        }

        public override void FromData(byte[] data)
        {
            var jsonString = System.Text.Encoding.UTF8.GetString(data);
            var jsonData = JsonConvert.DeserializeObject<ResLeaveRoom>(jsonString);
            code = jsonData.code;
            msg = jsonData.msg;
            roomId = jsonData.roomId;
            uid = jsonData.uid;
            newBoss = jsonData.newBoss;
        }

        public override byte[] ToData()
        {
            return new byte[0];
        }
    }
}