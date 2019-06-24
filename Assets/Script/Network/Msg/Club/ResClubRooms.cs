using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Network.Msg
{

    public class ResClubRooms : BaseMsg
    {
        public int code;
        public string msg;
        public List<RoomInfo> rooms;

        public ResClubRooms() : base(MsgID.ResClubRooms)
        {
        }

        public override void FromData(byte[] data)
        {
            var jsonString = System.Text.Encoding.UTF8.GetString(data);
            var jsonData = JsonConvert.DeserializeObject<ResClubRooms>(jsonString);
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