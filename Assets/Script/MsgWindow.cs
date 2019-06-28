using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;

public class MsgWindow : Window
{
    public MsgWindow()
    {

    }

    public void SetTitle(string title)
    {
        this.contentPane.GetChild("frame").asLabel.title = title;
    }
    public void SetMsg(string content)
    {
        this.contentPane.GetChild("container").asCom.GetChild("content").asRichTextField.text = content;
    }

    protected override void OnInit()
    {
        this.contentPane = UIPackage.CreateObject("qipai", "MsgWindow").asCom;
        this.modal = true;
        this.Center();
    }

}
