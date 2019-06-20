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
            var req = new Utils.Msg(MsgID.ReqCreateRoom);
            req.Add("players", players);
            req.Add("score", score);
            req.Add("start", start);
            req.Add("count", count);
            req.Add("pay", pay);
            req.Add("times", times);
            req.Send();
        }

        public static void RoomList()
        {
            new Utils.Msg(MsgID.ReqRoomList).Send();
        }

        public static void GetRoom(int roomId)
        {
            if (roomId <= 0)
            {
                Debug.LogWarning("不合法的roomid: " + roomId);
                SceneManager.LoadScene("Menu");
                return;
            }

            new Utils.Msg(MsgID.ReqRoom).Add("id",roomId).Send();
        }

        public static void JoinRoom(int roomId)
        {
            if (roomId <= 0)
            {
                Debug.LogWarning("不合法的roomid: " + roomId);
                SceneManager.LoadScene("Menu");
                return;
            }
            new Utils.Msg(MsgID.ReqJoinRoom).Add("roomId", roomId).Send();
        }

        public static void Sit(int roomId)
        {
            if (roomId <= 0)
            {
                Debug.LogWarning("不合法的roomid: " + roomId);
                SceneManager.LoadScene("Menu");
                return;
            }
            new Utils.Msg(MsgID.ReqSit).Add("id", roomId).Send();
        }


        public static void Leave(int roomId)
        {
            if (roomId <= 0)
            {
                Debug.LogWarning("不合法的roomid: " + roomId);
                SceneManager.LoadScene("Menu");
                return;
            }
            new Utils.Msg(MsgID.ReqLeaveRoom).Add("roomId", roomId).Send();
        }

        public static void Delete(int roomId)
        {
            if (roomId <= 0)
            {
                Debug.LogWarning("不合法的roomid: " + roomId);
                SceneManager.LoadScene("Menu");
                return;
            }
            new Utils.Msg(MsgID.ReqDeleteRoom).Add("id", roomId).Send();
        }
    }
}