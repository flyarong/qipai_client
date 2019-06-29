using System;
using Newtonsoft.Json;

namespace Network.Msg
{
    public class ResNotice : BaseMsg
    {
        public int code;
        public string msg;
        public string notice;

        public ResNotice() : base(MsgID.ResNotice)
        {
        }

        public override void FromData(byte[] data)
        {
            var jsonString = System.Text.Encoding.UTF8.GetString(data);
            var jsonData = JsonConvert.DeserializeObject<ResNotice>(jsonString);
            this.code = jsonData.code;
            this.msg = jsonData.msg;
            this.notice = jsonData.notice;
        }

        public override byte[] ToData()
        {
            return new byte[0];
        }
    }
}