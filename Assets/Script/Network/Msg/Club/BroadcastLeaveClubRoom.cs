using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Network.Msg
{

    [Serializable]
    public class BroadcastLeaveClubRoom : BaseMsg
    {
        public int code;
        public string msg;
        public int clubId;
        public int roomId;
        public int deskId;
        public int tableId;
        public int uid;

        public BroadcastLeaveClubRoom() : base(MsgID.ResLeaveRoom)
        {
        }

        public override void FromData(byte[] data)
        {
            var jsonString = System.Text.Encoding.UTF8.GetString(data);
            var jsonData = JsonConvert.DeserializeObject<BroadcastLeaveClubRoom>(jsonString);
            this.code = jsonData.code;
            this.msg = jsonData.msg;
            this.clubId = jsonData.clubId;
            uid = jsonData.uid;
            roomId = jsonData.roomId;
            deskId = jsonData.deskId;
            tableId = jsonData.tableId;
        }

        public override byte[] ToData()
        {
            return new byte[0];
        }
    }
}