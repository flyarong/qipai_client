using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Network.Msg
{

    public class TuiUser
    {
        public int deskId;
        public int score;
        public int uid;

        public override string ToString()
        {
            return "deskId:"+deskId+"\nscore:"+score+"\nuid:"+uid;
        }
    }

    [Serializable]
    public class BroadcastAllScore : BaseMsg
    {
        public int code;
        public string msg;
        public int roomId;
        public List<TuiUser> users;

        public override string ToString()
        {
            return "code:" + code + "\nroomId:" + roomId + "\n" + users.ToString();
        }

        public BroadcastAllScore() : base(MsgID.BroadcastAllScore)
        {
        }

        public override void FromData(byte[] data)
        {
            var jsonString = System.Text.Encoding.UTF8.GetString(data);
            var jsonData = JsonConvert.DeserializeObject<BroadcastAllScore>(jsonString);
            this.code = jsonData.code;
            this.msg = jsonData.msg;
            this.roomId = jsonData.roomId;
            this.users = jsonData.users;
        }

        public override byte[] ToData()
        {
            return new byte[0];
        }
    }
}