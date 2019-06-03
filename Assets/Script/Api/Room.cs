using Network.Msg;
using Network;
using System;
namespace Api
{
    public static class Room
    {
        private static Manager Inst = Manager.Inst;
        internal static void Create(int players, int score, int start, int count, int pay, int times)
        {
            var msg = new ReqCreateRoom(players, score, start, count, pay, times);
            Inst.SendMessage(msg.ToMessage());
        }
    }
}