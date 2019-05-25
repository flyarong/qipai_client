using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Api;
using Game;

namespace Data
{
    public static class Room
    {
        private static JSONObject info;
        private static List<Game.Player> players = new List<Game.Player>();
        private static int deskId; // 当前用户在第几个座位
        public static JSONObject Info { get => info; set => info = value; }

        // 当前用户所在房间号
        public static string Id
        {
            get
            {
                return PlayerPrefs.GetString("RoomId");
            }
            set
            {
                PlayerPrefs.SetString("RoomId", value);
            }
        }

        public static List<Player> Players { get => players; set => players = value; }
        public static int DeskId { get => deskId; set => deskId = value; }
        
    }

}