using UnityEngine;
using System;
using System.Collections.Generic;

namespace Network.Msg
{
    [Serializable]
    public class ReqRoom : BaseMsg
    {
        public int id;


        public ReqRoom(int id) : base(MsgID.ReqRoom)
        {
            this.id = id;
        }

        public override void FromData(byte[] data)
        {
        }

        public override byte[] ToData()
        {
            var str = JsonUtility.ToJson(this);
            return System.Text.Encoding.UTF8.GetBytes(str);
        }
    }

    [Serializable]
    public class RoomInfo
    {
        public int id;
        public int score;
        public int pay;
        public int current;
        public int count;
        public int uid;
        public int players;
    }


    [Serializable]
    public class ResRoom : BaseMsg
    {
        public int code;
        public string msg;
        public RoomInfo room;

        public ResRoom() : base(MsgID.ResRoom)
        {
        }

        public override void FromData(byte[] data)
        {
            var jsonString = System.Text.Encoding.UTF8.GetString(data);
            var jsonData = JsonUtility.FromJson<ResRoom>(jsonString);
            this.code = jsonData.code;
            this.msg = jsonData.msg;
            this.room = jsonData.room;
        }

        public override byte[] ToData()
        {
            return new byte[0];
        }
    }
}