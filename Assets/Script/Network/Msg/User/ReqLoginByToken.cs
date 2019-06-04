using UnityEngine;
using System;

namespace Network.Msg
{
    [Serializable]
    public class ReqLoginByToken : BaseMsg
    {
        public string token;


        public ReqLoginByToken(string token) : base(MsgID.ReqLoginByToken)
        {
            this.token = token;
        }

        public override void FromData(byte[] data)
        {
        }

        public override byte[] ToData()
        {
            var str = JsonUtility.ToJson(this);
            return System.Text.Encoding.UTF8.GetBytes(str);
        }
    }


    public class ResLoginByToken : ErrMsg
    {
        public string token;

        public ResLoginByToken() : base(MsgID.ResLoginByToken)
        {
        }

        public override void FromData(byte[] data)
        {
            var jsonString = System.Text.Encoding.UTF8.GetString(data);
            var jsonData = JsonUtility.FromJson<ResLoginByToken>(jsonString);
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