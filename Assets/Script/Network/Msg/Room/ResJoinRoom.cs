using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Network.Msg
{
    [Serializable]
    public class PlayerInfo
    {
        public int uid;
        public int deskId;
    }

    [Serializable]
    public class ResJoinRoom : BaseMsg
    {
        public int code;
        public string msg;
        public List<PlayerInfo> players;

        public ResJoinRoom() : base(MsgID.ResJoinRoom)
        {
        }

        public override void FromData(byte[] data)
        {
            var jsonString = System.Text.Encoding.UTF8.GetString(data);
            var jsonData = JsonConvert.DeserializeObject<ResJoinRoom>(jsonString);
            this.code = jsonData.code;
            this.msg = jsonData.msg;
            this.players = jsonData.players;
        }

        public override byte[] ToData()
        {
            return new byte[0];
        }
    }
}