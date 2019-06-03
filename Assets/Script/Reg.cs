using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using System.Text.RegularExpressions;
using System.Net.Http;
using System;
using UnityEngine.SceneManagement;
using Network;
using Network.Msg;

public class Reg : MonoBehaviour
{
    GComponent mainUI;
    GTextInput inputNick;
    GTextInput inputPhone;
    GTextInput inputCode;
    GTextInput inputPass;
    GTextInput inputConfirm;

    ErrorWindow errorWindow;

    private static bool b = false;

    private void Awake()
    {
        Utils.Handler.Clear();
        Utils.Handler.Add<ResReg>(MsgID.ResReg, Network.EventType.Network_OnResReg);
        Utils.Handler.Add<ResCode>(MsgID.ResCode, Network.EventType.Network_OnResCode);
        if (!b)
        {
            EventCenter ec = EventCenter.Inst;
            ec.AddEventListener(Network.EventType.Network_OnResReg, OnResReg);
            ec.AddEventListener(Network.EventType.Network_OnResCode, OnResCode);
            b = true;
        }
    }

    void OnResReg(EventArg arg)
    {
        var data = arg.GetValue<ResReg>();
        if (data.code != 0)
        {
            Utils.MsgBox.ShowErr(data.msg, 2);
            return;
        }
        
        SceneManager.LoadScene("Login");
    }
    
    void OnResCode(EventArg arg)
    {
        var data = arg.GetValue<ResCode>();
        if (data==null)
        {
            return;
        }
        Utils.MsgBox.ShowErr(data.msg, 2);
    }


    // Start is called before the first frame update
    void Start()
    {
        mainUI = GetComponent<UIPanel>().ui;

        errorWindow = new ErrorWindow();

        inputNick = mainUI.GetChild("inputNick").asTextInput;
        inputPhone = mainUI.GetChild("inputPhone").asTextInput;
        inputCode = mainUI.GetChild("inputCode").asTextInput;
        inputPass = mainUI.GetChild("inputPass").asTextInput;
        inputConfirm = mainUI.GetChild("inputConfirm").asTextInput;

        mainUI.GetChild("btnReg").onClick.Add(btnRegClick);
        mainUI.GetChild("btnGetCode").onClick.Add(btnGetCodeClick);
        mainUI.GetChild("btnLogin").onClick.Add(() =>
        {
            SceneManager.LoadScene("Login");
        });
    }

    void btnGetCodeClick()
    {
        Api.User.GetCode(inputPhone.text);
    }

    void btnRegClick()
    {
        if (inputNick.text.Length < 1 || inputNick.text.Length > 6)
        {
            Utils.MsgBox.ShowErr("昵称长度必须在1~6个字之间");
            return;
        }
        else if (!Regex.IsMatch(inputPhone.text, @"^1[34578]\d{9}$"))
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

        Api.User.Reg(LoginType.MobilePass, inputNick.text, inputPhone.text, inputPass.text, inputCode.text);

    }

    // Update is called once per frame
    void Update()
    {
        Utils.Handler.HandleMessage();
    }
    
}