using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Api;

namespace Data
{
    public static class Club
    {
        private static string id;
        private static JSONObject data;
        static bool isAdmin; // 当前登录用户是否是管理员
        static bool isBoss; // 当前登录用户是否是老板

        public static JSONObject Data { get => data; set => data = value; }
        public static bool IsAdmin { get => isAdmin; set => isAdmin = value; }
        public static bool IsBoss { get => isBoss; set => isBoss = value; }
        public static string Id
        {
            get
            {
                return PlayerPrefs.GetString("ClubId");
            }
            set
            {

            }
        }
    }

}