using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using Api;
using Utils;
using System;
using Network.Msg;
using Notification;
using UnityEngine.SceneManagement;

namespace Club
{
    public class Club : MonoBehaviour
    {
        private GComponent ui;
        GList Tables;
        string[] scores = {
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
            ui = GetComponent<UIPanel>().ui;
            Tables = ui.GetChild("tableList").asCom.GetChild("tables").asList;
            var btnQuit = ui.GetChild("header").asCom.GetChild("btnQuit").asButton;
            btnQuit.onClick.Add(onBtnQuit);
            var btnNotice = ui.GetChild("header").asCom.GetChild("btnNotice").asButton;
            btnNotice.onClick.Add(onBtnNotice);
        }

        private void onBtnNotice(EventContext context)
        {
            MsgWindow msgWindow = new MsgWindow();

            msgWindow.Show();
            msgWindow.position = new Vector2();
            msgWindow.width = ui.width;
            msgWindow.height = ui.height;
            msgWindow.SetTitle("本茶楼公告");
            msgWindow.SetMsg("没有消息<img src='http://192.168.1.103:9988/static/avatar/Avatar115.png' width='200' height='200' />");
        }

        private void onBtnQuit(EventContext context)
        {
            exit();
        }

        private void bindEvents()
        {
            Handler.Init();

            Handler.Add<ResClub>(MsgID.ResClub, NotificationType.Network_OnResClub);
            Handler.Add<BroadcastJoinClub>(MsgID.BroadcastJoinClub, NotificationType.Network_OnBroadcastJoinClub);
            Handler.Add<BroadcastDelClub>(MsgID.BroadcastDelClub, NotificationType.Network_OnBroadcastDelClub);
            Handler.Add<ResCreateClubRoom>(MsgID.ResCreateClubRoom, NotificationType.Network_OnResCreateClubRoom);
            Handler.Add<ResClubRooms>(MsgID.ResClubRooms, NotificationType.Network_OnResClubRooms);
            Handler.Add<ResRoom>(MsgID.ResRoom, NotificationType.Network_OnResRoom);
            Handler.Add<ResClubGameStart>(MsgID.ResGameStart, NotificationType.Network_OnResGameStart);
            Handler.Add<ResClubGameOver>(MsgID.BroadcastGameOver, NotificationType.Network_OnBroadcastGameOver);
            Handler.Add<ResClubRoomUsers>(MsgID.ResClubRoomUsers, NotificationType.Network_OnResClubRoomUsers);
            Handler.Add<BroadcastSitClubRoom>(MsgID.BroadcastSitRoom, NotificationType.Network_OnBroadcastSitClubRoom);
            Handler.Add<BroadcastLeaveClubRoom>(MsgID.ResLeaveRoom, NotificationType.Network_OnBroadcastLeaveClubRoom);


            Handler.AddListenner(NotificationType.Network_OnResClub, OnResClub);
            Handler.AddListenner(NotificationType.Network_OnBroadcastJoinClub, OnBroadcastJoinClub);
            Handler.AddListenner(NotificationType.Network_OnBroadcastDelClub, OnBroadcastDelClub);
            Handler.AddListenner(NotificationType.Network_OnResCreateClubRoom, ResCreateClubRoom);
            Handler.AddListenner(NotificationType.Network_OnResClubRooms, OnResClubRooms);
            Handler.AddListenner(NotificationType.Network_OnResRoom, OnResRoom);
            Handler.AddListenner(NotificationType.Network_OnResGameStart, OnResGameStart);
            Handler.AddListenner(NotificationType.Network_OnBroadcastGameOver, OnBroadcastGameOver);
            Handler.AddListenner(NotificationType.Network_OnResClubRoomUsers, OnResClubRoomUsers);
            Handler.AddListenner(NotificationType.Network_OnBroadcastSitClubRoom, OnBroadcastSitClubRoom);
            Handler.AddListenner(NotificationType.Network_OnBroadcastLeaveClubRoom, OnBroadcastLeaveClubRoom);
        }

