using Newtonsoft.Json;
using System;

namespace Network.Msg
{
    [Serializable]
    public class BroadcastDefaultVoice : BaseMsg
    {
        public int code;
        public string msg;
        public int roomId;
        public int voiceId;
        public int deskId;
        public int sex;

        public BroadcastDefaultVoice() : base(MsgID.BroadcastDefaultVoice)
        {
        }

        public override void FromData(byte[] data)
        {
            var jsonString = System.Text.Encoding.UTF8.GetString(data);
            var jsonData = JsonConvert.DeserializeObject<BroadcastDefaultVoice>(jsonString);
            code = jsonData.code;
            msg = jsonData.msg;
            roomId = jsonData.roomId;
            voiceId = jsonData.voiceId;
            deskId = jsonData.deskId;
            sex = jsonData.sex;
        }

        public override byte[] ToData()
        {
            return new byte[0];
        }
    }
}