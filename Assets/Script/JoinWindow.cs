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
        textField = contentPane.GetChild("text").asTextField;
    }

    void onBtnNubmerClick(EventContext context)
    {
        var btn = context.sender as GButton;
        if (textField.text.Length >= 6)
        {
            return;
        }

        textField.text += btn.data;
        if (textField.text.Length == 6)
        {
            Hide();
            var roomId = textField.text;

            Data.Room.Id = int.Parse(roomId);
            SceneManager.LoadScene("Game");
        }
    }

    protected override void OnShown()
    {
        textField.text = "";
    }
}