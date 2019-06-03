using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace Data
{
    public static class Room
    {
        // 用户当前所在房间编号
        public static int Id { set {PlayerPrefs.SetInt("roomId", value); } get { return PlayerPrefs.GetInt("roomId"); } }
    }
}