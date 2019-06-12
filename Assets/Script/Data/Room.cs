using UnityEngine;
using System.Collections;
using Network.Msg;
using System.Collections.Generic;
using System;

namespace Data
{
    public static class Game
    {
        // 用户当前所在房间编号
        public static int Id { set { PlayerPrefs.SetInt("currentRoomId", value); } get { return PlayerPrefs.GetInt("currentRoomId"); } }
        public static int DeskId; // 当前用户所在座位号
        public static Dictionary<int, Player> Players = new Dictionary<int, Player>(); // 当前房间所有玩家
        public static RoomInfo info = new RoomInfo();
        public static TotalScore TotalScore = new TotalScore();
        public static void RemoveAllPlayers()
        {
            // 隐藏并删除所有的玩家
            foreach (var p in Data.Game.Players)
            {
                p.Value.PlayerUi.visible = true;
            }
            Players.Clear();
        }

        public static Player GetPlayer(int uid)
        {
            if (Players.ContainsKey(uid))
            {
                return Players[uid];
            }
            Debug.LogWarning("获取玩家失败");
            return null;
        }
    }

    /// <summary>
    /// 管理总分
    /// </summary>
    public class TotalScore
    {
        // 增加积分
        public void Add(int roomId, int uid, int score)
        {
            var key = "Game_TotalScore_" + roomId + "_" + uid;
            if (!PlayerPrefs.HasKey(key))
            {
                PlayerPrefs.SetInt(key, 0);
            }
            PlayerPrefs.SetInt(key, PlayerPrefs.GetInt(key) + score);

            if (!PlayerPrefs.HasKey("Game_TotalScores"))
            {
                PlayerPrefs.SetString("Game_TotalScores", "");
            }
            PlayerPrefs.SetString("Game_TotalScores", PlayerPrefs.GetString("Game_TotalScores") + roomId + "_" + uid + "|");
        }

        public int Get(int roomId, int uid)
        {
            var key = "Game_TotalScore_" + roomId + "_" + uid;
            if (!PlayerPrefs.HasKey(key))
            {
                PlayerPrefs.SetInt(key, 0);
            }
            return PlayerPrefs.GetInt(key);
        }

        public void Clear()
        {
            foreach (var p in PlayerPrefs.GetString("Game_TotalScores").Split('|'))
            {
                var key = "Game_TotalScore_" + p;
                if (!PlayerPrefs.HasKey(key))
                {
                    continue;
                }
                PlayerPrefs.DeleteKey(key);
            }
        }
    }
}