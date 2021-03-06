using UnityEngine;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Network.Msg
{
    [Serializable]
    public class ResSit : BaseMsg
    {
        public int code;
        public string msg;
        public int roomId;
        public int deskId;
        public int uid;
        public List<PlayerInfo> players;

        public ResSit() : base(MsgID.ResSit)
        {
        }

        public override void FromData(byte[] data)
        {
            var jsonString = System.Text.Encoding.UTF8.GetString(data);
            var jsonData = JsonConvert.DeserializeObject<ResSit>(jsonString);
            code = jsonData.code;
            msg = jsonData.msg;
            roomId = jsonData.roomId;
            deskId = jsonData.deskId;
            players = jsonData.players;
            uid = jsonData.uid;
        }

        public override byte[] ToData()
        {
            return new byte[0];
        }
    }
}