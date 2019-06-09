using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using UnityEngine.SceneManagement;
using System;
using Utils;
using Notification;
using Network.Msg;
using Network;

namespace Game
{
    public class Room : MonoBehaviour
    {
        GComponent ui;
        GButton btnStart;
        RightWindow right;
        private string[] scores = {
            "1/2",
            "2/4",
            "3/6",
            "4/8",
            "5/10",
            "10/20"
        };

        private void Awake()
        {
            bindEvents();

            //Data.Room.Players.Clear();
            ui = GetComponent<UIPanel>().ui;
            right = new RightWindow();
            ui.GetChild("btnSetting").onClick.Add(onSettingClick);
            btnStart = ui.GetChild("btnStart").asButton;
            
        }

      

        private void bindEvents()
        {
            Handler.Init();
            Handler.Add<ResLoginByToken>(MsgID.ResLoginByToken, NotificationType.Network_OnResLoginByToken);
            Handler.Add<ResRoom>(MsgID.ResRoom, NotificationType.Network_OnResRoom);
            Handler.Add<ResJoinRoom>(MsgID.ResJoinRoom, NotificationType.Network_OnResJoinRoom);
            Handler.Add<ResSit>(MsgID.ResSit, NotificationType.Network_OnResSit);
            Handler.Add<ResLeaveRoom>(MsgID.ResLeaveRoom, NotificationType.Network_OnResLeaveRoom);
            Handler.Add<ResUserInfo>(MsgID.ResUserInfo, NotificationType.Network_OnResUserInfo);
            Handler.Add<BroadcastSitRoom>(MsgID.BroadcastSitRoom, NotificationType.Network_OnBroadcastSitRoom);
            Handler.Add<ResDeleteRoom>(MsgID.ResDeleteRoom, NotificationType.Network_OnResDeleteRoom);

            Handler.AddListenner(NotificationType.Network_OnConnected, OnConnected);
            Handler.AddListenner(NotificationType.Network_OnDisconnected, OnDisconnected);
            Handler.AddListenner(NotificationType.Network_OnResLoginByToken, OnResLoginByToken);
            Handler.AddListenner(NotificationType.Network_OnResRoom, OnResRoom);
            Handler.AddListenner(NotificationType.Network_OnResJoinRoom, OnResJoinRoom);
            Handler.AddListenner(NotificationType.Network_OnResSit, OnResSit);
            Handler.AddListenner(NotificationType.Network_OnResLeaveRoom, OnResLeaveRoom);
            Handler.AddListenner(NotificationType.Network_OnResUserInfo, OnResUserInfo);
            Handler.AddListenner(NotificationType.Network_OnBroadcastSitRoom, OnBroadcastSitRoom);
            Handler.AddListenner(NotificationType.Network_OnResDeleteRoom, OnResDeleteRoom);
        }

        private void OnResDeleteRoom(NotificationArg arg)
        {
            var data = arg.GetValue<ResDeleteRoom>();
            if (data.code != 0)
            {
                MsgBox.ShowErr(data.msg, 2);
                return;
            }
            if (data.roomId != Data.Room.Id)
            {
                Debug.LogWarning("收到不属于该房间的消息：ResDeleteRoom");
                return;
            }
            MsgBox.ShowErr("房间已解散",1);
            Data.Room.Id = 0;
            SceneManager.LoadScene("Menu");
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
            Data.User.Token = data.token;
        }

        private void OnBroadcastSitRoom(NotificationArg arg)
        {
            var data = arg.GetValue<BroadcastSitRoom>();

            if (data.code != 0)
            {
                MsgBox.ShowErr(data.msg);
                return;
            }
            if (data.roomId != Data.Room.Id)
            {
                Debug.LogWarning("收到不属于该房间的消息：BroadcastSitRoom");
                return;
            }
            AddPlayer(data.deskId, data.uid);
            Api.User.GetUserInfo(data.uid);
        }

        private void OnResUserInfo(NotificationArg arg)
        {
            var data = arg.GetValue<ResUserInfo>();

            if (data.code != 0)
            {
                MsgBox.ShowErr(data.msg);
                return;
            }

            var p = Data.Room.GetPlayer(data.user.id);
            if (p == null)
            {
                Debug.LogWarning("当前房间列表不存在该用户：" + data.user.id);
                return;
            }

            Data.PlayerInfo info = new Data.PlayerInfo();
            info.id = data.user.id;
            info.nick = data.user.nick;
            info.ip = data.user.ip;
            info.card = data.user.card;
            info.avatar = data.user.avatar;
            info.address = data.user.address;
            p.Info = info;

            p.PlayerUi.GetChild("name").text = data.user.nick;
            p.PlayerUi.GetChild("avatar").asLoader.url = "/static" + data.user.avatar;
            p.PlayerUi.visible = true;

        }

