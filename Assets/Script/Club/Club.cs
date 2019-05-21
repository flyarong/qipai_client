using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using Api;

namespace Club
{
    public class Club : MonoBehaviour
    {
        Client client;
        private GComponent mainUI;
        GList Tables;
        private void Awake()
        {
            mainUI = GetComponent<UIPanel>().ui;
            Tables = mainUI.GetChild("tableList").asCom.GetChild("tables").asList;
            EventCenter.AddListener<string>(NoticeType.FreshClub, GetClub);
        }

        private void OnDestroy()
        {
            EventCenter.RemoveListener<string>(NoticeType.FreshClub, GetClub);
        }

        // Start is called before the first frame update
        void Start()
        {
            GetClub(Data.Club.Id);
            GetTableList(Data.Club.Id);
        }

        public  void GetClub(string clubId)
        {
            var club = new Api.Club();

            var j = club.GetClub(Data.Club.Id);

            if (j["code"].n != 0)
            {
                Utils.MsgBox.ShowErr(j["msg"].str);
                return;
            }

            Data.Club.Data = j["data"]["club"];
            // 是否是老板
            Data.Club.IsBoss = Data.Club.Data["uid"].n == Data.User.Info["id"].n;

            var info = mainUI.GetChild("info").asTextField;
            info.text = Data.Club.Id + "\n" + Data.Club.Data["name"].str;


            //var roll = ui.GetChild("rollText").asCom;
            //roll.GetChild("text").text = clubData["roll_text"].str;

            PlayerPrefs.SetString("RollText", Data.Club.Data["roll_text"].str);
        }

        public  void GetTableList(string clubId)
        {
            Tables.RemoveChildren();
            for (var i = 0; i < 10; i++)
            {
                addItem(null);
            }
        }

        private  void addItem(JSONObject obj)
        {
            GComponent table = Tables.GetFromPool("ui://1ad63yxfhon0bo").asCom;
            var info = table.GetChild("info").asRichTextField;
            Dictionary<string, string> vars = new Dictionary<string, string>();
            vars["id"] = (2 + 1) + "";
            vars["now"] = (1 + 1) + "";
            vars["max"] = 10 + "";
            vars["score"] = "10/20";
            info.templateVars = vars;
            Tables.AddChild(table);
        }
    }

}