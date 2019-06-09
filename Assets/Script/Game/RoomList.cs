using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FairyGUI;
using UnityEngine.SceneManagement;
using Network.Msg;
using Notification;
using System;
using Utils;

namespace Game
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

            //EventCenter.AddListener(NoticeType.RoomList, RenderList);
            //EventCenter.AddListener<string>(NoticeType.RoomDelete, RoomDelete);
        }
        private void Start()
        {

            Handler.Add<ResRoomList>(MsgID.ResRoomList, NotificationType.Network_OnResRoomList);

            Handler.AddListenner(NotificationType.Network_OnResRoomList, OnResRoomList);

            Api.Room.RoomList();
        }

        private void OnResRoomList(NotificationArg arg)
        {
            var data = arg.GetValue<ResRoomList>();
            if (data.code != 0)
            {
                MsgBox.ShowErr(data.msg, 2);
                return;
            }

            foreach(var room in data.rooms)
            {
                addRoomItem(room);
            }
        }

        private void OnDestroy()
        {
           
        }

        void addRoomItem(Network.Msg.Room room)
        {
            GComponent item = list.GetFromPool("ui://1ad63yxfbul8a7").asCom;

            item.GetChild("id").text = room.id + "";
            item.GetChild("score").text = scores[room.score];
            item.GetChild("pay").text = room.pay == 0 ? "老板" : "AA";
            item.GetChild("count").text = room.current + "/" + room.count + "";
            item.GetChild("players").text =  room.players + "";
            item.GetChild("btnInvite").asButton.onClick.Add(onInviteClick);

            list.AddChild(item);
        }

        void removeRoomItem()
        {

        }

        //void RoomDelete(string roomId)
        //{
        //    RenderList();
        //}

        void RenderList()
        {
            //Debug.Log("更新房间列表");
            //var j = new Api.Room().List();
            //if (j["code"].n != 0)
            //{
            //    Utils.MsgBox.ShowErr("更新窗口失败，请联系管理");
            //    return;
            //}
            //list.RemoveChildrenToPool();

            //if (j["data"]["rooms"] == null || j["data"]["rooms"].list == null)
            //{
            //    return;
            //}

            //foreach (var v in j["data"]["rooms"].list)
            //{
            //    addClubItem(v);
            //}
        }

        void onClickItem(EventContext context)
        {
            var item = (GComponent)context.data;
            var id = item.GetChild("id").text;
            var roomId = id;
            Data.Room.Id = int.Parse(roomId);
            SceneManager.LoadScene("Game");
        }

        void onInviteClick(EventContext context)
        {
            context.StopPropagation();
            var btn = context.sender as GButton;
            GUIUtility.systemCopyBuffer = btn.parent.GetChild("id").asTextField.text;
        }
    }
}