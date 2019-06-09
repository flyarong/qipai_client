﻿using UnityEngine;
using System;

namespace Network.Msg
{
    [Serializable]
    public class BroadcastBanker : BaseMsg
    {
        public int code;
        public string msg;
        public GameInfo game;

        public BroadcastBanker() : base(MsgID.BroadcastBanker)
        {
        }

        public override void FromData(byte[] data)
        {
            var jsonString = System.Text.Encoding.UTF8.GetString(data);
            var jsonData = JsonUtility.FromJson<BroadcastBanker>(jsonString);
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