        private void OnResLeaveRoom(NotificationArg arg)
        {
            var data = arg.GetValue<ResLeaveRoom>();

            if (data.code != 0)
            {
                MsgBox.ShowErr(data.msg);
                return;
            }

            // 如果是自己退出，就返回菜单页
            if (data.uid == Data.User.Id)
            {
                Data.Room.Id = 0;
                Data.Room.info = null;
                SceneManager.LoadScene("Menu");
                return;
            }

            // 其他用户退出
            var player = Data.Room.GetPlayer(data.uid);
            if(player==null)
            {
                Debug.LogWarning("用户退出失败：" + data.uid);
                return;
            }
            player.PlayerUi.visible = false;
            Data.Room.Players.Remove(player);
        }

        /// <summary>
        /// 请求退出房间
        /// </summary>
        private void exit()
        {
            Api.Room.Leave(Data.Room.Id);
        }

        private void OnResSit(NotificationArg arg)
        {
            var data = arg.GetValue<ResSit>();

            if (data.code != 0)
            {
                MsgBox.ShowErr(data.msg);
                if (data.msg == "您当前正在其他房间")
                {
                    Data.Room.Id = data.roomId;
                    Data.Room.info = null;
                    SceneManager.LoadScene("Menu");
                    return;
                }
                exit();
                return;
            }
            
            Data.Room.DeskId = data.deskId;
            addPlayers(data.players);
        
            // 如果是房主，显示开始按钮
            if (data.uid == Data.User.Id)
            {
                btnStart.visible = true;
            }
            
        }

        // 添加玩家
        void addPlayers(List<PlayerInfo> players)
        {
            Data.Room.RemoveAllPlayers();
            foreach (var p in players)
            {
                AddPlayer(p.deskId, p.uid);
            }

            foreach (var p in players)
            {
                Api.User.GetUserInfo(p.uid);
            }
        }

        private void OnResJoinRoom(NotificationArg arg)
        {
            var data = arg.GetValue<ResJoinRoom>();

            if (data.code != 0)
            {
                MsgBox.ShowErr(data.msg);
                exit();
                return;
            }

            Api.Room.GetRoom(Data.Room.Id);
            Api.Room.Sit(Data.Room.Id);

            // 暂时不用围观功能，所以后面两句不需要调用

            //Data.Room.RemoveAllPlayers();
            //addPlayers(data.players);
        }

        Data.Player AddPlayer(int deskId, int uid)
        {
            Data.Player player = new Data.Player();
            Data.PlayerInfo info = new Data.PlayerInfo();
            info.id = uid;

            player.Info = info;
            player.DeskId = deskId;
            player.Index = player.DeskId - Data.Room.DeskId;
            if (player.Index < 0)
            {
                player.Index = 10 + player.Index;
            }

            player.PlayerUi = ui.GetChild("player" + (player.Index + 1)).asCom;
            
            Data.Room.Players.Add(player);
            
            return player;
        }

        private void OnResRoom(NotificationArg arg)
        {
            var data = arg.GetValue<ResRoom>();

            if (data.code != 0)
            {
                MsgBox.ShowErr(data.msg);
                exit();
                return;
            }

            var room = data.room;

            Data.Room.info = room;

            var text = "";
            if (Data.Club.Id == "")
            {
                text += "房号：" + room.id;
            }
            else
            {
                text += "茶楼：" + Data.Club.Id + "  第" + room.id + "桌";
            }

            text += "\n";
            text += "底分：" + scores[room.score];
            text += "\n";
            text += "局数：" + room.current + "/" + room.count;

            ui.GetChild("infoText").text = text;

            // 获取房间信息成功后，坐下
            Api.Room.Sit(Data.Room.Id);
        }

        private void Start()
        {
            Api.Room.JoinRoom(Data.Room.Id);
        }

        void onSettingClick()
        {
            GRoot.inst.ShowPopup(right);
        }

        private void Update()
        {
            Handler.HandleMessage();
        }

