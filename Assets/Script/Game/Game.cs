using UnityEngine;
using System.Collections;
using Utils;
using Network.Msg;
using Notification;
using System;
using System.Collections.Generic;
using FairyGUI;
using UnityEngine.SceneManagement;

namespace Game
{

    public class Game : MonoBehaviour
    {

        GComponent ui;
        AudioSource gameAudio;
        List<GComponent> cardPlaces = new List<GComponent>();
        Dictionary<int, List<GImage>> cardsUi = new Dictionary<int, List<GImage>>();
        GComponent cardCenter;  // 桌子中心牌堆的位置
        GImage centerCard;// 桌子中心的牌，用于模拟发牌
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
            GameObject audioObj = new GameObject("gameAudio");
            gameAudio = audioObj.AddComponent<AudioSource>();

            ui = GetComponent<UIPanel>().ui;
            tips = ui.GetChild("tips").asCom;
            tips.sortingOrder = 1000;
            cardCenter = ui.GetChild("cardCenter").asCom;

            for (var i = 1; i < 11; i++)
            {
                cardPlaces.Add(ui.GetChild("card" + i).asCom);
            }
            btnStart = ui.GetChild("btnStart").asButton;
            btnScore1 = ui.GetChild("btnScore1").asButton;
            btnScore2 = ui.GetChild("btnScore2").asButton;
            btnScore1.onClick.Add(onScore);
            btnScore2.onClick.Add(onScore);

            btnStart.onClick.Add(onBtnStartClick);

            // 绑定抢庄按钮事件
            ui.GetChild("btnTimes0").onClick.Add(onBtnTimesClick);
            ui.GetChild("btnTimes1").onClick.Add(onBtnTimesClick);
            ui.GetChild("btnTimes2").onClick.Add(onBtnTimesClick);
            ui.GetChild("btnTimes3").onClick.Add(onBtnTimesClick);
            ui.GetChild("btnTimes4").onClick.Add(onBtnTimesClick);
        }
        // Use this for initialization
        void Start()
        {
            Handler.Add<ResGameStart>(MsgID.ResGameStart, NotificationType.Network_OnResGameStart);
            Handler.Add<BroadcastTimes>(MsgID.BroadcastTimes, NotificationType.Network_OnBroadcastTimes);
            Handler.Add<BroadcastBanker>(MsgID.BroadcastBanker, NotificationType.Network_OnBroadcastBanker);
            Handler.Add<BroadcastScore>(MsgID.BroadcastScore, NotificationType.Network_OnBroadcastScore);
            Handler.Add<BroadcastShowCard>(MsgID.BroadcastShowCard, NotificationType.Network_OnBroadcastShowCard);
            Handler.Add<BroadcastCompareCard>(MsgID.BroadcastCompareCard, NotificationType.Network_OnBroadcastCompareCard);
            Handler.Add<BroadcastGameOver>(MsgID.BroadcastGameOver, NotificationType.Network_OnBroadcastGameOver);


            Handler.AddListenner(NotificationType.Network_OnResGameStart, OnResGameStart);
            Handler.AddListenner(NotificationType.Network_OnBroadcastTimes, OnBroadcastTimes);
            Handler.AddListenner(NotificationType.Network_OnBroadcastBanker, OnBroadcastBanker);
            Handler.AddListenner(NotificationType.Network_OnBroadcastScore, OnBroadcastScore);
            Handler.AddListenner(NotificationType.Network_OnBroadcastShowCard, OnBroadcastShowCard);
            Handler.AddListenner(NotificationType.Network_OnBroadcastCompareCard, OnBroadcastCompareCard);
            Handler.AddListenner(NotificationType.Network_OnBroadcastGameOver, OnBroadcastGameOver);

        }

