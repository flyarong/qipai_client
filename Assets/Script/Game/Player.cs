using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;

namespace Game
{
    public class Player
    {
        private JSONObject userInfo; // 玩家信息
        private GComponent playerUi; // 玩家头像等信息的ui
        private int index; // 玩家在本地ui上的座位号
        private int deskId; // 玩家在服务器上的座位号
        private List<Card> cards = new List<Card>();
        private List<GObject> cardsUi = new List<GObject>();

        public JSONObject UserInfo { get => userInfo; set => userInfo = value; }
        public GComponent PlayerUi { get => playerUi; set => playerUi = value; }
        public List<Card> Cards { get => cards; set => cards = value; }
    
        public int Index { get => index; set => index = value; }
        public int DeskId { get => deskId; set => deskId = value; }
        public List<GObject> CardsUi { get => cardsUi; set => cardsUi = value; }

        public override string ToString()
        {
            return "uid:" + userInfo["id"] + "\tDeskId:" + deskId + "\tLocal DeskId:" + index;
        }
    }
}
