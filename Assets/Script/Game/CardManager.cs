using System.Collections.Generic;
using UnityEngine;
using FairyGUI;

namespace Game
{
    public class CardManager : MonoBehaviour
    {
        GComponent ui;
        List<GComponent> cardPlaces = new List<GComponent>();
        Dictionary<string, List<GImage>> cardsUi = new Dictionary<string, List<GImage>>();
        Transition tPut;
        private void Awake()
        {
            ui = GetComponent<UIPanel>().ui;
            tPut = ui.GetTransition("t1");
            ui.GetChild("cardImg").sortingOrder = 1000;
            for (var i = 1; i < 11; i++)
            {
                cardPlaces.Add(ui.GetChild("card" + i).asCom);
            }
            EventCenter.AddListener<string, string>(NoticeType.PutCard, PutCard);
            EventCenter.AddListener<string>(NoticeType.GameBegin, GameBegin);
        }

        private void Start()
        {
            
        }

        private void OnDestroy()
        {
            EventCenter.RemoveListener<string, string>(NoticeType.PutCard, PutCard);
            EventCenter.RemoveListener<string>(NoticeType.GameBegin, GameBegin);
        }


        void GameBegin(string roomId)
        {
            if (roomId == Data.Room.Id + "")
            {
                return;
            }

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
                return;
            }

            List<GImage> cardImgs = new List<GImage>();
            var cp = cardPlaces[player.Index];
            var cpos = cp.position;
            foreach (var v in cards.Split('|'))
            {
                if (v == "-")
                {
                    cpos.x += player.DeskId == Data.Room.DeskId ? 40 : 15;
                    continue;
                }

                var n = int.Parse(v);
                string cardName = getName(n);
                GImage card = UIPackage.CreateObject("qipai", cardName).asImage;
                card.position = cpos;

                if (player.DeskId == Data.Room.DeskId)
                {
                    cpos.x += 50;
                   // card.SetScale(0.6f, 0.6f);
                }
                else
                {
                    cpos.x += 23;
                    card.SetScale(0.6f, 0.6f);
                }
                
                tPut.SetValue("end",cpos.x,cpos.y);
                tPut.Play();

                card.AddRelation(cp, RelationType.Middle_Middle);
                card.AddRelation(cp, RelationType.Center_Center);
                ui.AddChild(card);
                cardImgs.Add(card);
                
            }

            if (cardsUi.ContainsKey(deskId))
            {
                foreach (var img in cardsUi[deskId])
                {
                    ui.RemoveChild(img);
                }
                cardsUi.Remove(deskId);
            }

            cardsUi.Add(deskId, cardImgs);
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