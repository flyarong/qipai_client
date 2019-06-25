using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using UnityEngine.SceneManagement;
using System;

namespace History
{
    public class CalcWindow : Window
    {
        GComponent ui = null;


        public CalcWindow(GComponent ui)
        {
            this.ui = ui ?? throw new ArgumentNullException(nameof(ui));
        }

        GTextField textField;
        protected override void OnInit()
        {
            contentPane = UIPackage.CreateObject("qipai", "calcWindow").asCom;
            Center();
            modal = true;

            for (var i = 0; i < 10; i++)
            {
                this.contentPane.GetChild("btn" + i).onClick.Add(onBtnNubmerClick);
            }
            contentPane.GetChild("btnOk").onClick.Add(onBtnOkClick);
            contentPane.GetChild("btnClear").onClick.Add(onBtnClearClick);
            contentPane.GetChild("btnDot").onClick.Add(onBtnNubmerClick);
            contentPane.GetChild("btnDel").onClick.Add(onBtnNubmerClick);
            textField = contentPane.GetChild("text").asTextField;
        }
        
        private void onBtnClearClick(EventContext context)
        {
            textField.text = "";
        }

        private void onBtnOkClick(EventContext context)
        {
            if (textField.text == "") return;
            for (var i = 1; i <= 10; i++)
            {
                var u = ui.GetChild("u" + i).asCom;
                if (!u.visible)
                {
                    continue;
                }
                var score = u.GetChild("score").asTextField;
                var times = float.Parse(textField.text);
                var v = int.Parse(score.data+"") * times;
                score.text = (v > 0 ? "+" : "") + v;

                ui.GetChild("textTip").text = "本局积分已X" + times + "倍";
            }
            Hide();
        }

        void onBtnNubmerClick(EventContext context)
        {
            var btn = context.sender as GButton;


            if (btn.title == "del")
            {
                if (textField.text.Length == 0) return;
                textField.text = textField.text.Substring(0, textField.text.Length - 1);
                return;
            }

            if (btn.data + "" == "." && textField.text.Contains("."))
            {
                return;
            }

            if (float.Parse(textField.text + btn.data) > 999)
            {
                return;
            }

            textField.text += btn.data;
        }

        protected override void OnShown()
        {
            textField.text = "";
        }
    }

}