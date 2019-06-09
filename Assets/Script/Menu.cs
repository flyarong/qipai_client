using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using UnityEngine.SceneManagement;
using System;
using Utils;
using Network;
using Notification;
using Network.Msg;
public class Menu : MonoBehaviour
{
    private GComponent mainUI;
    private CreateClubWindow createClubWindow;
    private CreateRoomWindow createRoomWindow;
    private JoinWindow joinWindow;


    void Awake()
    {
        BindListenners();

        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        mainUI = GetComponent<UIPanel>().ui;
        createClubWindow = new CreateClubWindow();
        createRoomWindow = new CreateRoomWindow();
        joinWindow = new JoinWindow();


        mainUI.GetChild("right").asCom.GetChild("btnCreateClub").onClick.Add(() =>
        {
            Debug.Log("创建茶楼按钮被点击");

            createClubWindow.Show();
            createClubWindow.position = new Vector3();
            createClubWindow.width = mainUI.width;
            createClubWindow.height = mainUI.height;
        });

        mainUI.GetChild("right").asCom.GetChild("btnCreateRoom").onClick.Add(() =>
        {
            Debug.Log("创建房间按钮被点击");

            createRoomWindow.Show();
            createRoomWindow.position = new Vector3();
            createRoomWindow.width = mainUI.width;
            createRoomWindow.height = mainUI.height;
        });

        mainUI.GetChild("right").asCom.GetChild("btnJoinRoom").onClick.Add(() =>
        {
            joinWindow.Show();
            joinWindow.Center();
        });
    }

    private void BindListenners()
    {
        Handler.Init();
        Handler.Add<ResLoginByToken>(MsgID.ResLoginByToken, NotificationType.Network_OnResLoginByToken);
        Handler.Add<ResCreateRoom>(MsgID.ResCreateRoom, NotificationType.Network_OnResCreateRoom);

        Handler.AddListenner(NotificationType.Network_OnConnected, OnConnected);
        Handler.AddListenner(NotificationType.Network_OnDisconnected, OnDisconnected);
        Handler.AddListenner(NotificationType.Network_OnResLoginByToken, OnResLoginByToken);
        Handler.AddListenner(NotificationType.Network_OnResCreateRoom, OnResCreateRoom);
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
            SceneManager.LoadScene("Login");
            return;
        }
        Debug.Log(data.code + "  " + data.msg + "   " + data.token);
        Data.User.Token = data.token;

    }

    private void OnResCreateRoom(NotificationArg arg)
    {
        var data = arg.GetValue<ResCreateRoom>();
        if (data.code != 0)
        {
            MsgBox.ShowErr(data.msg, 2);
            return;
        }
        Data.Room.Id = data.id;

        createRoomWindow.Hide();
        SceneManager.LoadScene("Game");
    }

    private void Start()
    {
        if (Data.Room.Id > 0)
        {
            SceneManager.LoadScene("Game");
        }
    }


    // Update is called once per frame
    void Update()
    {
        Utils.Handler.HandleMessage();
    }

}
