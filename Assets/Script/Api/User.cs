﻿using UnityEngine;
using System.Collections;
using Network.Msg;
using Network;
using System;

namespace Api
{
    public class User
    {
        private static Manager Inst = Manager.Inst;
        public static void Login(LoginType type, string name, string pass)
        {
            new Utils.Msg(MsgID.ReqLogin).Add("type", type).Add("name", name).Add("pass", pass).Send();
        }

        public static void LoginByToken()
        {
            if (Data.User.Token == "")
            {
                return;
            }
            new Utils.Msg(MsgID.ReqLoginByToken).Add("token", Data.User.Token).Send();
        }

        public static void Reg(LoginType type, string nick, string name, string pass, string code)
        {
            var msg = new ReqReg(type, nick, name, pass, code);
            Inst.SendMessage(msg.ToMessage());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="phone"></param>
        public static void GetCode(string phone)
        {
            var msg = new ReqCode(phone);
            Inst.SendMessage(msg.ToMessage());
        }

        public static void GetResetCode(string phone)
        {
            var msg = new ReqCode(phone, CodeType.Reset);
            Inst.SendMessage(msg.ToMessage());
        }



        public static void GetUserInfo(int uid)
        {
            var req = new Utils.Msg(MsgID.ReqUserInfo);
            if (uid != Data.User.Id)
            {
                req.Add("id", uid);
            }
            req.Send();
        }

        internal static void ChangePass(int type, string name, string pass, string code)
        {
            new Utils.Msg(MsgID.ReqReset)
                .Add("type", type)
                .Add("name", name)
                .Add("pass", pass)
                .Add("code", code)
                .Send();
        }
    }
}

