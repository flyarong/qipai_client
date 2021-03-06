using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using UnityEngine.SceneManagement;

public class JoinWindow : Window
{
    GTextField textField;
    protected override void OnInit()
    {
        contentPane = UIPackage.CreateObject("qipai", "JoinWindow").asCom;
        Center();
        modal = true;

        for (var i = 0; i < 10; i++)
        {
            this.contentPane.GetChild("btn" + i).onClick.Add(onBtnNubmerClick);
        }
        contentPane.GetChild("reInput").onClick.Add(onBtnNubmerClick);
        contentPane.GetChild("del").onClick.Add(onBtnNubmerClick);
        textField = contentPane.GetChild("text").asTextField;
    }

    void onBtnNubmerClick(EventContext context)
    {
        var btn = context.sender as GButton;
        if (textField.text.Length >= 6)
        {
            return;
        }


        if (btn.title == "reInput")
        {
            textField.text = "";
            return;
        }
        else if (btn.title == "del")
        {
            if (textField.text.Length == 0) return;
            textField.text = textField.text.Substring(0, textField.text.Length - 1);
            return;
        }

        textField.text += btn.data;
        if (textField.text.Length == 6)
        {
            Hide();
            var id = textField.text;

            if (PlayerPrefs.GetString("joinType") == "room")
            {
                Data.Game.Id = int.Parse(id);
                SceneManager.LoadScene("Game");
            }
            else
            {
                Data.Club.Id = int.Parse(id);
                SceneManager.LoadScene("Club");
            }

        }
    }

    protected override void OnShown()
    {
        textField.text = "";
    }
}