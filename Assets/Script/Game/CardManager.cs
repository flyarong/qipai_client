using System.Collections.Generic;
using UnityEngine;
using FairyGUI;

namespace Game
{
    public class CardManager : MonoBehaviour
    {
        GComponent ui;
        List<GComponent> cardPlaces = new List<GComponent>();
        private void Awake()
        {
            ui = GetComponent<UIPanel>().ui;

            for (var i = 1; i < 11; i++)
            {
                cardPlaces.Add(ui.GetChild("card" + i).asCom);
            }

            EventCenter.AddListener<string, string>(NoticeType.PutCard, PutCard);
        }

        private void OnDestroy()
        {
            EventCenter.RemoveListener<string, string>(NoticeType.PutCard, PutCard);
        }


        void PutCard(string deskId, string cards)
        {
            
            Player player = null;
            foreach (var p in Data.Room.Players)
            {
                if (p.DeskId == int.Parse(deskId))
                {
                    player = p;
                }
            }
            if (player == null)
            {
                Debug.Log("发牌出错");
                return;
            }

            var cp = cardPlaces[player.Index].position;
            foreach (var v in cards.Split('|'))
            {
                Debug.Log("v:" + v);
                var n = int.Parse(v);
                string cardName = getName(n);
                GImage card = UIPackage.CreateObject("qipai", cardName).asImage;
                card.position = cp;
                cp.x += 20;
                card.SetScale(0.6f, 0.6f);
                card.AddRelation(GRoot.inst, RelationType.Middle_Middle);
                card.AddRelation(GRoot.inst, RelationType.Center_Center);
                ui.AddChild(card);
            }


        }

        string getName(int n)
        {
            if (n == -1)
            {
                return "Card_0";
            }

            var type = n % 4 + 1;
            var value = n % 13 + 1;
            // 大小王特殊处理
            if (n > 51)
            {
                type = n == 52 ? 5 : 6;
                value = 0;
            }
            string cardName = "Card_" + type + "_" + value;
            if (type == 5)
            {
                cardName = "Card_joker_1";
            }
            else if (type == 6)
            {
                cardName = "Card_joker_2";
            }

            return cardName;
        }

        //void PutSmallCard(string cardName, Vector3 v3)
        //{
        //    GImage card = UIPackage.CreateObject("qipai", cardName).asImage;
        //    card.position = v3;
        //    card.SetScale(0.35f, 0.35f);
        //    card.AddRelation(GRoot.inst, RelationType.Middle_Middle);
        //    card.AddRelation(GRoot.inst, RelationType.Center_Center);
        //    ui.AddChild(card);
        //}

    }
}