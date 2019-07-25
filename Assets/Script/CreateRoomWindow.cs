using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using System;

public class CreateRoomWindow : Window
{
    Controller roomTypeController; // 房间类型
    GComboBox scoreBox; // 底分
    GComboBox payBox; // 房费付款方式
    GComboBox tuiBox; // 推注
    GComboBox countBox; // 局数
    GComboBox startBox; // 游戏开始方式
    GComboBox timesBox; // 翻倍规则
    GComboBox kingBox; // 王癞模式
    GButton[] sps = new GButton[7];

    protected override void OnInit()
    {
        this.contentPane = UIPackage.CreateObject("qipai", "windowCreateRoom").asCom;
        this.Center();
        this.modal = true;

        roomTypeController = this.contentPane.GetController("roomType");
        scoreBox = this.contentPane.GetChild("score").asComboBox;
        tuiBox = this.contentPane.GetChild("tui").asComboBox;
        payBox = this.contentPane.GetChild("pay").asComboBox;
        countBox = this.contentPane.GetChild("count").asComboBox;
        startBox = this.contentPane.GetChild("start").asComboBox;
        timesBox = this.contentPane.GetChild("times").asComboBox;
        countBox.onChanged.Add(onCountBoxChanged);
        roomTypeController.onChanged.Add(onCountBoxChanged);
        this.contentPane.GetChild("btnCreateRoom").asButton.onClick.Add(onBtnCreateRoomClick);
    }

    private void onCountBoxChanged(EventContext context)
    {
        payBox.items = new string[] {
            "房主付(" +((countBox.selectedIndex+1)*(roomTypeController.selectedIndex+3)) +"钻)",
            "AA支付("+(countBox.selectedIndex+1)+"钻)"
        };
    }

    private void onBtnCreateRoomClick()
    {
        int type = roomTypeController.selectedIndex;
        int score = scoreBox.selectedIndex;
        int pay = payBox.selectedIndex;
        int tui = tuiBox.selectedIndex;
        int count = countBox.selectedIndex;
        int start = startBox.selectedIndex;
        int times = timesBox.selectedIndex;

        int players = 2 * (type + 3);
        count = (count + 1) * 10;
        Api.Room.Create(players, score, start, count, pay, times,tui);

    }
}