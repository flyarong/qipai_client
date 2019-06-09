using Network;
using Network.Msg;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Api
{
    public static class Game
    {
        private static Manager Inst = Manager.Inst;
        
        public static void  Start(int roomId)
        {
            if (roomId <= 0)
            {
                Debug.LogWarning("不合法的roomid: " + roomId);
                SceneManager.LoadScene("Menu");
                return;
            }

            new Utils.Msg(MsgID.ReqGameStart).Add("roomId", roomId).Send();
        }
    }
}
