using UnityEngine;
using System.Collections;
using FairyGUI;

namespace Room
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
        }

        void onQuitClick()
        {
            var j = new Api.Room().Exit(Data.Room.Id);
            if (j["code"].n != 0)
            {
                Utils.MsgBox.ShowErr(j["msg"].str);
                return;
            }
            Hide();
        }

        protected override void OnShown()
        {
            UIConfig.modalLayerColor = new Color(0f, 0f, 0f, 0f);
        }
    }

}