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

        footer.GetChild("btnRule").onClick.Add(()=> {
            MsgWindow msgWindow = new MsgWindow();
            
            msgWindow.Show();
            msgWindow.position = new Vector2();
            msgWindow.width = mainUI.width;
            msgWindow.height = mainUI.height;
            msgWindow.SetTitle("游戏规则");
            string rule = @"<b align='center'>牌型</b>

<b>五小牛：</b>5张牌点数之和小于等于10
<b>炸弹：</b>有4张牌一样
<b>五花牛：</b>五张牌都是花牌(J/Q/K)组成
<b>牛牛：</b>五张牌中第一组三张牌和第一组二张牌之和分别为10的整数倍。如： 3/7/K/10/J
<b>有牛：</b>五张牌中有三张的点数之和为10点的整数倍，并且另外两张牌之和与10进行取余，所得之数即为牛几。如: 2/8/J/6/3，即为牛9。
<b>无牛：</b>五张牌中没有任意三张牌点数之和为10的整数倍。例如: A/8/4/K/7


<b>牌型比较规则</b>

<b>数字比较：</b>A>K>Q>J>10>9>8>7>6>5>4>3>2
<b>花色比较：</b>黑桃>红桃>梅花>方块
<b>牌型比较：</b>五小牛>炸弹>五花牛>牛牛>有牛>无牛
<b>无牛牌型比较：</b>取其中最大的一张牌比较大小，牌大的赢，大小相同比花色
<b>有牛牌型比较：</b>取其中最大的一张牌比较大小，牌大的赢，大小相同比花色
<b>炸弹之间大小比较：</b>取炸弹牌比较大小
<b>五小牛牌型比较：</b>庄吃闲

";
            msgWindow.SetMsg(rule);
        });


        footer.GetChild("btnMsg").onClick.Add(() => {
            Api.User.GetNotice();
        });

        footer.GetChild("btnShare").onClick.Add(() => {
            Api.User.GetShareText();
        });
    }

    private void BindListenners()
    {
        Handler.Init();
        
        Handler.Add<ResCreateRoom>(MsgID.ResCreateRoom, NotificationType.Network_OnResCreateRoom);
        Handler.Add<ResCreateClub>(MsgID.ResCreateClub, NotificationType.Network_OnResCreateClub);
        Handler.Add<ResNotice>(MsgID.ResNotice, NotificationType.Network_OnResNotice);
        Handler.Add<ResRollText>(MsgID.ResRollText, NotificationType.Network_OnResRollText);
        Handler.Add<ResShareText>(MsgID.ResShareText, NotificationType.Network_OnResShareText);

        
        
        Handler.AddListenner(NotificationType.Network_OnResCreateRoom, OnResCreateRoom);
        Handler.AddListenner(NotificationType.Network_OnResCreateClub, OnResCreateClub);
        Handler.AddListenner(NotificationType.Network_OnResNotice, OnResNotice);
        Handler.AddListenner(NotificationType.Network_OnResRollText, OnResRollText);
        Handler.AddListenner(NotificationType.Network_OnResShareText, OnResShareText);
    }

    private void OnResShareText(NotificationArg arg)
    {
        var data = arg.GetValue<ResShareText>();
        if (data.code != 0)
        {
            MsgBox.ShowErr(data.msg);
            return;
        }
#if UNITY_IPHONE
#elif UNITY_ANDROID
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        jo.Call("shareText", data.shareText);
#endif
    }

    private void OnResRollText(NotificationArg arg)
    {
        var data = arg.GetValue<ResRollText>();
        if (data.code != 0)
        {
            MsgBox.ShowErr(data.msg);
            return;
        }
        mainUI.GetChild("rollText").asCom.GetChild("rollText").text = data.rollText!=""? data.rollText:"仅供娱乐，请勿用于赌博活动。";
    }

    private void OnResNotice(NotificationArg arg)
    {
        var data = arg.GetValue<ResNotice>();
        if (data.code != 0)
        {
            MsgBox.ShowErr(data.msg);
            return;
        }

        MsgWindow msgWindow = new MsgWindow();

        msgWindow.Show();
        msgWindow.position = new Vector2();
        msgWindow.width = mainUI.width;
        msgWindow.height = mainUI.height;
        msgWindow.SetTitle("消息通知");
        msgWindow.SetMsg(data.notice!=""?data.notice:"<center>暂无通知</center>");
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
        Api.User.GetRollText();
    }


    // Update is called once per frame
    void Update()
    {
        Utils.Handler.HandleMessage();
    }

}
