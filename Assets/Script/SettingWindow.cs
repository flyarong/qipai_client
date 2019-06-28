using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using UnityEngine.SceneManagement;

public class SettingWindow : Window
{

    public SettingWindow()
    {

    }

    protected override void OnInit()
    {
        this.contentPane = UIPackage.CreateObject("qipai", "windowSetting").asCom;
        this.contentPane.GetChild("btnChangeUser").onClick.Add(() =>
        {
            Utils.ConfirmWindow.ShowBox(() =>
            {
                PlayerPrefs.DeleteAll();
                Data.User.Info = null;
                Data.User.Id = 0;
                Data.User.Token = "";
                SceneManager.LoadScene("Login");
                this.Hide();
            }, "确认要退出当前账号吗？");

        });

        this.Center();
        this.modal = true;
    }
}
