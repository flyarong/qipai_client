using UnityEngine;
using System;

namespace Network.Msg
{
    public enum CodeType
    {
        Reg = 1,
        Login = 2,
        Reset = 3,
    }

    [Serializable]
    public class ReqCode : BaseMsg
    {
        public CodeType type;
        public string phone;


        public ReqCode(string phone,CodeType type=CodeType.Reg) : base(MsgID.ReqCode)
        {
            this.phone = phone;
            this.type = type;
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

    public class ResCode : ErrMsg
    {
        public ResCode() : base(MsgID.ResCode)
        {
        }
    }

}