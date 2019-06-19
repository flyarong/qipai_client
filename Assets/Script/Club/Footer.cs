using UnityEngine;
using System.Collections;
using FairyGUI;
using System;
using Utils;
using Network.Msg;
using Notification;

namespace Club
{
    public class Footer : MonoBehaviour
    {
        GComponent mainUI;
        ManageWindow manageWindow;
        UsersWindow usersWindow;
        GButton btnManage, btnLeave;
        // Use this for initialization
        void Start()
        {
            bindEvents();

            mainUI = GetComponent<UIPanel>().ui;
            manageWindow = new ManageWindow();
            manageWindow.name = "manageWindow";
            usersWindow = new UsersWindow();

            var footer = mainUI.GetChild("footer").asCom;
            btnManage = footer.GetChild("btnManage").asButton;
            btnManage.onClick.Add(onBtnManageClick);
            footer.GetChild("btnUsers").asButton.onClick.Add(onBtnUsersClick);
        }
        
        private void bindEvents()
        {
            Handler.Add<BroadcastEditClub>(MsgID.BroadcastEditClub, NotificationType.Network_OnBroadcastEditClub);
            Handler.Add<ResClubUsers>(MsgID.ResClubUsers, NotificationType.Network_OnResClubUsers);


            Handler.AddListenner(NotificationType.Network_OnBroadcastEditClub, OnBroadcastEditClub);
            Handler.AddListenner(NotificationType.Network_OnResClubUsers, OnResClubUsers);
        }

        private void OnResClubUsers(NotificationArg arg)
        {
            var data = arg.GetValue<ResClubUsers>();
            if (data.code != 0)
            {
                MsgBox.ShowErr(data.msg);
                return;
            }
            Data.Club.Users = data.users;
            usersWindow.Show();
            usersWindow.width = mainUI.width;
            usersWindow.height = mainUI.height;

        }

        private void OnBroadcastEditClub(NotificationArg arg)
        {
            var data = arg.GetValue<BroadcastEditClub>();
            if (data.code != 0)
            {
                MsgBox.ShowErr(data.msg, 2);
                return;
            }

            Api.Club.GetClub(data.clubId);
            manageWindow.Hide();
        }


        void onBtnManageClick()
        {
            manageWindow.width = mainUI.width;
            manageWindow.height = mainUI.height;
            manageWindow.Show();
        }

        void onBtnUsersClick()
        {
            Api.Club.ClubUsers(Data.Club.Id);
        }
        // Update is called once per frame
        void Update()
        {

        }
    }

}