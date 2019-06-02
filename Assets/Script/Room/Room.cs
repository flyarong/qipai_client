using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using FairyGUI;
using UnityEngine.SceneManagement;

namespace Room
{
    public class Room : MonoBehaviour
    {
        GComponent ui;
        GButton btnStart;
        RightWindow right;
        GButton btnScore1, btnScore2;
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
            Data.Room.Players.Clear();
            ui = GetComponent<UIPanel>().ui;
            right = new RightWindow();
            ui.GetChild("btnSetting").onClick.Add(onSettingClick);
            btnStart = ui.GetChild("btnStart").asButton;
            btnScore1 = ui.GetChild("btnScore1").asButton;
            btnScore2 = ui.GetChild("btnScore2").asButton;

            EventCenter.AddListener<string>(NoticeType.GameBegin, GameBegin);
            EventCenter.AddListener<string, string>(NoticeType.PlayerSitDown, RoomJoin);
            EventCenter.AddListener<string, string>(NoticeType.RoomExit, ExitRoom);
        }

        
        private void OnDestroy()
        {
            EventCenter.RemoveListener<string>(NoticeType.GameBegin, GameBegin);
            EventCenter.RemoveListener<string, string>(NoticeType.RoomExit, ExitRoom);
            EventCenter.RemoveListener<string, string>(NoticeType.PlayerSitDown, RoomJoin);
        }

        void GameBegin(string roomId)
        {
            updataInfo();
        }

        void ExitRoom(string roomId, string uid)
        {
            Debug.Log(uid + " 退出 " + roomId);

            if (uid == Data.User.Id+"")
            {
                SceneManager.LoadScene("Menu");
                return;
            }

            if(roomId != Data.Room.Id)
            {
                return;
            }
            var p = GetPlayer(int.Parse(uid));
            if (p == null)
            {
                Utils.MsgBox.ShowErr("获取玩家失败");
                return;
            }

            p.PlayerUi.visible = false;
            Data.Room.Players.Remove(p);
            
        }

        void RoomJoin(string roomId, string uid)
        {
            if (roomId != Data.Room.Id)
            {
                Debug.Log("不是本房间的消息");
                return;
            }
            var j = new Api.Room().Player(Data.Room.Id,uid);
            if (j == null)
            {
                Utils.MsgBox.ShowErr("网络出现状况");
                return;
            }
            else if (j["code"].n != 0)
            {
                Utils.MsgBox.ShowErr(j["msg"].str);
                return;
            }
            var p = j["data"]["player"];
            GetPlayer((int)p["desk_id"].n, (int)p["uid"].n);
        }

        private void Start()
        {
            initRoom();
            var j = new Api.Room().Player(Data.Room.Id);
            if(j==null )
            {
                Utils.MsgBox.ShowErr("网络出现状况");
                return;
            }
            else if(j["code"].n != 0)
            {
                Utils.MsgBox.ShowErr(j["msg"].str);
                return;
            }
            var p = j["data"]["player"];
            Data.Room.DeskId = (int)p["desk_id"].n;

            initPlayers();
        }

        Game.Player GetPlayer(int uid)
        {
            foreach (var p in Data.Room.Players)
            {
                // 只返回可见的，隐藏的都是离开的
                if (p.UserInfo["id"].n == uid)
                {
                    return p;
                }
            }
            return null;
        }

        Game.Player GetPlayer(int deskId, int uid)
        {

            Game.Player player = new Game.Player();
            player.DeskId = deskId;
            player.Index = player.DeskId - Data.Room.DeskId;
            if (player.Index < 0)
            {
                player.Index = 10 + player.Index;
            }

            player.PlayerUi = ui.GetChild("player" + (player.Index + 1)).asCom;
            var user = new Api.User().GetUserInfo(uid+"");
            if (user["code"].n != 0)
            {
                Utils.MsgBox.ShowErr(user["msg"].str);
                return null;
            }
            player.PlayerUi.visible = true;
            player.UserInfo = user["data"]["user"];
            player.PlayerUi.GetChild("name").text = player.UserInfo["nick"].str;
            player.PlayerUi.GetChild("avatar").asLoader.url = "/static" + player.UserInfo["avatar"].str;
            
            Data.Room.Players.Add(player);

            return player;
        }

        void onSettingClick()
        {
            GRoot.inst.ShowPopup(right);
        }

        void initRoom()
        {
            updataInfo();

            // 如果是房主，显示开始按钮
            if (Data.Room.Info["uid"].n == Data.User.Id)
            {
                btnStart.visible = true;
            }

            // 下注按钮积分初始化
            var s = scores[(int)Data.Room.Info["score"].n];
            var ss = s.Split('/');
            btnScore1.title = ss[0];
            btnScore2.title = ss[1];
        }


        
        void updataInfo()
        {
            var j = new Api.Room().GetRoom(Data.Room.Id);
            if (j["code"].n != 0)
            {
                Utils.MsgBox.ShowErr(j["msg"].str);
                return;
            }

            Data.Room.Info = j["data"]["room"];
            Debug.Log(Data.Room.Info);
            var room = Data.Room.Info;

            var text = "";
            if (Data.Club.Id == "")
            {
                text += "房号：" + room["id"];
            }
            else
            {
                text += "茶楼：" + Data.Club.Id + "  第" + room["id"] + "桌";
            }

            text += "\n";
            text += "底分：" + scores[(int)room["score"].n];
            text += "\n";
            text += "局数：" + room["current"].n + "/" + room["count"].n;

            ui.GetChild("infoText").text = text;
        }

        void initPlayers()
        {
            var j = new Api.Room().Players(Data.Room.Id);
            if (j["code"].n != 0)
            {
                Utils.MsgBox.ShowErr(j["msg"].str);
                return;
            }

            var players = j["data"]["players"].list;
            
            foreach (var p in players)
            {
                GetPlayer((int)p["desk_id"].n, (int)p["uid"].n);
            }
        }

       
    }

}