using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace Network.Msg
{
    public enum LoginType
    {
        MobilePass = 1, // 手机 密码登录
        MobileCode = 2, // 手机 验证码登录
        WeChat = 3, // 微信登录
    }

    [Serializable]
    public class ReqLogin : BaseMsg
    {
        public LoginType type;
        public string name;
        public string pass;

        public ReqLogin(LoginType type, string name, string pass) : base(MsgID.ReqLogin)
        {
            this.type = type;
            this.name = name;
            this.pass = pass;
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

    public class ResLogin : BaseMsg
    {
        public int code;
        public string msg;
        public string token;

        public ResLogin() : base(MsgID.ResLogin)
        {
        }

        public override void FromData(byte[] data)
        {
            var jsonString = System.Text.Encoding.Default.GetString(data);
            ResLogin jsonData = JsonUtility.FromJson<ResLogin>(jsonString);
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