using UnityEngine;
using System.Collections;
using Network.Msg;
using System.Collections.Generic;
using System;

namespace Data
{
   

    public static class Room
    {
        // 用户当前所在房间编号
        public static int Id { set { PlayerPrefs.SetInt("currentRoomId", value); } get { return PlayerPrefs.GetInt("currentRoomId"); } }
        public static int DeskId; // 当前用户所在座位号
        public static Dictionary<int,Player> Players = new Dictionary<int, Player>(); // 当前房间所有玩家
        public static RoomInfo info = new RoomInfo();
        public static void RemoveAllPlayers()
        {
            // 隐藏并删除所有的玩家
            foreach (var p in Data.Room.Players)
            {
                p.Value.PlayerUi.visible = true;
            }
            Players.Clear();
        }

        public static Player GetPlayer(int uid) {
            if (Players.ContainsKey(uid))
            {
                return Players[uid];
            }
            Debug.LogWarning("获取玩家失败");
            return null;
        }
    }
}