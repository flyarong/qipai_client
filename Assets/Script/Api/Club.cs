using Network.Msg;
using Network;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Api
{

    public static class Club
    {
        public static void Create(int players, int score, int start, int count, int pay, int times)
        {
            var req = new Utils.Msg(MsgID.ReqCreateClub);
            req.Add("players", players);
            req.Add("score", score);
            req.Add("start", start);
            req.Add("count", count);
            req.Add("pay", pay);
            req.Add("times", times);
            req.Send();
        }

        public static void Clubs()
        {
            new Utils.Msg(MsgID.ReqClubs).Send();
        }

        public static void ClubUsers(int clubId)
        {
            new Utils.Msg(MsgID.ReqClubUsers).Add("clubId", clubId).Send();
        }

        public static void GetClub(int clubId)
        {
            if (clubId <= 0)
            {
                Debug.LogWarning("不合法的茶楼编号: " + clubId);
                SceneManager.LoadScene("Menu");
                return;
            }
            new Utils.Msg(MsgID.ReqClub).Add("clubId", clubId).Send();
        }

        public static void Join(int clubId)
        {
            if (clubId <= 0)
            {
                Debug.LogWarning("不合法的茶楼编号: " + clubId);
                SceneManager.LoadScene("Menu");
                return;
            }
            new Utils.Msg(MsgID.ReqJoinClub).Add("clubId", clubId).Send();
        }
        
        public static void Exit(int clubId)
        {
            if (clubId <= 0)
            {
                Debug.LogWarning("不合法的茶楼编号: " + clubId);
                SceneManager.LoadScene("Menu");
                return;
            }
            new Utils.Msg(MsgID.ReqExitClub).Add("clubId", clubId).Send();
        }

        internal static void EditClubInfo(int clubId, bool check, bool close, string name, string rollText, string notice)
        {
            if (clubId <= 0)
            {
                Debug.LogWarning("不合法的茶楼编号: " + clubId);
                SceneManager.LoadScene("Menu");
                return;
            }
            new Utils.Msg(MsgID.ReqEditClub).Add("clubId", clubId)
                .Add("check", check)
                .Add("close", close)
                .Add("name", name)
                .Add("rollText", rollText)
                .Add("notice", notice)
                .Send();
        }

        public static void Delete(int clubId)
        {
            if (clubId <= 0)
            {
                Debug.LogWarning("不合法的茶楼编号: " + clubId);
                SceneManager.LoadScene("Menu");
                return;
            }
            new Utils.Msg(MsgID.ReqDelClub).Add("clubId", clubId).Send();
        }
    }
}