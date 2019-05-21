using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;


public class UserInfo : MonoBehaviour
{
    private GComponent mainUI;
    // Start is called before the first frame update
    void Start()
    {
        new Api.User().GetUserInfo();
        mainUI = GetComponent<UIPanel>().ui;

        var header = mainUI.GetChild("header").asCom;
        var userInfo = header.GetChild("userInfo").asCom;
        userInfo.GetChild("textNick").asTextField.text = "昵称:"+Data.User.Info["nick"].str;
        userInfo.GetChild("textId").asTextField.text = "ID:"+ Data.User.Info["id"].n;
        userInfo.GetChild("textGold").asTextField.text = Data.User.Info["card"].n + "";
        userInfo.GetChild("imgAvatar").asLoader.url = "/static" + Data.User.Info["avatar"].str;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
