using UnityEngine;
using System.Collections;
using Network.Msg;
using Network;

namespace Api
{
    public class User
    {
        private static Manager Inst = Manager.Inst;
        public static void Login(LoginType type, string name, string pass)
        {
            var msg = new ReqLogin(type, name, pass);
            Inst.SendMessage(msg.ToMessage());
        }

        public static void LoginByToken()
        {
            if (Data.User.Token == "") {
                return;
            }
            var msg = new ReqLoginByToken(Data.User.Token);
            Inst.SendMessage(msg.ToMessage());
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
    }
}

