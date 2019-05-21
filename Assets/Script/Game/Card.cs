using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FairyGUI;

namespace Game
{
    public class Card 
    {
        private string info;// 玩家牌的信息
        private GImage cardUi; // 玩家牌的ui

        public string Info { get => info; set => info = value; }
        public GImage CardUi { get => cardUi; set => cardUi = value; }
    }
}