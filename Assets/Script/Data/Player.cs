using FairyGUI;
using System;
using System.Collections.Generic;

namespace Data
{
    public class PlayerInfo
    {
        public int id;
        public string nick;
        public string avatar;
        public string ip;
        public string address;
        public int card;
        public DateTime createAt;
    }

    public class Player
    {
        private PlayerInfo info; // 玩家信息
        private GComponent playerUi; // 玩家头像等信息的ui
        private bool isBanker;
        private int index; // 玩家在本地ui上的座位号
        private int deskId; // 玩家在服务器上的座位号
        //private List<Card> cards = new List<Card>();
        private List<GObject> cardsUi = new List<GObject>();

        public GComponent PlayerUi { get => playerUi; set => playerUi = value; }
        //public List<Card> Cards { get => cards; set => cards = value; }

        public int Index { get => index; set => index = value; }
        public int DeskId { get => deskId; set => deskId = value; }
        public List<GObject> CardsUi { get => cardsUi; set => cardsUi = value; }
        public bool IsBanker { get => isBanker; set => isBanker = value; }
        public PlayerInfo Info { get => info; set => info = value; }
    }
}