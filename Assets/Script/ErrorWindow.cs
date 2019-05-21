using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;

public class ErrorWindow : Window
{

    float t = 3;
    public ErrorWindow()
    {

    }

    public float T {set => t = value; }

    public void SetMsg(string errMsg)
    {
        this.contentPane.GetChild("msg").asTextField.text = errMsg;
    }

    protected override void OnInit()
    {
        this.contentPane = UIPackage.CreateObject("qipai", "errorWindow").asCom;
        this.onClick.Add(()=>
        {
            Debug.Log("错误窗口被点击");
            this.Hide();
        });
        this.modal = true;
        this.Center();
    }

    protected override void OnShown()
    {
        t = 3;
    }

    protected override void OnUpdate()
    {
        t -= Time.deltaTime;
        if (t < 0)
        {
            this.Hide();
        }
    }
}
