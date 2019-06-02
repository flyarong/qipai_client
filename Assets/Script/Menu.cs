using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using Api;

using System;

public class Menu : MonoBehaviour
{
    private GComponent mainUI;
    private CreateClubWindow createClubWindow;
    private CreateRoomWindow createRoomWindow;
    private JoinWindow joinWindow;

    public Menu()
    {

    }

    void Awake()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        mainUI = GetComponent<UIPanel>().ui;
        createClubWindow = new CreateClubWindow();
        createRoomWindow = new CreateRoomWindow();
        joinWindow = new JoinWindow();

        new Api.User().GetUserInfo();
        
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
    
    private void Start()
    {
        EventCenter.Broadcast(NoticeType.RoomList);

    }

    
    // Update is called once per frame
    void Update()
    {
        
    }
    
}
