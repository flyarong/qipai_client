using UnityEngine;
using System;
using System.Collections.Generic;

namespace Network.Msg
{
    [Serializable]
    public class ReqRoomList : BaseMsg
    {

        public ReqRoomList() : base(MsgID.ReqRoomList)
        {
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
    public class Room
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
    public class ResRoomList : BaseMsg
    {
        public int code;
        public string msg;
        public List<Room> rooms;

        public ResRoomList() : base(MsgID.ResRoomList)
        {
        }

        public override void FromData(byte[] data)
        {
            var jsonString = System.Text.Encoding.UTF8.GetString(data);
            var jsonData = JsonUtility.FromJson<ResRoomList>(jsonString);
            this.code = jsonData.code;
            this.msg = jsonData.msg;
            this.rooms = jsonData.rooms;
        }

        public override byte[] ToData()
        {
            return new byte[0];
        }
    }
}