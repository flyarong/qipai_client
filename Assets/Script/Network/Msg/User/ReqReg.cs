using System;
using Newtonsoft.Json;

namespace Network.Msg
{
    [Serializable]
    public class ReqReg : BaseMsg
    {
        public LoginType type;
        public string nick;
        public string name;
        public string pass;
        public string code;


        public ReqReg(LoginType type, string nick, string name, string pass, string code) : base(MsgID.ReqReg)
        {
            this.type = type;
            this.name = name;
            this.pass = pass;
            this.nick = nick;
            this.code = code;
        }

        public override void FromData(byte[] data)
        {

        }

        public override byte[] ToData()
        {
            var str = JsonConvert.SerializeObject(this);
            return System.Text.Encoding.UTF8.GetBytes(str);
        }
    }




    [Serializable]
    public class ResReg : CommonMsg
    {
        public ResReg() : base(MsgID.ResReg)
        {
        }
    }

}