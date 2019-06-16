using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Network.Msg
{

    [Serializable]
    public class ClubUser
    {
        public int id;
        public string nick;
        public string avatar;
        public int clubId;
        public int status;
        public bool admin;
    }

    [Serializable]
    public class ResClubUsers : BaseMsg
    {
        public int code;
        public string msg;
        public List<ClubUser> users;

        public ResClubUsers() : base(MsgID.ResClubUsers)
        {
        }

        public override void FromData(byte[] data)
        {
            var jsonString = System.Text.Encoding.UTF8.GetString(data);
            var jsonData = JsonConvert.DeserializeObject<ResClubUsers>(jsonString);
            this.code = jsonData.code;
            this.msg = jsonData.msg;
            this.users = jsonData.users;
        }

        public override byte[] ToData()
        {
            return new byte[0];
        }
    }
}