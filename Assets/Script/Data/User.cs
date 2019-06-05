using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Network.Msg;

namespace Data
{
    public static class User
    {
        // 用户位置
        private static int pos;
        private static int id;
        private static Network.Msg.UserInfo info = null;
        private static string token;
        public static int Pos { get => pos; set => pos = value; }


        public static int Id { get => id; set => id = value; }
        public static string Token
        {
            get
            {
                if (token != "") return token;
                token = PlayerPrefs.GetString("token");
                return token;
            }
            set
            {
                token = value;
                PlayerPrefs.SetString("token", token);
            }
        }

        public static Network.Msg.UserInfo Info { get => info; set => info = value; }
    }

}