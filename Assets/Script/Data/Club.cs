using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Network.Msg;

namespace Data
{
    public static class Club
    {
        
        private static ClubInfo info;
        static bool isAdmin; // 当前登录用户是否是管理员
        static bool isBoss; // 当前登录用户是否是老板
        static List<ClubUser> users; // 当前房间的所有玩家

        public static ClubInfo Info { get => info; set => info = value; }
        public static bool IsAdmin { get => isAdmin; set => isAdmin = value; }
        public static bool IsBoss { get => isBoss; set => isBoss = value; }
        public static int Id { set { PlayerPrefs.SetInt("currentClubId", value); } get { return PlayerPrefs.GetInt("currentClubId"); } }
        public static int TableId { set { PlayerPrefs.SetInt("currentClubTableId", value); } get { return PlayerPrefs.GetInt("currentClubTableId"); } }

        public static List<ClubUser> Users { get => users; set => users = value; }
    }
}