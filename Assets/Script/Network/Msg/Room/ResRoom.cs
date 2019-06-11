using System;
using Newtonsoft.Json;

namespace Network.Msg
{
    [Serializable]
    public class RoomInfo
    {
        public int id;
        public int score;
        public int pay;
        public int current;
        public int count;
        public int uid;
        public int players;
    }


    [Serializable]
    public class ResRoom : BaseMsg
    {
        public int code;
        public string msg;
        public RoomInfo room;

        public ResRoom() : base(MsgID.ResRoom)
        {
        }

        public override void FromData(byte[] data)
        {
            var jsonString = System.Text.Encoding.UTF8.GetString(data);
            var jsonData = JsonConvert.DeserializeObject<ResRoom>(jsonString);
            this.code = jsonData.code;
            this.msg = jsonData.msg;
            this.room = jsonData.room;
        }

        public override byte[] ToData()
        {
            return new byte[0];
        }
    }


}