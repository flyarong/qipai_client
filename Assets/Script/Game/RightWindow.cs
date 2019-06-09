using UnityEngine;
using System.Collections;
using FairyGUI;
using UnityEngine.SceneManagement;
using System;

namespace Game
{
    public class RightWindow : Window
    {
        protected override void OnHide()
        {
            UIConfig.modalLayerColor = new Color(0f, 0f, 0f, 0.0f);
        }

        protected override void OnInit()
        {
            this.contentPane = UIPackage.CreateObject("qipai", "roomSettingWindow").asCom;
            this.Center();
            this.modal = true;
            this.x = GRoot.inst.width - this.width;
            this.height = GRoot.inst.height;
            

            contentPane.GetChild("btnQuit").onClick.Add(onQuitClick);
            contentPane.GetChild("btnDelete").onClick.Add(onDeleteClick);
        }

        private void onDeleteClick(EventContext context)
        {
            Api.Room.Delete(Data.Room.Id);
            Hide();
        }

        void onQuitClick()
        {
            Api.Room.Leave(Data.Room.Id);
            Hide();
        }

        protected override void OnShown()
        {
            UIConfig.modalLayerColor = new Color(0f, 0f, 0f, 0f);
        }
    }

}