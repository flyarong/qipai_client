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
        private void Awake()
        {
            bindEvents();
            ui = GetComponent<UIPanel>().ui;
            Tables = ui.GetChild("tableList").asCom.GetChild("tables").asList;
            var btnQuit = ui.GetChild("btnQuit").asButton;
            btnQuit.onClick.Add(onBtnQuit);
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
            
            
            Handler.AddListenner(NotificationType.Network_OnResClub, OnResClub);
            Handler.AddListenner(NotificationType.Network_OnBroadcastJoinClub, OnBroadcastJoinClub);
        }

        private void OnBroadcastJoinClub(NotificationArg arg)
        {
            var data = arg.GetValue<BroadcastJoinClub>();
            if (data.code != 0)
            {
                MsgBox.ShowErr(data.msg, 2);
                exit();
                return;
            }

            if (data.uid == Data.User.Id)
            {
                Api.Club.GetClub(Data.Club.Id);
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

            var info = ui.GetChild("info").asTextField;
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

        string[] scores = {
            "1/2",
            "2/4",
            "3/6",
            "4/8",
            "5/10",
            "10/20"
        };


        private void addItem(ClubInfo club, int tableId)
        {
            //GComponent table = Tables.GetFromPool("ui://1ad63yxfhon0bo").asCom;
            GComponent table = Tables.AddItemFromPool().asCom;
            var info = table.GetChild("info").asRichTextField;
            Dictionary<string, string> vars = new Dictionary<string, string>();
            vars["id"] = tableId + "";
            vars["max"] = club.count + "";
            vars["score"] = scores[club.score];
            info.templateVars = vars;
            Tables.AddChild(table);
        }
    }

}