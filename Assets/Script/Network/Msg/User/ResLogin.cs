using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Network.Msg
{
    public enum LoginType
    {
        MobilePass = 1, // 手机 密码登录
        MobileCode = 2, // 手机 验证码登录
        WeChat = 3, // 微信登录
    }

    [Serializable]
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
            var jsonString = System.Text.Encoding.UTF8.GetString(data);
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