using FairyGUI;
using Network;
using Network.Msg;
using Notification;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

public class Connected : MonoBehaviour
{
    bool isConnected = true;
    // Start is called before the first frame update
    void Start()
    {
        Handler.Add<ResLoginByToken>(MsgID.ResLoginByToken, NotificationType.Network_OnResLoginByToken);

        Handler.AddListenner(NotificationType.Network_OnConnected, OnConnected);
        Handler.AddListenner(NotificationType.Network_OnDisconnected, OnDisconnected);
        Handler.AddListenner(NotificationType.Network_OnResLoginByToken, OnResLoginByToken);
    }

    private void OnConnected(NotificationArg arg)
    {
        Debug.Log("网络连接成功");
        isConnected = true;
        Api.User.LoginByToken();
    }

    private void OnDisconnected(NotificationArg arg)
    {
        Debug.LogWarning("网络连接中断");
        
        isConnected = false;
        Manager.Inst.Connect();
    }

    private void OnResLoginByToken(NotificationArg arg)
    {
        var data = arg.GetValue<ResLoginByToken>();
        if (data.code != 0)
        {
            MsgBox.ShowErr(data.msg, 2);
            Data.User.Token = "";
            SceneManager.LoadScene("Login");
            return;
        }
        Debug.Log(data.code + "  " + data.msg + "   " + data.token);
        Data.User.Token = data.token;
    }

    void Update()
    {
        if (!isConnected)
        {
            Utils.MsgBox.ShowErr("网络重连中···");
        }
    }

}
