using System;
using Newtonsoft.Json;

namespace Network.Msg
{
    public class ResClubGameStart : BaseMsg
    {
        public int code;
        public string msg;
        public int clubId;
        public int roomId;

        public ResClubGameStart() : base(MsgID.ResGameStart)
        {
        }

        public override void FromData(byte[] data)
        {
            var jsonString = System.Text.Encoding.UTF8.GetString(data);
            var jsonData = JsonConvert.DeserializeObject<ResClubGameStart>(jsonString);
            this.code = jsonData.code;
            this.msg = jsonData.msg;
            clubId = jsonData.clubId;
            roomId = jsonData.roomId;
        }

        public override byte[] ToData()
        {
            return new byte[0];
        }
    }
}