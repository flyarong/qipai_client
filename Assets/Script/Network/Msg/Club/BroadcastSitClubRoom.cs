using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Network.Msg
{

    [Serializable]
    public class BroadcastSitClubRoom : BaseMsg
    {
        public int code;
        public string msg;
        public int clubId;
        public int roomId;
        public int deskId;
        public int tableId;
        public string avatar;
        public string nick;
        public int uid;

        public BroadcastSitClubRoom() : base(MsgID.BroadcastSitRoom)
        {
        }

        public override void FromData(byte[] data)
        {
            var jsonString = System.Text.Encoding.UTF8.GetString(data);
            var jsonData = JsonConvert.DeserializeObject<BroadcastSitClubRoom>(jsonString);
            this.code = jsonData.code;
            this.msg = jsonData.msg;
            this.clubId = jsonData.clubId;
            uid = jsonData.uid;
            roomId = jsonData.roomId;
            deskId = jsonData.deskId;
            avatar = jsonData.avatar;
            tableId = jsonData.tableId;
            nick = jsonData.nick;
        }

        public override byte[] ToData()
        {
            return new byte[0];
        }
    }
}