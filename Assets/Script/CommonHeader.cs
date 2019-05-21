using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using Api;

public class CommonHeader : MonoBehaviour
{

    //Client client;
    private GComponent mainUI;
    private SettingWindow settingWindow;


    // Start is called before the first frame update
    void Start()
    {
        //// 加载个人信息
        //var token = PlayerPrefs.GetString("token");
        //Debug.Log("player token:" + token);
        //client = new Client(token);

        //string text = client.GetContent("/users");
        //Debug.Log(text);
        //JSONObject j = new JSONObject(text);

        settingWindow = new SettingWindow();
        mainUI = GetComponent<UIPanel>().ui;

        var header = mainUI.GetChild("header").asCom;
        //header.GetChild("textNick").asTextField.text = j["data"]["user"]["nick"].str;
        //header.GetChild("textGold").asTextField.text = j["data"]["user"]["card"].n + "";
        //header.GetChild("imgAvatar").asLoader.url = "/static" + j["data"]["user"]["avatar"].str;
        header.GetChild("btnSetting").onClick.Add(() =>
        {
            settingWindow.Show();
            settingWindow.Center();
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
