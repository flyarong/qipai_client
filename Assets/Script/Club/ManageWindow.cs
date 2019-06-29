using UnityEngine;
using System.Collections;
using FairyGUI;
using Api;
using System.Collections.Generic;
using System;

namespace Club
{

    public class ManageWindow : Window
    {
        GComponent setting;
        public ManageWindow()
        {

        }



        protected override void OnInit()
        {
            this.contentPane = UIPackage.CreateObject("qipai", "ClubManageWindow").asCom;
            this.Center();
            this.modal = true;


        }

        protected override void OnShown()
        {
            setting = this.contentPane.GetChild("setting").asCom;
            var btnSave = setting.GetChild("btnSave").asButton;
            var btnLeave = setting.GetChild("btnLeave").asButton; // 离开茶楼
            var btnDelete = setting.GetChild("btnDelete").asButton; // 解散茶楼
            btnSave.onClick.Add(onBtnSaveClick);
            btnDelete.onClick.Add(onBtnDeleteClick);
            btnLeave.onClick.Add(onBtnLeaveClick);

            if (Data.Club.IsBoss)
            {
                btnDelete.visible = true;
            }
            else
            {
                btnLeave.visible = true;
            }

            var check = setting.GetChild("check").asButton.GetController("button");
            var close = setting.GetChild("close").asButton.GetController("button");
            var name = setting.GetChild("name").asTextInput;
            var rollText = setting.GetChild("rollText").asTextInput;
            var notice = setting.GetChild("notice").asTextInput;

            var d = Data.Club.Info;
            check.SetSelectedIndex(d.check ? 1 : 0);
            close.SetSelectedIndex(d.close ? 1 : 0);
            name.text = d.name;
            rollText.text = d.rollText;
            notice.text = d.notice;
        }

        private void onBtnLeaveClick(EventContext context)
        {
            Utils.ConfirmWindow.ShowBox(() => {
                Api.Club.Exit(Data.Club.Id);
                Hide();
            }, "确定退出该茶楼吗？");
        }

        private void onBtnDeleteClick(EventContext context)
        {
            Utils.ConfirmWindow.ShowBox(() => {
                Api.Club.Delete(Data.Club.Id);
            }, "确定解散该茶楼吗？");
        }
        

        void onBtnSaveClick()
        {
            var check = setting.GetChild("check").asButton.GetController("button");
            var close = setting.GetChild("close").asButton.GetController("button");
            var name = setting.GetChild("name").asTextInput;
            var rollText = setting.GetChild("rollText").asTextInput;
            var notice = setting.GetChild("notice").asTextInput;

            Api.Club
                .EditClubInfo(Data.Club.Id,
                check.selectedIndex == 1,
                close.selectedIndex == 1,
                name.text, rollText.text, notice.text);
        }
    }

}