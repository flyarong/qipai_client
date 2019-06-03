using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using UnityEngine.SceneManagement;
using System;
using Utils;
using Network;
using Network.Msg;
public class Menu : MonoBehaviour
{
    private GComponent mainUI;
    private CreateClubWindow createClubWindow;
    private CreateRoomWindow createRoomWindow;
    private JoinWindow joinWindow;
    static bool b = false;

    void Awake()
    {
        Handler.Clear();
        Handler.Add<ResCreateRoom>(MsgID.ResCreateRoom, Network.EventType.Network_OnResCreateRoom);
        if (!b)
        {
            b = true;
            EventCenter ec = EventCenter.Inst;
            ec.AddEventListener(Network.EventType.Network_OnResCreateRoom, OnResCreateRoom);
            ec.AddEventListener(Network.EventType.Network_OnDisconnected, OnDisconnected);
        }

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

    private void OnDisconnected(EventArg arg)
    {
        Manager.Inst.Connect();
    }

    private void OnResCreateRoom(EventArg arg)
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

    }

    
    // Update is called once per frame
    void Update()
    {
        Utils.Handler.HandleMessage();
    }
    
}
