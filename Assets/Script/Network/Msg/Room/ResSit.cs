using UnityEngine;
using System;
using System.Collections.Generic;

namespace Network.Msg
{
    [Serializable]
    public class ResSit : BaseMsg
    {
        public int code;
        public string msg;
        public int roomId;
        public int deskId;
        public List<PlayerInfo> players;

        public ResSit() : base(MsgID.ResSit)
        {
        }

        public override void FromData(byte[] data)
        {
            var jsonString = System.Text.Encoding.UTF8.GetString(data);
            var jsonData = JsonUtility.FromJson<ResSit>(jsonString);
            code = jsonData.code;
            msg = jsonData.msg;
            roomId = jsonData.roomId;
            deskId = jsonData.deskId;
            players = jsonData.players;
        }

        public override byte[] ToData()
        {
            return new byte[0];
        }
    }
}