        private void OnBroadcastLeaveClubRoom(NotificationArg arg)
        {
            var data = arg.GetValue<BroadcastLeaveClubRoom>();

            if (data.code != 0)
            {
                MsgBox.ShowErr(data.msg);
                return;
            }

            updateDeskUser(data.tableId, data.deskId, null);
        }

        private void OnBroadcastSitClubRoom(NotificationArg arg)
        {
            var data = arg.GetValue<BroadcastSitClubRoom>();

            if (data.code != 0)
            {
                MsgBox.ShowErr(data.msg);
                return;
            }

            updateDeskUser(data.tableId, data.deskId, data.avatar,data.nick);
        }

        private void OnResClubRoomUsers(NotificationArg arg)
        {
            var data = arg.GetValue<ResClubRoomUsers>();

            if (data.code != 0)
            {
                MsgBox.ShowErr(data.msg);
                return;
            }
            if (data.users == null)
            {
                return;
            }
            foreach (var u in data.users)
            {
                Debug.Log(u.nick);
                updateDeskUser(data.tableId, u.deskId, u.avatar,u.nick);
            }
        }

        void updateDeskUser(int tableId, int deskId, string avatar, string nick="")
        {
            var table = Tables.GetChildAt(tableId - 1).asCom;
            var desk = table.GetChild("desk" + deskId);
            var nickText = table.GetChild("nick" + deskId);
            var avatarUi = desk.asCom.GetChild("avatar").asLoader;
            avatarUi.url = avatar == null ? avatarUi.data + "" : Config.HttpBaseHost + "/static" + avatar;
            if (avatar != null)
            {
                nickText.text = nick;
                nickText.visible = true;
                desk.onClick.Remove(onDeskClick);
            }
            else
            {
                nickText.visible = false;
                desk.onClick.Set(onDeskClick);
            }
        }

        private void OnBroadcastGameOver(NotificationArg arg)
        {
            var data = arg.GetValue<ResClubGameOver>();

            if (data.code != 0)
            {
                MsgBox.ShowErr(data.msg);
                return;
            }

            var table = Tables.GetChildAt(data.tableId - 1).asCom;
            var info = table.GetChild("info").asRichTextField;
            for (var i = 1; i <= 10; i++)
            {
                var desk = table.GetChild("desk" + i);
            }
            var room = Data.Club.Info;
            Dictionary<string, string> vars = new Dictionary<string, string>();
            vars["id"] = data.tableId + "";
            vars["current"] = "0";
            vars["count"] = room.count + "";
            vars["score"] = scores[room.score];
            vars["status"] = "等待中";
            info.templateVars = vars;
        }

        private void OnResGameStart(NotificationArg arg)
        {
            var data = arg.GetValue<ResClubGameStart>();

            if (data.code != 0)
            {
                MsgBox.ShowErr(data.msg);
                return;
            }
            if (data.clubId != Data.Club.Id)
            {
                Debug.LogWarning("收到不是当前茶楼的消息");
                return;
            }
            Api.Room.GetRoom(data.roomId);
        }

        private void OnResRoom(NotificationArg arg)
        {
            var data = arg.GetValue<ResRoom>();

            if (data.code != 0)
            {
                MsgBox.ShowErr(data.msg);
                return;
            }

            var room = data.room;

            var table = Tables.GetChildAt(room.tableId - 1).asCom;
            var info = table.GetChild("info").asRichTextField;
            for (var i = 1; i <= 10; i++)
            {
                var desk = table.GetChild("desk" + i);
                desk.data = room.tableId;
            }

            Dictionary<string, string> vars = new Dictionary<string, string>();
            vars["id"] = room.tableId + "";
            vars["current"] = room.current + "";
            vars["count"] = room.count + "";
            vars["score"] = scores[room.score];
            vars["status"] = room.status == 0 ? "等待中" : "游戏中";
            info.templateVars = vars;

            // 获取房间的玩家信息
            Api.Club.ClubRoomUsers(room.id);
        }

