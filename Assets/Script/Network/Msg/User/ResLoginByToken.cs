using System;
using Newtonsoft.Json;

namespace Network.Msg
{
    [Serializable]
    public class ResLoginByToken : CommonMsg
    {
        public string token;

        public ResLoginByToken() : base(MsgID.ResLoginByToken)
        {
        }

        public override void FromData(byte[] data)
        {
            var jsonString = System.Text.Encoding.UTF8.GetString(data);
            var jsonData = JsonConvert.DeserializeObject<ResLoginByToken>(jsonString);
            this.code = jsonData.code;
            this.msg = jsonData.msg;
            this.token = jsonData.token;
        }

        public override byte[] ToData()
        {
            return new byte[0];
        }
    }
}