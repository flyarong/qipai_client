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
    GComponent list;

    void Awake()
    {
        BindListenners();

        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        mainUI = GetComponent<UIPanel>().ui;
        createClubWindow = new CreateClubWindow();
        createRoomWindow = new CreateRoomWindow();
        joinWindow = new JoinWindow();
        list = mainUI.GetChild("list").asCom;

        // 让茶楼和房间列表选项卡固定在用户选择的页面
        var clubOrRoom = list.GetController("tab");
        clubOrRoom.onChanged.Set(() =>
        {
            PlayerPrefs.SetInt("clubOrRoom", clubOrRoom.selectedIndex);
        });
        clubOrRoom.selectedIndex = PlayerPrefs.GetInt("clubOrRoom");



        mainUI.GetChild("right").asCom.GetChild("btnCreateClub").onClick.Add(() =>
        {
            createClubWindow.Show();
            createClubWindow.position = new Vector3();
            createClubWindow.width = mainUI.width;
            createClubWindow.height = mainUI.height;
        });

        mainUI.GetChild("right").asCom.GetChild("btnCreateRoom").onClick.Add(() =>
        {
            createRoomWindow.Show();
            createRoomWindow.position = new Vector3();
            createRoomWindow.width = mainUI.width;
            createRoomWindow.height = mainUI.height;
        });

        mainUI.GetChild("right").asCom.GetChild("btnJoinRoom").onClick.Add(() =>
        {
            PlayerPrefs.SetString("joinType", "room");
            joinWindow.Show();
            joinWindow.Center();
        });

        mainUI.GetChild("right").asCom.GetChild("btnJoinClub").onClick.Add(() =>
        {
            PlayerPrefs.SetString("joinType", "club");
            joinWindow.Show();
            joinWindow.Center();
        });


        var footer = mainUI.GetChild("footer").asCom;
        footer.GetChild("btnHistory").onClick.Add(()=> {
            SceneManager.LoadScene("History");
        });
    }

    private void BindListenners()
    {
        Handler.Init();
        
        Handler.Add<ResCreateRoom>(MsgID.ResCreateRoom, NotificationType.Network_OnResCreateRoom);
        Handler.Add<ResCreateClub>(MsgID.ResCreateClub, NotificationType.Network_OnResCreateClub);

        
        
        Handler.AddListenner(NotificationType.Network_OnResCreateRoom, OnResCreateRoom);
        Handler.AddListenner(NotificationType.Network_OnResCreateClub, OnResCreateClub);
    }

    private void OnResCreateClub(NotificationArg arg)
    {
        var data = arg.GetValue<ResCreateClub>();
        if (data.code != 0)
        {
            MsgBox.ShowErr(data.msg, 2);
            return;
        }
        Data.Club.Id = data.clubId;
        createClubWindow.Hide();
        SceneManager.LoadScene("Club");
    }
    
    private void OnResCreateRoom(NotificationArg arg)
    {
        var data = arg.GetValue<ResCreateRoom>();
        if (data.code != 0)
        {
            MsgBox.ShowErr(data.msg, 2);
            return;
        }
        Data.Game.Id = data.roomId;

        createRoomWindow.Hide();
        toGame();
    }

    void toGame()
    {
        SceneManager.LoadScene("Game");
    }

    void toClub()
    {
        SceneManager.LoadScene("Club");
    }

    private void Start()
    {
        if (Data.Game.Id > 0)
        {
            Invoke("toGame", 0.5f);
        }
        else if (Data.Club.Id > 0)
        {
            Invoke("toClub", 0.5f);
        }

    }


    // Update is called once per frame
    void Update()
    {
        Utils.Handler.HandleMessage();
    }

}
