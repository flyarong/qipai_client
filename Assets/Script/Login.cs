﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using FairyGUI;
using System.Text.RegularExpressions;
using Utils;
using Network;
using Network.Msg;
using System;
using Notification;
using Newtonsoft.Json;

public class Login : MonoBehaviour
{
    GTextInput inputPhone;
    GTextInput inputPass;
    GComponent mainUI;

    private void Awake()
    {
        BindListenners();
        
        mainUI = GetComponent<UIPanel>().ui;
        mainUI.GetChild("btnReg").onClick.Add(() =>
        {
            Debug.Log("切换注册界面按钮被点击");
            SceneManager.LoadScene("Reg");
        });
        UIObjectFactory.SetLoaderExtension(typeof(Utils.HttpLoader));
        Data.User.Token = PlayerPrefs.GetString("token");
        inputPhone = mainUI.GetChild("inputPhone").asTextInput;
        inputPass = mainUI.GetChild("inputPass").asTextInput;

        mainUI.GetChild("btnLogin").onClick.Add(LoginBtnClick);

        mainUI.GetChild("btnReg").onClick.Add(() =>
        {
            Debug.Log("切换注册界面按钮被点击");
            SceneManager.LoadScene("Reg");
        });

        mainUI.GetChild("btnReset").onClick.Add(() =>
        {
            SceneManager.LoadScene("Reset");
        });
    }

    private void BindListenners()
    {
        Handler.Init();
        Handler.Add<ResLogin>(MsgID.ResLogin, NotificationType.Network_OnResLogin);

        Handler.AddListenner(NotificationType.Network_OnConnected, OnConnected);
        Handler.AddListenner(NotificationType.Network_OnDisconnected, OnDisconnected);
        Handler.AddListenner(NotificationType.Network_OnResLoginByToken, OnResLoginByToken);
        Handler.AddListenner(NotificationType.Network_OnResLogin, OnResLogin);
    }

    private void OnConnected(NotificationArg arg)
    {
        Api.User.LoginByToken();
    }

    private void OnDisconnected(NotificationArg arg)
    {
        Manager.Inst.Connect();
    }
    private void OnResLoginByToken(NotificationArg arg)
    {
        var data = arg.GetValue<ResLoginByToken>();
        if (data.code != 0)
        {
            MsgBox.ShowErr(data.msg, 2);
            Data.User.Token = "";
            return;
        }
        Debug.Log(data.code + "  " + data.msg + "   " + data.token);
        Data.User.Token = data.token;
        SceneManager.LoadScene("Menu");
    }

    void OnResLogin(NotificationArg arg)
    {
        ResLogin data = arg.GetValue<ResLogin>();

        if (data.code != 0)
        {
            MsgBox.ShowErr(data.msg, 2);
            return;
        }
        Debug.Log(data.code + "  " + data.msg + "   " + data.token);
        Data.User.Token = data.token;
        SceneManager.LoadScene("Menu");
    }

    private void Update()
    {
        Handler.HandleMessage();
    }


    void LoginBtnClick()
    {
        if (!Regex.IsMatch(inputPhone.text, @"^1[34578]\d{9}$"))
        {
            Utils.MsgBox.ShowErr("手机号格式不正确");
            return;
        }
        else if (inputPass.text.Length < 6 || inputPass.text.Length > 30)
        {
            Utils.MsgBox.ShowErr("密码长度必须在6~30之间");
            return;
        }

        Api.User.Login(LoginType.MobilePass, inputPhone.text, inputPass.text);

    }

    
}


