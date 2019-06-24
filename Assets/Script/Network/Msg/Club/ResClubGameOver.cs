using System;
using Newtonsoft.Json;

namespace Network.Msg
{
    public class ResClubGameOver : BaseMsg
    {
        public int code;
        public string msg;
        public int clubId;
        public int roomId;
        public int tableId;

        public ResClubGameOver() : base(MsgID.BroadcastGameOver)
        {
        }

        public override void FromData(byte[] data)
        {
            var jsonString = System.Text.Encoding.UTF8.GetString(data);
            var jsonData = JsonConvert.DeserializeObject<ResClubGameOver>(jsonString);
            this.code = jsonData.code;
            this.msg = jsonData.msg;
            clubId = jsonData.clubId;
            roomId = jsonData.roomId;
            tableId = jsonData.tableId;
        }

        public override byte[] ToData()
        {
            return new byte[0];
        }
    }
}