        private void OnResClubRooms(NotificationArg arg)
        {
            var data = arg.GetValue<ResClubRooms>();
            if (data.code != 0)
            {
                MsgBox.ShowErr(data.msg);
                return;
            }
            if (data.rooms == null) return;
            foreach (var room in data.rooms)
            {
                if (room.clubId != Data.Club.Id)
                {
                    Debug.Log("收到不是当前俱乐部的消息");
                    continue;
                }
                Api.Room.GetRoom(room.id);
            }
        }

        private void ResCreateClubRoom(NotificationArg arg)
        {
            var data = arg.GetValue<ResCreateClubRoom>();
            if (data.code != 0)
            {
                MsgBox.ShowErr(data.msg, 2);
                return;
            }

            if (data.clubId != Data.Club.Id)
            {
                Debug.Log("收到不是当前俱乐部的消息");
                return;
            }

            // 如果是自己创建的，就进入房间
            if (data.uid == Data.User.Id)
            {
                Data.Game.Id = data.roomId;
                SceneManager.LoadScene("Game");
                return;
            }

            // 如果是其他人创建的房间，则获取房间信息
            Api.Room.GetRoom(data.roomId);
        }

        private void OnBroadcastDelClub(NotificationArg arg)
        {
            var data = arg.GetValue<BroadcastDelClub>();
            if (data.code != 0)
            {
                MsgBox.ShowErr(data.msg);
                exit();
                return;
            }

            if (data.clubId != Data.Club.Id)
            {
                Debug.Log("收到不是当前俱乐部的消息，来自俱乐部id：" + data.clubId);
                return;
            }

            if (data.uid != Data.User.Id)
            {
                MsgBox.ShowErr("该茶楼被老板解散");
            }
            GRoot.inst.CloseAllWindows();
            exit();
        }

        private void OnBroadcastJoinClub(NotificationArg arg)
        {
            var data = arg.GetValue<BroadcastJoinClub>();
            if (data.code != 0)
            {
                MsgBox.ShowErr(data.msg);
                exit();
                return;
            }

            if (data.uid == Data.User.Id)
            {
                Api.Club.GetClub(Data.Club.Id);
                Api.Club.ClubRooms(Data.Club.Id);// 获取当前茶楼所有房间信息
            }
        }

        void exit()
        {
            Data.Club.Id = 0;
            SceneManager.LoadScene("Menu");
        }

        private void OnResClub(NotificationArg arg)
        {
            var data = arg.GetValue<ResClub>();
            if (data.code != 0)
            {
                MsgBox.ShowErr(data.msg, 2);
                exit();
                return;
            }

            Data.Club.Info = data.club;
            Data.Club.IsBoss = data.club.uid == Data.User.Id;

            var info = ui.GetChild("header").asCom.GetChild("info").asTextField;
            info.text = Data.Club.Id + "\n" + data.club.name;

            var roll = ui.GetChild("rollText").asCom;
            roll.GetChild("text").text = data.club.rollText;

            Tables.RemoveChildrenToPool();
            for (var i = 1; i <= 10; i++)
            {
                addItem(data.club, i);
            }
        }


        // Start is called before the first frame update
        void Start()
        {
            Api.Club.Join(Data.Club.Id);
        }

        private void Update()
        {
            Handler.HandleMessage();
        }




        private void addItem(ClubInfo club, int tableId)
        {
            GComponent table = Tables.AddItemFromPool().asCom;
            var info = table.GetChild("info").asRichTextField;
            for (var i = 1; i <= 10; i++)
            {
                var desk = table.GetChild("desk" + i);
                desk.data = tableId;
                desk.onClick.Set(onDeskClick);
            }

            Dictionary<string, string> vars = new Dictionary<string, string>();
            vars["id"] = tableId + "";
            vars["current"] = "0";
            vars["count"] = club.count + "";
            vars["score"] = scores[club.score];
            info.templateVars = vars;
            Tables.AddChild(table);
        }

        private void onDeskClick(EventContext context)
        {
            var desk = context.sender as GButton;
            var tableId = int.Parse(desk.data + "");
            Data.Club.TableId = tableId;
            Api.Club.CreateRoom(Data.Club.Id, tableId);
        }

        void OnDestroy()
        {
            Api.Club.Exit();
        }
    }

}