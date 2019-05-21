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
            PlayerPrefs.DeleteAll();
            SceneManager.LoadScene("Login");
            Debug.Log("切换账号按钮被点击");
            this.Hide();
        });
        this.Center();
        this.modal = true;

    }
}