        private void OnBroadcastGameOver(NotificationArg arg)
        {
            var data = arg.GetValue<BroadcastGameOver>();
            if (data.code != 0)
            {
                MsgBox.ShowErr(data.msg, 2);
                return;
            }
            if (data.roomId != Data.Game.Id)
            {
                Debug.Log("收到不属于该房间的消息，来自房间号：" + data.roomId);
                return;
            }
            Data.Game.TotalScore.Clear();
            Data.Game.Id = 0;
            SceneManager.LoadScene("Menu");
        }

        private void OnBroadcastCompareCard(NotificationArg arg)
        {
            var data = arg.GetValue<BroadcastCompareCard>();
            if (data.code != 0)
            {
                MsgBox.ShowErr(data.msg, 2);
                return;
            }
            var games = data.games;
            foreach (var game in games)
            {
                var me = game.playerId == Data.User.Id;
                PutCard(game.deskId, game.cards, game.cardType, me);
                showNiu(game.playerId, game.cardType);
                Debug.Log(game.playerId + ":" + game.totalScore);

                Data.Game.TotalScore.Add(Data.Game.Id, game.playerId, game.totalScore);

                var p = Data.Game.GetPlayer(game.playerId);
                p.PlayerUi.GetChild("score").text = Data.Game.TotalScore.Get(Data.Game.Id, game.playerId) + "";
            }

        }


        List<GComponent> nius = new List<GComponent>();
        // 显示指定玩家的牌型图标
        void showNiu(int uid, int type)
        {
            var p = Data.Game.GetPlayer(uid);
            if (p == null)
            {
                return;
            }

            GComponent niu = UIPackage.CreateObject("qipai", "niu").asCom;
            var cp = cardPlaces[p.Index];
            var cpos = cp.position;
            niu.GetController("niu").selectedIndex = type;
            niu.visible = true;
            if (uid == Data.User.Id)
            {
                cpos.y += 65;
                cpos.x += 65;
                niu.SetScale(1.4f, 1.4f);
            }
            else
            {
                cpos.y += 45;
                cpos.x += 30;
                niu.SetScale(0.8f, 0.8f);
            }

            niu.position = cpos;
            niu.AddRelation(cp, RelationType.Middle_Middle, true);
            niu.AddRelation(cp, RelationType.Center_Center, true);
            ui.AddChild(niu);
            nius.Add(niu);
        }

        // 隐藏所有玩家的牌型图标
        void hideNius()
        {
            foreach (var niu in nius)
            {
                ui.RemoveChild(niu);
            }
        }

        private void OnBroadcastShowCard(NotificationArg arg)
        {
            var data = arg.GetValue<BroadcastShowCard>();
            if (data.code != 0)
            {
                MsgBox.ShowErr(data.msg, 2);
                return;
            }
            var game = data.game;

            foreach (var p in Data.Game.Players.Values)
            {
                if (p.Info.id != game.playerId)
                {
                    PutCard(p.DeskId, "-1|-1|-1|-1|-1");
                    continue;
                }

                PutCard(p.DeskId, game.cards, game.cardType);
            }

            HideTips();
            ui.GetChild("btnScores").visible = false;

        }

        private void OnBroadcastScore(NotificationArg arg)
        {
            var data = arg.GetValue<BroadcastScore>();
            if (data.code != 0)
            {
                MsgBox.ShowErr(data.msg, 2);
                return;
            }
            ShowSetScore(data.game.roomId, data.game.playerId, data.game.score);
            // 如果是自己，就隐藏下注按钮
            if (data.game.playerId == Data.User.Id)
            {
                // 如果是自己，就播放下注音乐
                gameAudio.clip = Resources.Load<AudioClip>("Game/audio/game_setScore");
                gameAudio.Play();
                ui.GetChild("btnScores").visible = false;
            }
            Debug.Log(data.game.playerId + " 下注：" + data.game.score);
        }


        void onBtnTimesClick(EventContext e)
        {
            GButton btn = e.sender as GButton;
            Api.Game.SetTimes(Data.Game.Id, int.Parse(btn.data + ""));
        }

        void onScore(EventContext e)
        {

            GButton btn = e.sender as GButton;
            Api.Game.SetScore(Data.Game.Id, int.Parse(btn.title));
        }


