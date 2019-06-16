﻿using UnityEngine;
using System.Collections;
using FairyGUI;
using Api;
using System.Collections.Generic;
using Data;
using Network.Msg;

namespace Club
{

    public class UsersWindow : Window
    {
        GComponent setting;
        GList userList;

        public UsersWindow()
        {

        }

        protected override void OnInit()
        {
            this.contentPane = UIPackage.CreateObject("qipai", "ClubUserWindow").asCom;
            this.Center();
            this.modal = true;

            userList = this.contentPane.GetChild("userList").asList;

        }

        protected override void OnShown()
        {
            userList.RemoveChildrenToPool();

            foreach (var user in Data.Club.Users)
            {
                if(user.id == Data.User.Id)
                {
                    Data.Club.IsAdmin = user.admin;
                    Data.Club.IsBoss = Data.User.Id == Data.Club.Info.uid;
                }
                addUserItem(user);
            }
        }

        void addUserItem(ClubUser user)
        {
            GComponent item = userList.AddItemFromPool().asCom;
            item.GetChild("imgAvatar").asLoader.url = "/static" + user.avatar;
            item.GetChild("textId").text = "ID:" + user.id;
            item.GetChild("textNick").text = "昵称:" + user.nick;

            var btnPayer = item.GetChild("btnPayer").asButton;
            var btnAdmin = item.GetChild("btnAdmin").asButton;
            var btnCheck = item.GetChild("btnCheck").asButton;
            var btnDisable = item.GetChild("btnDisable").asButton;


            // 是管理员或者老板  
            // 并且 当前列表项不是老板
            // 才显示控制按钮
            var b = (Data.Club.IsAdmin || Data.Club.IsBoss)
                && Data.Club.Info.uid != user.id;

            // 管理员 只有 老板可以控制
            if (user.admin)
            {
                if (Data.Club.IsBoss)
                {
                    b = true;
                }
                else
                {
                    b = false;
                }

            }


            btnPayer.visible = b;
            btnAdmin.visible = b;
            btnCheck.visible = b;
            btnDisable.visible = b;


            //btnPayer.selected = user["id"].n == Data.Club.Data["payer_uid"].n;
            btnAdmin.selected = user.admin;
            btnCheck.selected = user.status != 0;
            btnDisable.selected = user.status == 2;

            btnPayer.onClick.Add(onBtnPayerClick);
            btnAdmin.onClick.Add(onBtnAdminClick);
            btnCheck.onClick.Add(onBtnCheckClick);
            btnDisable.onClick.Add(onBtnDisableClick);


            userList.AddChild(item);
        }

        void onBtnPayerClick(EventContext context)
        {
            var btn = context.sender as GButton;
            // 点击会切换选中状态，恢复原来的状态，再做判断
            btn.selected = !btn.selected;
            doAction(btn, "pay");
        }

        void onBtnAdminClick(EventContext context)
        {
            var btn = context.sender as GButton;
            // 点击会切换选中状态，恢复原来的状态，再做判断
            btn.selected = !btn.selected;
            doAction(btn, "admin");
        }

        void onBtnCheckClick(EventContext context)
        {
            var btn = context.sender as GButton;
            // 点击会切换选中状态，恢复原来的状态，再做判断
            btn.selected = !btn.selected;

            if (btn.selected)
            {
                Utils.ConfirmWindow.ShowBox(() =>
                {
                    doAction(btn, "add");
                });
            }
            else
            {
                doAction(btn, "add");
            }
        }

        void onBtnDisableClick(EventContext context)
        {
            var btn = context.sender as GButton;
            // 点击会切换选中状态，恢复原来的状态，再做判断
            btn.selected = !btn.selected;
            doAction(btn, "disable");
        }

        // action 编辑会员状态：设为管理(admin) 取消管理(_admin)  冻结(disable) 取消冻结(_disable) 设为代付(pay) 取消代付(_pay) 审核通过用户(add)  移除用户(_add)
        void doAction(GButton btn, string action)
        {
            var item = btn.parent;
            var uid = item.GetChild("textId").text.Substring(3);


            // 如果已经是代付，就执行取消代付
            if (btn.selected)
            {
                action = "_" + action;
            }

            //var j = new Api.Club().ChangeMemberStatus(int.Parse(uid), action);

            //if (j["code"].n != 0)
            //{
            //    Utils.MsgBox.ShowErr(j["msg"].str, 2);
            //    btn.selected = false;
            //}

            // 更新俱乐部信息
            //EventCenter.Broadcast<string>(NoticeType.FreshClub, Data.Club.Id);
            // 更新用户列表
        }
    }
}