using FairyGUI;
using System;
using System.Collections.Generic;
using UnityEngine;

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
        public Vector3 zhuangPos; // 庄家图标所在位置
        public Vector3 scorePos; // 积分图标所在位置
        public GComponent PlayerUi
        {
            get => playerUi; set
            {
                playerUi = value;
                var zhuang = playerUi.GetChild("zhuang");
                var score = playerUi.GetChild("score");
                zhuangPos = zhuang.position;
                scorePos = score.position;
            }
        }
        //public List<Card> Cards { get => cards; set => cards = value; }

        public int Index { get => index; set => index = value; }
        public int DeskId { get => deskId; set => deskId = value; }
        public List<GObject> CardsUi { get => cardsUi; set => cardsUi = value; }
        public bool IsBanker { get => isBanker; set => isBanker = value; }
        public PlayerInfo Info { get => info; set => info = value; }
    }
}