        private void OnBroadcastBanker(NotificationArg arg)
        {
            var data = arg.GetValue<BroadcastBanker>();
            if (data.code != 0)
            {
                MsgBox.ShowErr(data.msg, 2);
                return;
            }
            ui.GetChild("btnTimes").visible = false;
            Debug.Log("庄家是：" + data.game.playerId + "  " + data.game.banker);
            if (Data.User.Id != data.game.playerId)
            {
                ui.GetChild("btnScores").visible = true;
            }


            foreach (var p in Data.Game.Players.Values)
            {
                if (p.Info.id == data.game.playerId)
                {
                    p.IsBanker = true;
                    p.PlayerUi.GetChild("zhuang").visible = true;
                }
                else
                {
                    // 隐藏所有闲家家抢庄倍数
                    var tc = p.PlayerUi.GetController("times");
                    tc.selectedIndex = 0;
                }
            }

            hideSetScoreTips();
            nScoreTips = 10;
            showSetScoreTips();
        }

        private void OnBroadcastTimes(NotificationArg arg)
        {
            var data = arg.GetValue<BroadcastTimes>();
            if (data.code != 0)
            {
                MsgBox.ShowErr(data.msg, 2);
                return;
            }
            ShowSetTimes(data.game.roomId, data.game.playerId, data.game.times);
            // 如果是自己，就隐藏抢庄按钮
            if (data.game.playerId == Data.User.Id)
            {
                ui.GetChild("btnTimes").visible = false;
            }
            Debug.Log(data.game.playerId + "抢庄倍数：" + data.game.times);
        }

        private void onBtnStartClick(EventContext context)
        {
            Api.Game.Start(Data.Game.Id);
        }
        private void OnResGameStart(NotificationArg arg)
        {

            var data = arg.GetValue<ResGameStart>();
            if (data.code != 0)
            {
                MsgBox.ShowErr(data.msg, 2);
                return;
            }

            // 每次游戏开始，都更新一下房间信息
            Api.Room.GetRoom(Data.Game.Id);

            // 播放开始音乐
            gameAudio.clip = Resources.Load<AudioClip>("Game/audio/game_start");
            gameAudio.Play();

            foreach (var p in Data.Game.Players.Values)
            {
                if (p.DeskId == data.game.deskId)
                {
                    PutCard(p.DeskId, data.game.cards + "|-|-1", true);
                }
                else
                {
                    PutCard(p.DeskId, "-1|-1|-1|-1|-1", true);
                }
            }
            showCenterCard();
            btnStart.visible = false;
            hideNius();

            // 下注按钮积分初始化
            var s = scores[Data.Game.info.score];
            var ss = s.Split('/');
            btnScore1.title = ss[0];
            btnScore2.title = ss[1];

            ui.GetChild("btnTimes").visible = true;

            nTimesTips = 10;
            showSetTimesTips();

            foreach (var p in Data.Game.Players.Values)
            {
                // 隐藏用户头像上方的积分图标
                p.PlayerUi.GetChild("setScore").visible = false;
                // 隐藏用户头像上方的庄家标志
                p.PlayerUi.GetChild("zhuang").visible = false;


                // 隐藏所有玩家抢庄倍数
                var tc = p.PlayerUi.GetController("times");
                tc.selectedIndex = 0;
            }
        }

        void ShowSetTimes(int roomId, int uid, int times)
        {
            var tc = Data.Game.GetPlayer(uid).PlayerUi.GetController("times");
            tc.selectedIndex = times + 1;
        }

        void ShowSetScore(int roomId, int uid, int score)
        {
            var p = Data.Game.GetPlayer(uid);
            var scoreUi = p.PlayerUi.GetChild("setScore").asTextField;
            scoreUi.visible = true;
            scoreUi.text = score + "";
            var tc = p.PlayerUi.GetController("times");
            tc.selectedIndex = 0;
        }

