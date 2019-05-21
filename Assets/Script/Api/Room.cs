using UnityEngine;
using System.Collections.Generic;

namespace Api
{
    public class Room
    {
        Client client;
        public Room()
        {
            client = Client.Inst;
        }

        public JSONObject GetRoom(string roomId)
        {
            string text = client.GetContent("/rooms/" + roomId);
            Debug.Log(text);
            return new JSONObject(text);
        }


        /// <summary>
        /// 创建房间
        /// </summary>
        /// <param name="players">房间玩家个数</param>
        /// <param name="score">底分下拉选项</param>
        /// <param name="start">房间开始方式</param>
        /// <param name="count">局数下拉选项</param>
        /// <param name="pay">房费付款模式</param>
        /// <param name="king">王癞模式</param>
        /// <param name="sp">特殊牌型</param>
        /// <param name="times">翻倍规则</param>
        /// <returns>返回创建后的json对象</returns>
        public JSONObject Create(int players, int score, int start, int count, int pay, int king, int sp, int times)
        {
            List<KeyValuePair<string, string>> paramList = new List<KeyValuePair<string, string>>();
            paramList.Add(new KeyValuePair<string, string>("players", players + ""));
            paramList.Add(new KeyValuePair<string, string>("score", score + ""));
            paramList.Add(new KeyValuePair<string, string>("start", start + ""));
            paramList.Add(new KeyValuePair<string, string>("count", count == 0 ? "10" : count == 1 ? "20" : "30"));
            paramList.Add(new KeyValuePair<string, string>("pay", pay + ""));
            paramList.Add(new KeyValuePair<string, string>("king", king + ""));
            paramList.Add(new KeyValuePair<string, string>("special", sp + ""));
            paramList.Add(new KeyValuePair<string, string>("times", times + ""));

            return new JSONObject(client.PostContent("/rooms", paramList));
        }

        public JSONObject List()
        {
            string content = Client.Inst.GetContent("/rooms");
            Debug.Log(content);
            return new JSONObject(content);
        }

        /// <summary>
        /// 进入房间
        /// </summary>
        /// <param name="roomId"></param>
        /// <returns></returns>
        public JSONObject Join(string roomId)
        {
            var content = Client.Inst.PostContent("/rooms/" + roomId + "/players", null);
            Debug.Log(content);
            return new JSONObject(content);
        }

        /// <summary>
        /// 玩家坐下
        /// </summary>
        /// <param name="roomId"></param>
        /// <returns></returns>
        public JSONObject SitDown(string roomId)
        {
            var content = Client.Inst.PutContent("/rooms/" + roomId + "/players/sit", null);
            Debug.Log(content);
            return new JSONObject(content);
        }

        /// <summary>
        /// 开始游戏
        /// </summary>
        /// <param name="roomId"></param>
        /// <returns></returns>
        public JSONObject Start(string roomId)
        {
            var content = Client.Inst.PutContent("/rooms/" + roomId + "/start", null);
            Debug.Log(content);
            return new JSONObject(content);
        }

        /// <summary>
        /// 玩家退出房间
        /// </summary>
        /// <param name="roomId"></param>
        /// <returns></returns>
        public JSONObject Exit(string roomId)
        {
            var content = Client.Inst.PutContent("/rooms/" + roomId + "/players/exit", null);
            Debug.Log(content);
            return new JSONObject(content);
        }


        public JSONObject Players(string roomId)
        {
            var content = Client.Inst.GetContent("/rooms/" + roomId + "/players");
            Debug.Log(content);
            return new JSONObject(content);
        }


        public JSONObject Player(string roomId, string uid = "")
        {
            var content = Client.Inst.GetContent("/rooms/" + roomId + "/player/" + uid);
            Debug.Log(content);
            return new JSONObject(content);
        }

        /// <summary>
        /// 获取纸牌
        /// </summary>
        /// <param name="roomId"></param>
        /// <param name="uid"></param>
        /// <returns></returns>
        public JSONObject GetCards(string roomId, string uid = "")
        {
            var content = Client.Inst.GetContent("/rooms/" + roomId + "/cards/" + uid);
            Debug.Log(content);
            return new JSONObject(content);
        }

        public JSONObject SetScore(string roomId, string score)
        {
            var content = Client.Inst.PutContent("/rooms/" + roomId + "/score/" + score, null);
            Debug.Log(content);
            return new JSONObject(content);
        }
    }
}
