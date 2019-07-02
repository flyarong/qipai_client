using System;
using Newtonsoft.Json;

namespace Network.Msg
{
    public class ResShareText : BaseMsg
    {
        public int code;
        public string msg;
        public string shareText;

        public ResShareText() : base(MsgID.ResShareText)
        {
        }

        public override void FromData(byte[] data)
        {
            var jsonString = System.Text.Encoding.UTF8.GetString(data);
            var jsonData = JsonConvert.DeserializeObject<ResShareText>(jsonString);
            this.code = jsonData.code;
            this.msg = jsonData.msg;
            this.shareText = jsonData.shareText;
        }

        public override byte[] ToData()
        {
            return new byte[0];
        }
    }
}