using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Network.Msg;

namespace Data
{
    public static class Room
    {
        // 用户当前所在房间编号
        public static int Id { set {PlayerPrefs.SetInt("currentRoomId", value); } get { return PlayerPrefs.GetInt("currentRoomId"); } }
        public static RoomInfo info;
    }
}