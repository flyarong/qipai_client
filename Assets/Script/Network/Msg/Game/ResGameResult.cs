using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Network.Msg
{
    public class GameResult
    {
        public int uid;
        public string nick;
        public string avatar;
        public int banks;
        public int totalScore;
    }

    public class RoomGameResult
    {
        public int id;
        public int players;
        public int score;
        public int pay;
        public int count;
        public string createdAt;
        public int start;
    }

    [Serializable]
    public class ResGameResult : BaseMsg
    {
        public int code;
        public string msg;
        public int page;
        public RoomGameResult room;
        public List<GameResult> games;

        public ResGameResult() : base(MsgID.ResGameResult)
        {
        }

        public override void FromData(byte[] data)
        {
            var jsonString = System.Text.Encoding.UTF8.GetString(data);
            var jsonData = JsonConvert.DeserializeObject<ResGameResult>(jsonString);
            this.code = jsonData.code;
            this.msg = jsonData.msg;
            this.room = jsonData.room;
            games = jsonData.games;
        }

        public override byte[] ToData()
        {
            return new byte[0];
        }
    }


}