using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FairyGUI;
using UnityEngine.SceneManagement;
using Utils;
using Notification;
using Network.Msg;

namespace Club
{
    public class ClubList : MonoBehaviour
    {
        // 菜单页面俱乐部列表
        private GList list;
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
            var mainUI = GetComponent<UIPanel>().ui;
            list = mainUI.GetChild("list").asCom.GetChild("clubs").asCom.GetChild("list").asList;
            list.onClickItem.Add(onClickItem);
           
        }

        private void Start()
        {

            Handler.Add<ResClubList>(MsgID.ResClubs, NotificationType.Network_OnResClubs);

            Handler.AddListenner(NotificationType.Network_OnResClubs, OnResClubList);

            Api.Club.Clubs();
        }

        private void OnResClubList(NotificationArg arg)
        {
            var data = arg.GetValue<ResClubList>();
            if (data.code != 0)
            {
                MsgBox.ShowErr(data.msg, 2);
                return;
            }
            if (data.clubs == null)
            {
                return;
            }
            foreach (var club in data.clubs)
            {
                addClubItem(club);
            }
        }

        private void addClubItem(Network.Msg.Club club)
        {
            GComponent item = list.AddItemFromPool().asCom;

            item.GetChild("id").asTextField.text = club.id+"";
            item.GetChild("score").asTextField.text = scores[club.score];
            item.GetChild("pay").asTextField.text = club.pay == 0 ? "老板" : "AA";
            item.GetChild("count").asTextField.text = club.count + "";
            item.GetChild("boss").asTextField.text = club.boss;
            list.AddChild(item);
        }

        private void onClickItem(EventContext context)
        {
            var item = (GComponent)context.data;
            var id = item.GetChild("id").asTextField.text;
            Debug.Log("点击:" + id);
            Data.Club.Id = int.Parse(id);
            SceneManager.LoadScene("Club");
        }

    }
}