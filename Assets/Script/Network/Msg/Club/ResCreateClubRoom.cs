using UnityEngine;
using System;
using Newtonsoft.Json;

namespace Network.Msg
{

    public class ResCreateClubRoom : BaseMsg
    {
        public int code;
        public string msg;
        public int uid;
        public int clubId;
        public int roomId;

        public ResCreateClubRoom() : base(MsgID.ResCreateClubRoom)
        {
        }

        public override void FromData(byte[] data)
        {
            var jsonString = System.Text.Encoding.Default.GetString(data);
            var jsonData = JsonConvert.DeserializeObject<ResCreateClubRoom>(jsonString);
            this.code = jsonData.code;
            this.msg = jsonData.msg;
            this.uid = jsonData.uid;
            this.clubId = jsonData.clubId;
            this.roomId = jsonData.roomId;
        }

        public override byte[] ToData()
        {
            return new byte[0];
        }
    }

}