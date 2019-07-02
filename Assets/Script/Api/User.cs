using UnityEngine;
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

        public static void LoginByWeChatCode(string code)
        {
            if (code == "")
            {
                return;
            }
            new Utils.Msg(MsgID.ReqLoginByWeChatCode).Add("code", code).Send();
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
            var req = new Utils.Msg(MsgID.ReqReset);
            req.Add("type", type);
            req.Add("name", name);
            req.Add("pass", pass);
            req.Add("code", code);
            req.Send();
        }

        public static void GetNotice()
        {
            new Utils.Msg(MsgID.ReqNotice).Send();
        }

        public static void GetRollText()
        {
            new Utils.Msg(MsgID.ReqRollText).Send();
        }

        public static void GetShareText()
        {
            new Utils.Msg(MsgID.ReqShareText).Send();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="roomId">房间编号</param>
        /// <param name="deskId">座位编号</param>
        /// <param name="voiceId">语音编号0-9</param>
        /// <param name="sex">性别，女0，男1</param>
        public static void SayDefaultVoice(int roomId, int deskId, int voiceId, int sex)
        {
            new Utils.Msg(MsgID.ReqDefaultVoice).Add("voiceId", voiceId).Add("roomId", roomId).Add("deskId", deskId).Add("sex", sex).Send();
        }
    }
}

