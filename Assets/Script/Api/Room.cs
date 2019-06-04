using Network.Msg;
using Network;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Api
{
    public static class Room
    {
        private static Manager Inst = Manager.Inst;
        public static void Create(int players, int score, int start, int count, int pay, int times)
        {
            var msg = new ReqCreateRoom(players, score, start, count, pay, times);
            Inst.SendMessage(msg.ToMessage());
        }

        public static void RoomList()
        {
            var msg = new ReqRoomList();
            Inst.SendMessage(msg.ToMessage());
        }

        public static void GetRoom(int roomId)
        {
            if (roomId <= 0)
            {
                Debug.LogWarning("不合法的roomid: " + roomId);
                SceneManager.LoadScene("Menu");
                return;
            }
            var msg = new ReqRoom(roomId);
            Inst.SendMessage(msg.ToMessage());
        }

        public static void JoinRoom(int roomId)
        {
            if (roomId <= 0)
            {
                Debug.LogWarning("不合法的roomid: " + roomId);
                SceneManager.LoadScene("Menu");
                return;
            }
            var msg = new ReqJoinRoom(roomId);
            Inst.SendMessage(msg.ToMessage());
        }
    }
}