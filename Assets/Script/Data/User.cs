using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Api;

namespace Data
{
    public static class User
    {
        // 用户位置
        private static int pos;
        private static int id;
        private static string token="";
        // 用户信息
        private static JSONObject info = null;
        

        public static int Pos { get => pos; set => pos = value; }
        public static JSONObject Info { get => info; set { info = value; Id = (int)info["id"].n; } }
        

        public static int Id { get => id; set => id = value; }
        public static string Token { get => token; set { token = value; PlayerPrefs.SetString("token", value); } }
    }

}