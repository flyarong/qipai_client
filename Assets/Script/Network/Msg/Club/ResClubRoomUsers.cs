using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Network.Msg
{
    
    public class ClubRoomUser
    {
        public int uid;
        public string nick;
        public string avatar;
        public int deskId;
    }


    public class ResClubRoomUsers : BaseMsg
    {
        public int code;
        public string msg;
        public int roomId;
        public int clubId;
        public int tableId;
        public List<ClubRoomUser> users;

        public ResClubRoomUsers() : base(MsgID.ResClubRoomUsers)
        {
        }

        public override void FromData(byte[] data)
        {
            var jsonString = System.Text.Encoding.UTF8.GetString(data);
            var jsonData = JsonConvert.DeserializeObject<ResClubRoomUsers>(jsonString);
            this.code = jsonData.code;
            this.msg = jsonData.msg;
            roomId = jsonData.roomId;
            clubId = jsonData.clubId;
            tableId = jsonData.tableId;
            this.users = jsonData.users;
        }

        public override byte[] ToData()
        {
            return new byte[0];
        }
    }
}