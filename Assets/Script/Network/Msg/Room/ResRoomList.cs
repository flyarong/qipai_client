using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Network.Msg
{

    [Serializable]
    public class Room
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
    public class ResRoomList : BaseMsg
    {
        public int code;
        public string msg;
        public List<Room> rooms;

        public ResRoomList() : base(MsgID.ResRoomList)
        {
        }

        public override void FromData(byte[] data)
        {
            var jsonString = System.Text.Encoding.UTF8.GetString(data);
            var jsonData = JsonConvert.DeserializeObject<ResRoomList>(jsonString);
            this.code = jsonData.code;
            this.msg = jsonData.msg;
            this.rooms = jsonData.rooms;
        }

        public override byte[] ToData()
        {
            return new byte[0];
        }
    }
}