using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Api
{
    public class User
    {
        static JSONObject GetSmsCode(string action, string phone)
        {
            string url = "/code?action=" + action + "&phone=" + phone;
            return new JSONObject(Client.Inst.GetContent(url));
        }
        public static JSONObject GetRegSmsCode(string phone)
        {
            return GetSmsCode("reg", phone);
        }

        public static JSONObject GetResetSmsCode(string phone)
        {
            return GetSmsCode("reset", phone);
        }

        public JSONObject Login(string type, string name, string password)
        {
            List<KeyValuePair<string, string>> paramList = new List<KeyValuePair<string, string>>();
            paramList.Add(new KeyValuePair<string, string>("type", type));
            paramList.Add(new KeyValuePair<string, string>("name", name));
            paramList.Add(new KeyValuePair<string, string>("pass", password));

            string result = Client.Inst.PostContent("/users/login", paramList);

            Debug.Log(result);

            JSONObject j = new JSONObject(result);


            if (j["code"].n != 0)
            {
                return j;
            }

            // 保存token
            Data.User.Token = j["data"]["token"].str;

            // 每次登录都强制获取,否则上次登录信息会残留
            GetUserInfo(true);
            return j;
        }

        // 获取当前登录用户信息
        public JSONObject GetUserInfo(bool force = false)
        {
            // 不是强制获取就不获取
            if (!force && Data.User.Info != null)
            {
                return null;
            }
            // 加载个人信息

            string text = Client.Inst.GetContent("/users");
            Debug.Log(text);
            JSONObject j = new JSONObject(text);
            // uid为空表示是获取自身信息，那么把信息保存起来
            Data.User.Info = j["data"]["user"];

            return j;
        }

        // 获取用户信息
        public JSONObject GetUserInfo(string uid)
        {
            string text = Client.Inst.GetContent("/users/" + uid);
            Debug.Log(text);
            JSONObject j = new JSONObject(text);
            return j;
        }
    }
}