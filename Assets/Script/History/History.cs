using FairyGUI;
using Network.Msg;
using Notification;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

namespace History
{
    public class History : MonoBehaviour
    {
        GComponent ui;
        List<GComponent> users = new List<GComponent>();
        CalcWindow calcWindow;
        private void Awake()
        {
            bindEvents();

            ui = GetComponent<UIPanel>().ui;
            calcWindow = new CalcWindow(ui);

            for (var i = 1; i <= 10; i++)
            {
                users.Add(ui.GetChild("u" + i).asCom);
            }

            ui.GetChild("btnBack").onClick.Add(() =>
            {
                SceneManager.LoadScene("Menu");
            });

            ui.GetChild("btnLast").onClick.Add(() =>
             {
                 Api.Game.GetGameResult(++tPage);
             });

            ui.GetChild("btnNext").onClick.Add(() =>
             {
                 --tPage;
                 if (tPage < 1)
                 {
                     ++tPage;
                     MsgBox.ShowErr("已经是最新的战绩了");
                     return;
                 }
                 Api.Game.GetGameResult(tPage);
             });

            ui.GetChild("btnCalc").onClick.Add(() =>
            {
                calcWindow.Show();
            });
        }

        private void bindEvents()
        {
            Handler.Init();
            Handler.Add<ResGameResult>(MsgID.ResGameResult, NotificationType.Network_OnResGameResult);

            Handler.AddListenner(NotificationType.Network_OnResGameResult, OnResGameResult);
        }


        int page = 1;
        int tPage = 1;
        string[] scores = {
            "1/2",
            "2/4",
            "3/6",
            "4/8",
            "5/10",
            "10/20"
        };
        private void OnResGameResult(NotificationArg arg)
        {

            ui.GetChild("textTip").text = "";

            var data = arg.GetValue<ResGameResult>();
            if (data.code != 0)
            {
                tPage = page;
                MsgBox.ShowErr(data.msg);
                return;
            }
            page = tPage;

            ui.GetChild("roomId").text = "房号:" + data.room.id;
            ui.GetChild("count").text = "局数:" + data.room.count;
            ui.GetChild("score").text = "底分:" + scores[data.room.score];

            DateTime dt = DateTime.ParseExact(data.room.createdAt, "yyyy-MM-ddTHH:mm:ss+08:00", System.Globalization.CultureInfo.CurrentCulture);
            ui.GetChild("time").text = "" + dt.ToString("yyyy-MM-dd HH:mm:ss");


            foreach (var u in users)
            {
                u.visible = false;
            }

            var i = 0;
            var minG = data.games[0]; // 总分最小的玩家的数据
            var maxG = data.games[1];
            var minU = users[0]; // 总分最小的玩家的ui句柄
            var maxU = users[1];
            foreach (var u in data.games)
            {
                var userUi = users[i++];


                // 找出最大最小
                if (u.totalScore > maxG.totalScore)
                {
                    maxG = u;
                    maxU = userUi;

                }
                else if (u.totalScore < minG.totalScore)
                {
                    minG = u;
                    minU = userUi;
                }

                userUi.visible = true;
                var userInfo = userUi.GetChild("userInfo").asCom;
                userInfo.GetChild("imgAvatar").asLoader.url = "/static" + u.avatar;

                var nick = userInfo.GetChild("textNick");
                var id = userInfo.GetChild("textId");
                nick.text = "昵称:" + u.nick;
                id.text = "ID:" + u.uid;


                var score = userUi.GetChild("score").asTextField;
                TextFormat tf = score.textFormat;
                if (u.totalScore >= 0)
                {
                    tf.font = "ui://1ad63yxfq9h6q5";
                }
                else
                {
                    tf.font = "ui://1ad63yxfq9h6q6";
                }

                score.textFormat = tf;
                score.text = (u.totalScore > 0 ? "+" : "") + u.totalScore;
                score.data = score.text;
                var zhuang = userUi.GetChild("zhuang");
                zhuang.text = u.banks + "";
            }

            // 显示大赢家和土豪
            if (minG.totalScore == 0)
            {
                return;
            }
            minU.GetController("winner").selectedIndex = 2;
            maxU.GetController("winner").selectedIndex = 1;
        }

        // Start is called before the first frame update
        void Start()
        {
            Api.Game.GetGameResult();
        }

        // Update is called once per frame
        void Update()
        {
            Handler.HandleMessage();
        }
    }
}