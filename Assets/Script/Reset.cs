using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using System.Text.RegularExpressions;
using System.Net.Http;
using System;
using UnityEngine.SceneManagement;
using Api;
using Utils;
using Network.Msg;
using Notification;

public class Reset : MonoBehaviour
{
    GComponent mainUI;

    GTextInput inputPhone;
    GTextInput inputCode;
    GTextInput inputPass;
    GTextInput inputConfirm;

    ErrorWindow errorWindow;

    private void Awake()
    {
        Handler.Init();
        Handler.Add<ResReset>(MsgID.ResReset, NotificationType.Network_OnResReset);
        Handler.Add<ResCode>(MsgID.ResCode, Notification.NotificationType.Network_OnResCode);

        Handler.AddListenner(NotificationType.Network_OnResReset, OnResReset);
        Handler.AddListenner(Notification.NotificationType.Network_OnResCode, OnResCode);
    }

    void OnResCode(NotificationArg arg)
    {
        var data = arg.GetValue<ResCode>();
        if (data.code != 0)
        {
            Utils.MsgBox.ShowErr(data.msg, 2);
            return;
        }
        Utils.MsgBox.ShowErr("验证码发送成功");
    }

    private void OnResReset(NotificationArg arg)
    {
        var data = arg.GetValue<ResReset>();
        if (data.code != 0)
        {
            MsgBox.ShowErr(data.msg);
            return;
        }
        Utils.MsgBox.ShowErr("密码重置成功");
        //SceneManager.LoadScene("Login");
    }

    // Start is called before the first frame update
    void Start()
    {
        mainUI = GetComponent<UIPanel>().ui;

        errorWindow = new ErrorWindow();

        inputPhone = mainUI.GetChild("inputPhone").asTextInput;
        inputCode = mainUI.GetChild("inputCode").asTextInput;
        inputPass = mainUI.GetChild("inputPass").asTextInput;
        inputConfirm = mainUI.GetChild("inputConfirm").asTextInput;

        mainUI.GetChild("btnReset").onClick.Add(btnResetClick);
        mainUI.GetChild("btnGetCode").onClick.Add(btnGetCodeClick);
        mainUI.GetChild("btnLogin").onClick.Add(() =>
        {
            SceneManager.LoadScene("Login");
        });
    }

    void btnGetCodeClick()
    {
        Api.User.GetResetCode(inputPhone.text);
    }

    void btnResetClick()
    {
        if (!Regex.IsMatch(inputPhone.text, @"^1[34578]\d{9}$"))
        {
            Utils.MsgBox.ShowErr("手机号格式不正确");
            return;
        }
        else if (!Regex.IsMatch(inputCode.text, @"\d+"))
        {
            Utils.MsgBox.ShowErr("请输入收到的数字验证码");
            return;
        }
        else if (inputPass.text.Length < 6 || inputPass.text.Length > 30)
        {
            Utils.MsgBox.ShowErr("密码长度必须在6~30之间");
            return;
        }
        else if (inputPass.text != inputConfirm.text)
        {
            Utils.MsgBox.ShowErr("两次输入的密码不一致");
            return;
        }

        Api.User.ChangePass(1, inputPhone.text, inputPass.text, inputCode.text);

    }

    

    // Update is called once per frame
    void Update()
    {
        Handler.HandleMessage();
    }
    
}