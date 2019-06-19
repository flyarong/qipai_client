using UnityEngine;
using System.Collections;
using FairyGUI;
namespace Utils
{
    public class ConfirmWindow : Window
    {
        private string msg;
        EventCallback0 callback;

        private ConfirmWindow()
        {

        }

        public ConfirmWindow(EventCallback0 callback, string msg = "确认执行此操作吗？")
        {
            this.callback = callback;
            this.msg = msg;
        }

        protected override void OnInit()
        {
            this.contentPane = UIPackage.CreateObject("qipai", "ConfirmWindow").asCom;
            this.Center();
            this.modal = true;

            this.contentPane.GetChild("text").text = msg;
            this.contentPane.GetChild("btnOk").onClick.Add(() =>
            {
                callback();
                this.Hide();
                this.Dispose();
            });
        }

        public static void ShowBox(EventCallback0 callback, string msg = "")
        {
            ConfirmWindow confirm = msg == "" ? new ConfirmWindow(callback) : new ConfirmWindow(callback, msg);
            confirm.Show();
        }
    }

}