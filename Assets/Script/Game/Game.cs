using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;

namespace Game
{
    public class Game : MonoBehaviour
    {

        AudioSource audio;
        GComponent ui;
        GComponent tips;
        GButton btnStart;
        GButton btnScore1, btnScore2;
        private string[] scores = {
            "1/2",
            "2/4",
            "3/6",
            "4/8",
            "5/10",
            "10/20"
        };

        private void Awake()
        {
            GameObject audioObj = new GameObject("audio");
            audio = audioObj.AddComponent<AudioSource>();

            ui = GetComponent<UIPanel>().ui;
            tips = ui.GetChild("tips").asCom;

            btnStart = ui.GetChild("btnStart").asButton;
            btnScore1 = ui.GetChild("btnScore1").asButton;
            btnScore2 = ui.GetChild("btnScore2").asButton;
            btnStart.onClick.Add(onStartClick);
            btnScore1.onClick.Add(onScore);
            btnScore2.onClick.Add(onScore);

            // 绑定抢庄按钮事件
            ui.GetChild("btnTimes0").onClick.Add(onBtnTimesClick);
            ui.GetChild("btnTimes1").onClick.Add(onBtnTimesClick);
            ui.GetChild("btnTimes2").onClick.Add(onBtnTimesClick);
            ui.GetChild("btnTimes3").onClick.Add(onBtnTimesClick);
            ui.GetChild("btnTimes4").onClick.Add(onBtnTimesClick);


            showTips("等待游戏开始...");
        }

        void Start()
        {
            EventCenter.AddListener<string>(NoticeType.GameBegin, GameBegin);
            EventCenter.AddListener<string, string, string>(NoticeType.SetTimes, SetTimes);
            EventCenter.AddListener<string, string, string>(NoticeType.SetScore, SetScore);
            EventCenter.AddListener<string>(NoticeType.SetScoreAll, SetScoreAll);
            EventCenter.AddListener<string, string>(NoticeType.SetBanker, SetBanker);
            EventCenter.AddListener<string, string>(NoticeType.CardTypes, CardTypes);
        }

        private void OnDestroy()
        {
            EventCenter.RemoveListener<string>(NoticeType.GameBegin, GameBegin);
            EventCenter.RemoveListener<string, string, string>(NoticeType.SetTimes, SetTimes);
            EventCenter.RemoveListener<string, string, string>(NoticeType.SetScore, SetScore);
            EventCenter.RemoveListener<string>(NoticeType.SetScoreAll, SetScoreAll);
            EventCenter.RemoveListener<string, string>(NoticeType.SetBanker, SetBanker);
            EventCenter.RemoveListener<string, string>(NoticeType.CardTypes, CardTypes);
        }

        void GameBegin(string roomId)
        {

            var d = new Api.Room().GetCards(roomId);
            if (d["code"].n != 0)
            {
                Utils.MsgBox.ShowErr(d["msg"].str);
                return;
            }

            showTips("正在抢庄...");

            // 显示抢庄按钮
            ui.GetChild("btnTimes").visible = true;


            foreach (var p in Data.Room.Players)
            {
                // 隐藏用户头像上方的积分图标
                p.PlayerUi.GetChild("setScore").visible = false;
                // 隐藏用户头像上方的庄家标志
                p.PlayerUi.GetChild("zhuang").visible = false;


                // 隐藏所有玩家抢庄倍数
                var tc = p.PlayerUi.GetController("times");
                tc.selectedIndex = 0;
            }



            for (var i = 1; i < 11; i++)
            {
                // 跳过自己
                if (i == Data.Room.DeskId)
                {
                    EventCenter.Broadcast<string, string>(NoticeType.PutCard, Data.Room.DeskId + "", d["data"]["cards"].str);
                    continue;
                }

                EventCenter.Broadcast<string, string>(NoticeType.PutCard, i + "", "-1|-1|-1|-1");
            }
        }

        /// <summary>
        /// 显示提示条
        /// </summary>
        /// <param name="text">提示的内容</param>
        void showTips(string text)
        {
            tips.GetChild("text").text = text;
            tips.visible = true;
        }

        /// <summary>
        /// 隐藏提示条
        /// </summary>
        void hideTips()
        {
            tips.visible = false;
        }