        //private void OnDestroy()
        //{
        //EventCenter.RemoveListener<string>(NoticeType.GameBegin, GameBegin);
        //EventCenter.RemoveListener<string, string>(NoticeType.RoomExit, ExitRoom);
        //EventCenter.RemoveListener<string, string>(NoticeType.PlayerSitDown, RoomJoin);
        //}

        //void GameBegin(string roomId)
        //{
        //    updataInfo();
        //}

        //void ExitRoom(string roomId, string uid)
        //{
        //    Debug.Log(uid + " 退出 " + roomId);

        //    if (uid == Data.User.Id + "")
        //    {
        //        SceneManager.LoadScene("Menu");
        //        return;
        //    }

        //    if (roomId != Data.Room.Id)
        //    {
        //        return;
        //    }
        //    var p = GetPlayer(int.Parse(uid));
        //    if (p == null)
        //    {
        //        Utils.MsgBox.ShowErr("获取玩家失败");
        //        return;
        //    }

        //    p.PlayerUi.visible = false;
        //    Data.Room.Players.Remove(p);

        //}

        //void RoomJoin(string roomId, string uid)
        //{
        //    if (roomId != Data.Room.Id)
        //    {
        //        Debug.Log("不是本房间的消息");
        //        return;
        //    }
        //    var j = new Api.Room().Player(Data.Room.Id, uid);
        //    if (j == null)
        //    {
        //        Utils.MsgBox.ShowErr("网络出现状况");
        //        return;
        //    }
        //    else if (j["code"].n != 0)
        //    {
        //        Utils.MsgBox.ShowErr(j["msg"].str);
        //        return;
        //    }
        //    var p = j["data"]["player"];
        //    GetPlayer((int)p["desk_id"].n, (int)p["uid"].n);
        //}

        //private void Start()
        //{
        //initRoom();
        //var j = new Api.Room().Player(Data.Room.Id);
        //if (j == null)
        //{
        //    Utils.MsgBox.ShowErr("网络出现状况");
        //    return;
        //}
        //else if (j["code"].n != 0)
        //{
        //    Utils.MsgBox.ShowErr(j["msg"].str);
        //    return;
        //}
        //var p = j["data"]["player"];
        //Data.Room.DeskId = (int)p["desk_id"].n;

        //initPlayers();
        //}

        //Game.Player GetPlayer(int uid)
        //{
        //    foreach (var p in Data.Room.Players)
        //    {
        //        // 只返回可见的，隐藏的都是离开的
        //        if (p.UserInfo["id"].n == uid)
        //        {
        //            return p;
        //        }
        //    }
        //    return null;
        //}





        //void initRoom()
        //{
        //    updataInfo();

        //    // 如果是房主，显示开始按钮
        //    if (Data.Room.Info["uid"].n == Data.User.Id)
        //    {
        //        btnStart.visible = true;
        //    }

        //    // 下注按钮积分初始化
        //    var s = scores[(int)Data.Room.Info["score"].n];
        //    var ss = s.Split('/');
        //    btnScore1.title = ss[0];
        //    btnScore2.title = ss[1];
        //}



        //void updataInfo()
        //{
        //    var j = new Api.Room().GetRoom(Data.Room.Id);
        //    if (j["code"].n != 0)
        //    {
        //        Utils.MsgBox.ShowErr(j["msg"].str);
        //        return;
        //    }

        //    Data.Room.Info = j["data"]["room"];
        //    Debug.Log(Data.Room.Info);
        //    var room = Data.Room.Info;

        //    var text = "";
        //    if (Data.Club.Id == "")
        //    {
        //        text += "房号：" + room["id"];
        //    }
        //    else
        //    {
        //        text += "茶楼：" + Data.Club.Id + "  第" + room["id"] + "桌";
        //    }

        //    text += "\n";
        //    text += "底分：" + scores[(int)room["score"].n];
        //    text += "\n";
        //    text += "局数：" + room["current"].n + "/" + room["count"].n;

        //    ui.GetChild("infoText").text = text;
        //}

        //void initPlayers()
        //{
        //    var j = new Api.Room().Players(Data.Room.Id);
        //    if (j["code"].n != 0)
        //    {
        //        Utils.MsgBox.ShowErr(j["msg"].str);
        //        return;
        //    }

        //    var players = j["data"]["players"].list;

        //    foreach (var p in players)
        //    {
        //        GetPlayer((int)p["desk_id"].n, (int)p["uid"].n);
        //    }
        //}


    }

}