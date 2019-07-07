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
        AudioSource roomAudio;
        GComponent tips;
        GComponent roomRule;

        private string[] scores = {
            "1/2",
            "2/4",
            "3/6",
            "4/8",
            "5/10",
            "10/20"
        };
        private string[] rules = {
            "牛一→牛牛 分别对应 1→10倍",
            "牛牛x5 牛九x4 牛八x3 牛七x2",
            "牛牛x3 牛九x2 牛八x2 牛七x2",
            "牛牛x3 牛九x2 牛八x2 牛七x1",
            "牛牛x4 牛九x2 牛八x2 牛七x2"
        };
        private void Awake()
        {
            bindEvents();
            GameObject audioObj = new GameObject("roomAudio");
            roomAudio = audioObj.AddComponent<AudioSource>();
            //Data.Room.Players.Clear();
            ui = GetComponent<UIPanel>().ui;
            tips = ui.GetChild("tips").asCom;
            tips.sortingOrder = 1000;
            right = new RightWindow();
            ui.GetChild("btnSetting").onClick.Add(onSettingClick);
            btnStart = ui.GetChild("btnStart").asButton;
            ui.GetChild("btnCopy").onClick.Add(() =>
            {
                var content = "房间号:" + Data.Game.Id;
                if (Data.Club.Id > 0)
                {
                    content = "茶楼编号:" + Data.Club.Id + "  第" + Data.Club.TableId + "桌";
                }
                UnityEngine.GUIUtility.systemCopyBuffer = content;
                Utils.MsgBox.ShowErr("房间号已复制到剪贴板，在聊天输入框长按粘贴即可");
            });

            ui.GetChild("btnRefresh").onClick.Add(() =>
            {
                Utils.ConfirmWindow.ShowBox(() =>
                {
                    Api.Room.JoinRoom(Data.Game.Id);
                }, "当游戏界面卡主的时候，点此刷新游戏。\n如果多次刷新还是无效，请尝试退出游戏重进。");
            });

            roomRule = UIPackage.CreateObject("qipai", "roomRule").asCom;
            var ruleText = roomRule.GetChild("ruleText").asRichTextField;
            var btnRule = ui.GetChild("btnRule");
            btnRule.onClick.Add(() =>
            {
                var info = Data.Game.info;
                Dictionary<string, string> vars = new Dictionary<string, string>();
                vars.Add("players", info.players+"");
                vars.Add("score", scores[info.score]);
                vars.Add("pay", info.pay == 0 ? "老板付款" : "AA付款");
                vars.Add("count", info.count+"");
                vars.Add("rule",rules[info.times]);
                ruleText.templateVars = vars;
                GRoot.inst.ShowPopup(roomRule);
                roomRule.Center();
            });
            
        }

        private void bindEvents()
        {
            Handler.Init();

            Handler.Add<ResRoom>(MsgID.ResRoom, NotificationType.Network_OnResRoom);
            Handler.Add<ResJoinRoom>(MsgID.ResJoinRoom, NotificationType.Network_OnResJoinRoom);
            Handler.Add<ResSit>(MsgID.ResSit, NotificationType.Network_OnResSit);
            Handler.Add<ResLeaveRoom>(MsgID.ResLeaveRoom, NotificationType.Network_OnResLeaveRoom);
            Handler.Add<ResUserInfo>(MsgID.ResUserInfo, NotificationType.Network_OnResUserInfo);
            Handler.Add<BroadcastSitRoom>(MsgID.BroadcastSitRoom, NotificationType.Network_OnBroadcastSitRoom);
            Handler.Add<ResDeleteRoom>(MsgID.ResDeleteRoom, NotificationType.Network_OnResDeleteRoom);


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
            if (data.roomId != Data.Game.Id)
            {
                Debug.LogWarning("收到不属于该房间的消息：ResDeleteRoom");
                return;
            }
            MsgBox.ShowErr("房间已解散", 1);
            Data.Game.Id = 0;
            LeaveRoom();
        }


        private void OnBroadcastSitRoom(NotificationArg arg)
        {
            var data = arg.GetValue<BroadcastSitRoom>();

            if (data.code != 0)
            {
                MsgBox.ShowErr(data.msg);
                return;
            }
            if (data.roomId != Data.Game.Id)
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

            var p = Data.Game.GetPlayer(data.user.id);
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
            p.PlayerUi.GetChild("avatar").asLoader.url = Utils.Helper.GetReallyImagePath(data.user.avatar);
            p.PlayerUi.visible = true;

        }
        void LeaveRoom()
        {
            // 如果clubId>0表示是从俱乐部进入房间的，直接退回俱乐部
            if (Data.Club.Id > 0)
            {
                SceneManager.LoadScene("Club");
                return;
            }

            SceneManager.LoadScene("Menu");
        }
        private void OnResLeaveRoom(NotificationArg arg)
        {
            var data = arg.GetValue<ResLeaveRoom>();

            if (data.code != 0 && data.msg != "该房间不存在")
            {
                MsgBox.ShowErr(data.msg);
                return;
            }
            // 如果是自己退出，就返回菜单页
            if (data.uid == Data.User.Id)
            {
                Data.Game.Id = 0;
                Data.Game.info = null;

                LeaveRoom();
                return;
            }

            // 其他用户退出
            var player = Data.Game.GetPlayer(data.uid);
            if (player == null)
            {
                Debug.LogWarning("用户退出失败：" + data.uid);
                return;
            }
            player.PlayerUi.visible = false;
            Data.Game.Players.Remove(data.uid);

            // 如果我是新房主，就显示开始按钮
            if (data.newBoss == Data.User.Id)
            {
                btnStart.visible = true;
            }
        }

        /// <summary>
        /// 请求退出房间
        /// </summary>
        private void exit()
        {
            Api.Room.Leave(Data.Game.Id);
        }

        private void OnResSit(NotificationArg arg)
        {
            var data = arg.GetValue<ResSit>();

            if (data.code != 0)
            {
                MsgBox.ShowErr(data.msg);
                if (data.msg == "您当前正在其他房间")
                {
                    Data.Game.Id = data.roomId;
                    Data.Game.info = null;
                    LeaveRoom();
                    return;
                }
                exit();
                return;
            }

            Data.Game.DeskId = data.deskId;
            addPlayers(data.players);



            // 如果是首位开始类型的房间，自己也是第一个来的,就显示开始按钮
            if (Data.Game.info.startType == 1 && data.players.Count == 1 && Data.Game.info.current == 0)
            {
                btnStart.visible = true;
            }
            else if (Data.Game.info.startType == 0 && data.uid == Data.Game.info.uid && Data.Game.info.current == 0) // 老板开始游戏并且自己是老板，就显示开始按钮
            {
                btnStart.visible = true;
            }
            else if(Data.Game.info.status==0)
            {
                ShowTips("您已准备好，等待房主开始游戏···");
            }
        }

        // 添加玩家
        void addPlayers(List<PlayerInfo> players)
        {
            Data.Game.RemoveAllPlayers();
            foreach (var p in players)
            {
                Data.Game.TotalScore.Set(Data.Game.Id, p.uid, p.totalScore);
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
                Data.Game.Id = 0;
                Data.Game.info = null;
                LeaveRoom();

                return;
            }

            Api.Room.GetRoom(Data.Game.Id);
            Api.Room.Sit(Data.Game.Id);

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
            player.Index = player.DeskId - Data.Game.DeskId;
            if (player.Index < 0)
            {
                player.Index = 10 + player.Index;
            }

            player.PlayerUi = ui.GetChild("player" + (player.Index + 1)).asCom;
            player.PlayerUi.sortingOrder = 2000;
            if (!Data.Game.Players.ContainsKey(uid))
            {
                Data.Game.Players.Add(uid, player);

                // 自己坐下，不用播放声音
                if (uid != Data.User.Id)
                {
                    roomAudio.clip = Resources.Load<AudioClip>("Game/audio/game_sit");
                    roomAudio.Play();
                }

            }

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

            Data.Game.info = room;


            var text = "";
            if (Data.Club.Id > 0)
            {
                text += "茶楼：" + Data.Club.Id + "\n";
                text += "桌号：" + "第" + Data.Club.TableId + "桌\n";
            }
            else
            {
                text += "房号：" + room.id + "\n";
            }


            text += "底分：" + scores[room.score] + "\n";
            text += "局数：" + room.current + "/" + room.count;

            ui.GetChild("infoText").text = text;


            if (Data.Game.GetPlayer(Data.User.Id) == null)
            {
                Api.Room.Sit(Data.Game.Id);
            }

        }

        private void Start()
        {
            Api.Room.JoinRoom(Data.Game.Id);
        }

        void onSettingClick()
        {
            GRoot.inst.ShowPopup(right);
        }

        private void Update()
        {
            Handler.HandleMessage();
        }


        void ShowTips(string text)
        {
            tips.visible = true;
            tips.GetChild("text").text = text;
        }

        void HideTips()
        {
            this.CancelInvoke();
            tips.visible = false;
        }

    }

}