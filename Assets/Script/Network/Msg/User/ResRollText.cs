using System;
using Newtonsoft.Json;

namespace Network.Msg
{
    public class ResRollText : BaseMsg
    {
        public int code;
        public string msg;
        public string rollText;

        public ResRollText() : base(MsgID.ResRollText)
        {
        }

        public override void FromData(byte[] data)
        {
            var jsonString = System.Text.Encoding.UTF8.GetString(data);
            var jsonData = JsonConvert.DeserializeObject<ResRollText>(jsonString);
            this.code = jsonData.code;
            this.msg = jsonData.msg;
            this.rollText = jsonData.rollText;
        }

        public override byte[] ToData()
        {
            return new byte[0];
        }
    }
}