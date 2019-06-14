using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Network.Msg
{

    [Serializable]
    public class Club
    {
        public int id;
        public int score;
        public int pay;
        public int count;
        public int bossUid;
        public string boss;
    }
    
    [Serializable]
    public class ResClubList : BaseMsg
    {
        public int code;
        public string msg;
        public List<Club> clubs;

        public ResClubList() : base(MsgID.ResClubs)
        {
        }

        public override void FromData(byte[] data)
        {
            var jsonString = System.Text.Encoding.UTF8.GetString(data);
            var jsonData = JsonConvert.DeserializeObject<ResClubList>(jsonString);
            this.code = jsonData.code;
            this.msg = jsonData.msg;
            this.clubs = jsonData.clubs;
        }

        public override byte[] ToData()
        {
            return new byte[0];
        }
    }
}