        int nTimesTips = 0;
        void showSetTimesTips()
        {

            if (nTimesTips > 0)
            {
                Invoke("showSetTimesTips", 1);
            }

            ShowTips("发牌完毕，抢庄中(" + nTimesTips + ")···");
            if (nTimesTips <= 0)
            {
                HideTips();
            }
            nTimesTips--;
        }
        void hideSetTimesTips()
        {
            nTimesTips = 0;
            HideTips();
        }

        int nScoreTips = 0;
        void showSetScoreTips()
        {
            if (nScoreTips > 0)
            {
                Invoke("showSetScoreTips", 1);
            }
            ShowTips("下注中(" + nScoreTips + ")···");
            if (nScoreTips <= 0)
            {
                HideTips();
            }
            nScoreTips--;
        }
        void hideSetScoreTips()
        {
            nScoreTips = 0;
            HideTips();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deskId"></param>
        /// <param name="cards"></param>
        /// <param name="cardType"></param>
        /// <param name="audio">是否播放牌型提示音</param>
        void PutCard(int deskId, string cards, int cardType, bool playAudio = false)
        {
            if (playAudio)
            {

                gameAudio.clip = Resources.Load<AudioClip>("Game/audio/nv_niu" + cardType);
                gameAudio.Play();
            }

            var pxStr = "";
            var pxx = cards.Split('|');
            for (var i = 0; i < 5; i++)
            {
                pxStr += pxx[i] + "|";
                switch (cardType)
                {
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                    case 6:
                    case 7:
                    case 8:
                    case 9:
                    case 10:
                        pxStr += i == 2 ? "-|" : "";
                        break;
                    case 12:
                        pxStr += i == 3 ? "-|" : "";
                        break;
                }
            }
            pxStr = pxStr.Substring(0, pxStr.Length - 1);
            PutCard(deskId, pxStr);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deskId"></param>
        /// <param name="cards"></param>
        /// <param name="action">是否执行发牌动画</param>
        void PutCard(int deskId, string cards, bool action = false)
        {

            Data.Player player = null;
            foreach (var p in Data.Game.Players.Values)
            {
                if (p.DeskId == deskId)
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
                    cpos.x += player.DeskId == Data.Game.DeskId ? 40 : 15;
                    continue;
                }

                var n = int.Parse(v);
                string cardName = getName(n);
                GImage card = UIPackage.CreateObject("qipai", cardName).asImage;
                card.position = cpos;

                var scale = 1.0f;
                if (player.DeskId == Data.Game.DeskId)
                {
                    cpos.x += 50;
                }
                else
                {
                    cpos.x += 23;
                    scale = 0.6f;
                }

                if (action)
                {
                    //putCardAction(cpos, scale);
                }

                card.SetScale(scale, scale);
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

        void showCenterCard()
        {

            GImage card1 = UIPackage.CreateObject("qipai", "Card_0").asImage;
            card1.position = cardCenter.position;
            card1.AddRelation(cardCenter, RelationType.Middle_Middle);
            card1.AddRelation(cardCenter, RelationType.Center_Center);
            ui.AddChild(card1);

            centerCard = UIPackage.CreateObject("qipai", "Card_0").asImage;
            centerCard.position = cardCenter.position;
            centerCard.AddRelation(cardCenter, RelationType.Middle_Middle);
            centerCard.AddRelation(cardCenter, RelationType.Center_Center);
            ui.AddChild(centerCard);

        }

        /// <summary>
        /// 发牌动画
        /// </summary>
        /// <param name="pos">牌发到哪里</param>
        /// <param name="scale">发到目的地牌的大小</param>
        void putCardAction(Vector3 pos, float scale)
        {
            var tPos = cardCenter.position;
            for (var i = 0; i < 100; i++)
            {
                tPos.x++;
                tPos.y++;
                centerCard.SetPosition(tPos.x, tPos.y, tPos.z);
            }
            centerCard.SetScale(scale, scale);
            //centerCard.position = cardCenter.position;
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
            this.CancelInvoke();
            tips.visible = false;
        }

    }

}