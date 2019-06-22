using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Network.Msg
{
    public class ClubPlayer
    {
        public int uid;
        public string nick;
        public string avatar;
        public int deskId;
    }

    [Serializable]
    public class ResGamePlayers : BaseMsg
    {
        public int code;
        public string msg;
        public int roomId;
        public List<ClubPlayer> players;

        public ResGamePlayers() : base(MsgID.ResGamePlayers)
        {
        }

        public override void FromData(byte[] data)
        {
            var jsonString = System.Text.Encoding.UTF8.GetString(data);
            var jsonData = JsonConvert.DeserializeObject<ResGamePlayers>(jsonString);
            code = jsonData.code;
            msg = jsonData.msg;
            roomId = jsonData.roomId;
            players = jsonData.players;
        }

        public override byte[] ToData()
        {
            return new byte[0];
        }
    }
}