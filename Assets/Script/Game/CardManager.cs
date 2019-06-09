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
        }

        private void Start()
        {

        }

        private void OnDestroy()
        {
        }


        void GameBegin(string roomId)
        {
            if (roomId == Data.Room.Id + "")
            {
                return;
            }

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