using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using Utils;
using Network.Msg;
using Notification;
using System;

public class UserInfo : MonoBehaviour
{
    private GComponent mainUI;

    private void Awake()
    {
        Handler.Add<ResUserInfo>(MsgID.ResUserInfo, NotificationType.Network_OnResUserInfo);

        Handler.AddListenner(NotificationType.Network_OnResUserInfo, OnResUserInfo);

        mainUI = GetComponent<UIPanel>().ui;
    }

    private void OnResUserInfo(NotificationArg arg)
    {
        var data = arg.GetValue<ResUserInfo>();
        if (data.code != 0)
        {
            MsgBox.ShowErr(data.msg);
            return;
        }

        Data.User.Info = data.user;
        updateInfo();
    }

    void updateInfo()
    {
        var header = mainUI.GetChild("header").asCom;
        var userInfo = header.GetChild("userInfo").asCom;
        userInfo.GetChild("textNick").asTextField.text = "昵称:" + Data.User.Info.nick;
        userInfo.GetChild("textId").asTextField.text = "ID:" + Data.User.Info.id;
        userInfo.GetChild("textGold").asTextField.text = Data.User.Info.card + "";
        userInfo.GetChild("imgAvatar").asLoader.url = "/static" + Data.User.Info.avatar;
    }
    // Start is called before the first frame update
    void Start()
    {
        if (Data.User.Info == null)
        {
            Api.User.GetUserInfo(Data.User.Id);
        }
        else
        {
            updateInfo();
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}