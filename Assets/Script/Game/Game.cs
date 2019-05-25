using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;

namespace Game
{
    public class Game : MonoBehaviour
    {
        GComponent ui;
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
            ui = GetComponent<UIPanel>().ui;
            btnStart = ui.GetChild("btnStart").asButton;
            btnScore1 = ui.GetChild("btnScore1").asButton;
            btnScore2 = ui.GetChild("btnScore2").asButton;
            btnStart.onClick.Add(onStartClick);
            btnScore1.onClick.Add(onScore);
            btnScore2.onClick.Add(onScore);
        }

        void Start()
        {
            EventCenter.AddListener<string>(NoticeType.GameBegin, GameStart);
            EventCenter.AddListener<string, string, string>(NoticeType.SetTimes, SetScore);
            EventCenter.AddListener<string>(NoticeType.SetTimesAll, SetScoreAll);
        }

        private void OnDestroy()
        {
            EventCenter.RemoveListener<string>(NoticeType.GameBegin, GameStart);
            EventCenter.RemoveListener<string, string, string>(NoticeType.SetTimes, SetScore);
            EventCenter.RemoveListener<string>(NoticeType.SetTimesAll, SetScoreAll);
        }

        void GameStart(string roomId)
        {
            var d = new Api.Room().GetCards(roomId);
            if (d["code"].n != 0)
            {
                Utils.MsgBox.ShowErr(d["str"].str);
                return;
            }
            btnScore1.visible = true;
            btnScore2.visible = true;


            foreach (var p in Data.Room.Players)
            {
                var scoreUi = p.PlayerUi.GetChild("setScore").asTextField;
                scoreUi.visible = false;

            }


            EventCenter.Broadcast<string, string>(NoticeType.PutCard, Data.Room.DeskId + "", d["data"]["cards"].str);
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
            ui.GetChild("waitTips").visible = false;
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

        }

        void SetScore(string roomId, string uid, string score)
        {
            // 隐藏自己的下注按钮
            if (uid == Data.User.Id + "")
            {
                btnScore1.visible = false;
                btnScore2.visible = false;
            }

            foreach (var p in Data.Room.Players)
            {
                if (p.UserInfo["id"].n + "" == uid)
                {
                    var scoreUi = p.PlayerUi.GetChild("setScore").asTextField;
                    scoreUi.visible = true;
                    scoreUi.text = score;
                }
            }
        }

        void SetScoreAll(string roomId)
        {
            var d = new Api.Room().GetCards(roomId);
            if (d["code"].n != 0)
            {
                Utils.MsgBox.ShowErr(d["str"].str);
                return;
            }
            EventCenter.Broadcast<string, string>(NoticeType.PutCard, Data.Room.DeskId + "", d["data"]["cards"].str);
        }
    }
}
