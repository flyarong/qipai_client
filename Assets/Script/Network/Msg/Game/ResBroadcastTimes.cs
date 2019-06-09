using UnityEngine;
using System;

namespace Network.Msg
{
    [Serializable]
    public class BroadcastTimes : BaseMsg
    {
        public int code;
        public string msg;
        public GameInfo game;

        public BroadcastTimes() : base(MsgID.BroadcastTimes)
        {
        }

        public override void FromData(byte[] data)
        {
            var jsonString = System.Text.Encoding.UTF8.GetString(data);
            var jsonData = JsonUtility.FromJson<BroadcastTimes>(jsonString);
            this.code = jsonData.code;
            this.msg = jsonData.msg;
            this.game = jsonData.game;
        }

        public override byte[] ToData()
        {
            return new byte[0];
        }
    }
}