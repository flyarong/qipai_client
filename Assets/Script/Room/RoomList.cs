using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FairyGUI;
using UnityEngine.SceneManagement;

namespace Room
{
    public class RoomList : MonoBehaviour
    {
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
            list = mainUI.GetChild("list").asCom.GetChild("rooms").asCom.GetChild("list").asList;
            list.onClickItem.Add(onClickItem);

            EventCenter.AddListener(NoticeType.RoomList, RenderList);
            EventCenter.AddListener<string>(NoticeType.RoomDelete, RoomDelete);
        }
        private void Start()
        {


            EventCenter.Broadcast(NoticeType.RoomList);
        }

        private void OnDestroy()
        {
            EventCenter.RemoveListener(NoticeType.RoomList, RenderList);
            EventCenter.RemoveListener<string>(NoticeType.RoomDelete, RoomDelete);
        }

        void addClubItem(JSONObject club)
        {
            GComponent item = list.GetFromPool("ui://1ad63yxfbul8a7").asCom;

            item.GetChild("id").text = club["id"].n + "";
            item.GetChild("score").text = scores[(int)club["score"].n];
            item.GetChild("pay").text = club["pay"].n == 0 ? "老板" : "AA";
            item.GetChild("count").text = club["current"].n + "/" + club["count"].n + "";
            item.GetChild("players").text = club["players"].n + "";
            item.GetChild("btnInvite").onClick.Add(onInviteClick);
            list.AddChild(item);
        }

        void RoomDelete(string roomId)
        {
            RenderList();
        }

        void RenderList()
        {
            Debug.Log("更新房间列表");
            var j = new Api.Room().List();
            if (j["code"].n != 0)
            {
                Utils.MsgBox.ShowErr("更新窗口失败，请联系管理");
                return;
            }
            list.RemoveChildrenToPool();

            if (j["data"]["rooms"] == null || j["data"]["rooms"].list == null)
            {
                return;
            }

            foreach (var v in j["data"]["rooms"].list)
            {
                addClubItem(v);
            }
        }

        void onClickItem(EventContext context)
        {
            var item = (GComponent)context.data;
            var id = item.GetChild("id").text;
            var roomId = id;
            var j = new Api.Room().Join(roomId);

            if (j["code"].n != 0)
            {
                Utils.MsgBox.ShowErr(j["msg"].str);
                return;
            }

            j = new Api.Room().SitDown(roomId);
            if (j["code"].n != 0)
            {
                Utils.MsgBox.ShowErr(j["msg"].str);
                return;
            }
            Data.Room.Id = roomId;
        }

        void onInviteClick(EventContext context)
        {
            context.StopPropagation();
            var btn = context.sender as GButton;
            GUIUtility.systemCopyBuffer = btn.parent.GetChild("id").asTextField.text;
        }
    }
}