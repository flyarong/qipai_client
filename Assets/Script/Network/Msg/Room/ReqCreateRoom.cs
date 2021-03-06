using UnityEngine;
using System;
using Newtonsoft.Json;

namespace Network.Msg
{
    [Serializable]
    public class ReqCreateRoom : BaseMsg
    {
        public int players;
        public int score;
        public int pay;
        public int count;
        public int start;
        public int times;
        
        public ReqCreateRoom(int players, int score, int start, int count, int pay, int times) : base(MsgID.ReqCreateRoom)
        {
            this.players = players;
            this.score = score;
            this.pay = pay;
            this.count = count;
            this.start = start;
            this.times = times;
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

    public class ResCreateRoom : BaseMsg
    {
        public int code;
        public string msg;
        public int roomId;

        public ResCreateRoom() : base(MsgID.ResCreateRoom)
        {
        }

        public override void FromData(byte[] data)
        {
            var jsonString = System.Text.Encoding.Default.GetString(data);
            var jsonData = JsonConvert.DeserializeObject<ResCreateRoom>(jsonString);
            this.code = jsonData.code;
            this.msg = jsonData.msg;
            this.roomId = jsonData.roomId;
        }

        public override byte[] ToData()
        {
            return new byte[0];
        }
    }

}