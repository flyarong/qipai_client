using UnityEngine;
using System.Collections;
using Utils;
using Network.Msg;
using Notification;
using System;
using System.Collections.Generic;
using FairyGUI;

namespace Game
{

    public class Game : MonoBehaviour
    {

        GComponent ui;
        List<GComponent> cardPlaces = new List<GComponent>();
        Dictionary<string, List<GImage>> cardsUi = new Dictionary<string, List<GImage>>();
        Transition tPut;
        GButton btnStart;
        GComponent tips;

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
            tPut = ui.GetTransition("t1");
            tips = ui.GetChild("tips").asCom;

            ui.GetChild("cardImg").sortingOrder = 1000;
            for (var i = 1; i < 11; i++)
            {
                cardPlaces.Add(ui.GetChild("card" + i).asCom);
            }
            btnStart = ui.GetChild("btnStart").asButton;
            btnScore1 = ui.GetChild("btnScore1").asButton;
            btnScore2 = ui.GetChild("btnScore2").asButton;

            btnStart.onClick.Add(onBtnStartClick);
        }
        // Use this for initialization
        void Start()
        {
            Handler.Add<ResGameStart>(MsgID.ResGameStart, NotificationType.Network_OnResGameStart);
            Handler.Add<BroadcastTimes>(MsgID.BroadcastTimes, NotificationType.Network_OnBroadcastTimes);


            Handler.AddListenner(NotificationType.Network_OnResGameStart, OnResGameStart);
            Handler.AddListenner(NotificationType.Network_OnBroadcastTimes, OnBroadcastTimes);
        }

        private void OnBroadcastTimes(NotificationArg arg)
        {
            var data = arg.GetValue<BroadcastTimes>();
            if (data.code != 0)
            {
                MsgBox.ShowErr(data.msg, 2);
                return;
            }
            Debug.Log(data.game.playerId + "抢庄倍数：" + data.game.times);
        }

        private void onBtnStartClick(EventContext context)
        {
            Api.Game.Start(Data.Room.Id);
        }
        private void OnResGameStart(NotificationArg arg)
        {

            var data = arg.GetValue<ResGameStart>();
            if (data.code != 0)
            {
                MsgBox.ShowErr(data.msg, 2);
                return;
            }
            Debug.Log(data.game.cards);
            foreach (var p in Data.Room.Players)
            {
                if (p.DeskId == data.game.deskId)
                {
                    PutCard(p.DeskId + "", data.game.cards);
                }
                else
                {
                    PutCard(p.DeskId + "", "-1|-1|-1|-1");
                }
            }

            btnStart.visible = false;

            // 下注按钮积分初始化
            var s = scores[Data.Room.info.score];
            var ss = s.Split('/');
            btnScore1.title = ss[0];
            btnScore2.title = ss[1];
            
            ui.GetChild("btnScores").visible = true;
        }

        void PutCard(string deskId, string cards)
        {

            Data.Player player = null;
            foreach (var p in Data.Room.Players)
            {
                if (p.DeskId == int.Parse(deskId))
                {
                    player = p;
                }
            }
            if (player == null)
            {
                return;
            }

            List<GImage> cardImgs = new List<GImage>();
            var cp = cardPlaces[player.Index];
            var cpos = cp.position;
            foreach (var v in cards.Split('|'))
            {
                if (v == "-")
                {
                    cpos.x += player.DeskId == Data.Room.DeskId ? 40 : 15;
                    continue;
                }

                var n = int.Parse(v);
                string cardName = getName(n);
                GImage card = UIPackage.CreateObject("qipai", cardName).asImage;
                card.position = cpos;

                if (player.DeskId == Data.Room.DeskId)
                {
                    cpos.x += 50;
                    // card.SetScale(0.6f, 0.6f);
                }
                else
                {
                    cpos.x += 23;
                    card.SetScale(0.6f, 0.6f);
                }

                tPut.SetValue("end", cpos.x, cpos.y);
                tPut.Play();

                card.AddRelation(cp, RelationType.Middle_Middle);
                card.AddRelation(cp, RelationType.Center_Center);
                ui.AddChild(card);
                cardImgs.Add(card);

            }

            if (cardsUi.ContainsKey(deskId))
            {
                foreach (var img in cardsUi[deskId])
                {
                    ui.RemoveChild(img);
                }
                cardsUi.Remove(deskId);
            }

            cardsUi.Add(deskId, cardImgs);
        }

        string getName(int n)
        {
            if (n == -1)
            {
                return "Card_0";
            }

            var type = n % 4 + 1;
            var value = n % 13 + 1;
            // 大小王特殊处理
            if (n > 51)
            {
                type = n == 52 ? 5 : 6;
                value = 0;
            }
            string cardName = "Card_" + type + "_" + value;
            if (type == 5)
            {
                cardName = "Card_joker_1";
            }
            else if (type == 6)
            {
                cardName = "Card_joker_2";
            }

            return cardName;
        }

        void ShowTips(string text)
        {
            tips.visible = true;
            tips.GetChild("text").text = text;
        }

        void HideTips()
        {
            tips.visible = false;
        }
    }

}