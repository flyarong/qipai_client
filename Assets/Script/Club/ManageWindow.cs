using UnityEngine;
using System.Collections;
using FairyGUI;
using Api;
using System.Collections.Generic;

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
            setting.GetChild("btnSave").asButton.onClick.Add(onBtnSaveClick);

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