        void CardTypes(string roomId, string paixing)
        {
            if (roomId != Data.Room.Id)
            {
                return;
            }

            

            var pxs = paixing.Split(';');
            foreach (var v in pxs)
            {
                var px = v.Split(',');
                var deskId = "";
                foreach (var p in Data.Room.Players)
                {
                    if (px[0] == p.Uid + "")
                    {
                        deskId = p.DeskId + "";
                        break;
                    }
                }
                if (deskId == "")
                {
                    Debug.Log("显示牌型失败");
                    continue;
                }

                if (px[0] == Data.User.Id + "")
                {
                    audio.clip = Resources.Load<AudioClip>("Game/audio/nv_niu" + px[1]);
                    audio.Play();
                }

                var pxStr = "";
                var pxx = px[2].Split('|');
                for (var i= 0; i< 5; i++)
                {
                    pxStr += pxx[i] + "|";
                    switch (px[1])
                    {
                        case "1":
                        case "2":
                        case "3":
                        case "4":
                        case "5":
                        case "6":
                        case "7":
                        case "8":
                        case "9":
                        case "10":
                            pxStr += i==2?"-|":"";
                            break;
                        case "12":
                            pxStr += i == 3 ? "-|" : "";
                            break;
                    }
                }
                pxStr = pxStr.Substring(0, pxStr.Length - 1);
                EventCenter.Broadcast<string, string>(NoticeType.PutCard, deskId, pxStr);
            }

        }

        void onStartClick()
        {


            var d = new Api.Room().Start(Data.Room.Id);
            if (d != null && d["code"] != null)
            {
                if (d["code"].n != 0)
                {
                    Utils.MsgBox.ShowErr(d["msg"].str);
                    return;
                }
            }

            btnStart.visible = false;
            ui.GetChild("cardImg").visible = true;
            ui.GetChild("cardImg1").visible = true;
        }

        void onScore(EventContext e)
        {

            GButton btn = e.sender as GButton;
            var d = new Api.Room().SetScore(Data.Room.Id, btn.title);
            if (d != null && d["code"] != null)
            {
                if (d["code"].n != 0)
                {
                    Utils.MsgBox.ShowErr(d["msg"].str);
                    return;
                }
            }
            ui.GetChild("btnScores").visible = false;

        }

        void onBtnTimesClick(EventContext e)
        {
            var tg = ui.GetChild("btnTimes");
            if (!tg.visible)
            {
                return;
            }
            tg.visible = false;

            GButton btn = e.sender as GButton;
            var d = new Api.Room().SetTimes(Data.Room.Id, btn.data + "");
            if (d != null && d["code"] != null)
            {
                if (d["code"].n != 0)
                {
                    Utils.MsgBox.ShowErr(d["msg"].str);
                    return;
                }
            }
        }

        void SetTimes(string roomId, string uid, string times)
        {
            foreach (var p in Data.Room.Players)
            {
                if (p.UserInfo["id"].n + "" == uid)
                {
                    var tc = p.PlayerUi.GetController("times");
                    tc.selectedIndex = int.Parse(times) + 1;
                }
            }
        }

        void SetScore(string roomId, string uid, string score)
        {
            foreach (var p in Data.Room.Players)
            {
                if (p.Uid + "" == uid)
                {
                    var scoreUi = p.PlayerUi.GetChild("setScore").asTextField;
                    scoreUi.visible = true;
                    scoreUi.text = score;
                }
                if (uid!=p.Uid+"")
                {
                    var tc = p.PlayerUi.GetController("times");
                    tc.selectedIndex = 0;
                }
            }
        }

        void SetScoreAll(string roomId)
        {
            showTips("比牌中...");
            // 隐藏所有玩家抢庄倍数
            foreach (var p in Data.Room.Players)
            {
                var tc = p.PlayerUi.GetController("times");
                if (!p.IsBanker)
                {
                    tc.selectedIndex = 0;
                }
            }
            ui.GetChild("btnScores").visible = false;
        }

        void SetBanker(string roomId, string bankerUid)
        {
            var msg = "等待闲家下注...";
            if (bankerUid != Data.User.Id + "")
            {
                msg = "等您下注...";
                ui.GetChild("btnScores").visible = true;
            }
            showTips(msg);
            ui.GetChild("btnTimes").visible = false;

            var d = new Api.Room().GetCards(roomId);
            if (d == null) return;
            if (d["code"].n != 0)
            {
                Utils.MsgBox.ShowErr(d["msg"].str);
                return;
            }

            foreach (var p in Data.Room.Players)
            {
                if (p.Uid + "" == bankerUid)
                {
                    p.IsBanker = true;
                    p.PlayerUi.GetChild("zhuang").visible = true;
                }
            }
            Debug.Log("庄家：" + bankerUid);
            EventCenter.Broadcast<string, string>(NoticeType.PutCard, Data.Room.DeskId + "", d["data"]["cards"].str);
        }
    }
}
