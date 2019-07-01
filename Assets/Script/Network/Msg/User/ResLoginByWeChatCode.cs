using System;
using Newtonsoft.Json;

namespace Network.Msg
{
    [Serializable]
    public class ResLoginByWeChatCode : CommonMsg
    {
        public string token;
        public string accessToken;

        public ResLoginByWeChatCode() : base(MsgID.ResLoginByWeChatCode)
        {
        }

        public override void FromData(byte[] data)
        {
            var jsonString = System.Text.Encoding.UTF8.GetString(data);
            var jsonData = JsonConvert.DeserializeObject<ResLoginByWeChatCode>(jsonString);
            this.code = jsonData.code;
            this.msg = jsonData.msg;
            this.token = jsonData.token;
            accessToken = jsonData.accessToken;
        }

        public override byte[] ToData()
        {
            return new byte[0];
        }
    }
}