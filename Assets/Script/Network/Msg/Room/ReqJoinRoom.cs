using UnityEngine;
using System;

namespace Network.Msg
{
    [Serializable]
    public class ReqJoinRoom : BaseMsg
    {
        public int id;


        public ReqJoinRoom(int id) : base(MsgID.ReqJoinRoom)
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

    public class ResJoinRoom : ErrMsg
    {
        public ResJoinRoom() : base(MsgID.ResJoinRoom)
        {
        }
    }
}