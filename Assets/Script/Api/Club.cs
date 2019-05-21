using UnityEngine;
using System.Collections.Generic;

namespace Api
{
    public class Club
    {
        Client client;
        public Club()
        {
            client = Client.Inst;
        }

        public JSONObject GetClub(string clubId)
        {
            string text = client.GetContent("/clubs/" + clubId);
            return new JSONObject(text);
        }

        /// <summary>
        /// 编辑俱乐部信息
        /// </summary>
        /// <param name="clubId">俱乐部编号</param>
        /// <param name="check">是否需要审查</param>
        /// <param name="close">是否打烊</param>
        /// <param name="name">俱乐部名字</param>
        /// <param name="roolText">俱乐部大厅滚动字幕</param>
        /// <param name="notice">俱乐部通知信息</param>
        /// <returns>返回请求的内容</returns>
        public JSONObject EditClubInfo(string clubId, bool check, bool close, string name, string rollText, string notice)
        {
            List<KeyValuePair<string, string>> paramList = new List<KeyValuePair<string, string>>();
            paramList.Add(new KeyValuePair<string, string>("check", check ? "true" : "false"));
            paramList.Add(new KeyValuePair<string, string>("close", close ? "true" : "false"));
            paramList.Add(new KeyValuePair<string, string>("name", name));
            paramList.Add(new KeyValuePair<string, string>("roll_text", rollText));
            paramList.Add(new KeyValuePair<string, string>("notice", notice));
            string content = client.PutContent("/clubs/" + clubId, paramList);
            return new JSONObject(content);
        }

        /// <summary>
        /// 创建俱乐部
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

            return new JSONObject(client.PostContent("/clubs", paramList));
        }

        /// <summary>
        /// 列出俱乐部
        /// </summary>
        /// <returns></returns>
        public JSONObject List()
        {
            Client client = new Client(Data.User.Token);
            string content = client.GetContent("/clubs");
            Debug.Log(content);
            return new JSONObject(content);
        }

        /// <summary>
        /// 改变当前所在俱乐部中指定成员的状态
        /// </summary>
        /// <param name="memberUid"></param>
        /// <param name="action">action 编辑会员状态：设为管理(admin) 取消管理(_admin)  冻结(disable) 取消冻结(_disable) 设为代付(pay) 取消代付(_pay) 审核通过用户(add)  移除用户(_add)</param>
        /// <returns></returns>
        public JSONObject ChangeMemberStatus(int memberUid, string action)
        {
            string text = client.PutContent("/clubs/" + Data.Club.Data["id"].n + "/user/" + memberUid + "/" + action, null);
            return new JSONObject(text);
        }

        public JSONObject GetMembers(string clubId)
        {
            string text = new Client(PlayerPrefs.GetString("token")).GetContent("/clubs/" + clubId + "/users");
            return new JSONObject(text);
        }
    }
}
