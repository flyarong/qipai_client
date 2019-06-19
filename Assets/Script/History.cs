using FairyGUI;
using Network.Msg;
using Notification;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

public class History : MonoBehaviour
{
    GComponent ui;
    List<GComponent> users = new List<GComponent>();
    private void Awake()
    {
        bindEvents();

        ui = GetComponent<UIPanel>().ui;
        for (var i = 1; i <= 10; i++)
        {
            users.Add(ui.GetChild("u" + i).asCom);
        }
        ui.GetChild("btnBack").onClick.Add(() =>
        {
            SceneManager.LoadScene("Menu");
        });
    }

    private void bindEvents()
    {
        Handler.Init();
        Handler.Add<ResGameResult>(MsgID.ResGameResult, NotificationType.Network_OnResGameResult);

        Handler.AddListenner(NotificationType.Network_OnResGameResult, OnResGameResult);
    }


    int page = 0;
    private void OnResGameResult(NotificationArg arg)
    {
        var data = arg.GetValue<ResGameResult>();
        if (data.code != 0)
        {
            MsgBox.ShowErr(data.msg);
            return;
        }

        foreach(var u in users)
        {
            u.visible = false;
        }

        var i = 0;
        foreach (var u in data.games)
        {
            var userUi = users[i++];
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

            var zhuang = userUi.GetChild("zhuang");
            zhuang.text = u.banks + "";
        }
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
