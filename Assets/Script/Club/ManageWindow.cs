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
        Client client;
        public ManageWindow()
        {

        }

        

        protected override void OnInit()
        {
            this.contentPane = UIPackage.CreateObject("qipai", "ClubManageWindow").asCom;
            this.Center();
            this.modal = true;
            this.SetSize(GRoot.inst.width, GRoot.inst.height);

            setting = this.contentPane.GetChild("setting").asCom;
            setting.GetChild("btnSave").asButton.onClick.Add(onBtnSaveClick);
            client = new Client(PlayerPrefs.GetString("token"));

            var check = setting.GetChild("check").asButton.GetController("button");
            var close = setting.GetChild("close").asButton.GetController("button");
            var name = setting.GetChild("name").asTextInput;
            var rollText = setting.GetChild("rollText").asTextInput;
            var notice = setting.GetChild("notice").asTextInput;

            var d = Data.Club.Data;
            check.SetSelectedIndex(d["check"].b ? 1 : 0);
            close.SetSelectedIndex(d["close"].b ? 1 : 0);
            name.text = d["name"].str;
            rollText.text = d["roll_text"].str;
            notice.text = d["notice"].str;
        }

        void onBtnSaveClick()
        {
            var check = setting.GetChild("check").asButton.GetController("button");
            var close = setting.GetChild("close").asButton.GetController("button");
            var name = setting.GetChild("name").asTextInput;
            var rollText = setting.GetChild("rollText").asTextInput;
            var notice = setting.GetChild("notice").asTextInput;

            var j = new Api.Club()
                .EditClubInfo(Data.Club.Id,
                check.selectedIndex == 0,
                close.selectedIndex == 0,
                name.text, rollText.text, notice.text);
            if (j["code"].n != 0)
            {

            }
            EventCenter.Broadcast<string>(NoticeType.FreshClub, Data.Club.Id);
            this.Hide();
        }
    }

}