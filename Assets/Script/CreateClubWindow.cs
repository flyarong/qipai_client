using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using Data;

public class CreateClubWindow : Window
{
    Controller roomTypeController; // 房间类型
    GComboBox scoreBox; // 底分
    GComboBox payBox; // 房费付款方式
    GComboBox tuiBox; // 推注
    GComboBox countBox; // 局数
    GComboBox timesBox; // 翻倍规则
    public CreateClubWindow()
    {

    }

    protected override void OnInit()
    {
        this.contentPane = UIPackage.CreateObject("qipai", "windowCreateClub").asCom;
        this.Center();
        this.modal = true;

        roomTypeController = this.contentPane.GetController("roomType");
        scoreBox = this.contentPane.GetChild("score").asComboBox;
        payBox = this.contentPane.GetChild("pay").asComboBox;
        tuiBox = this.contentPane.GetChild("tui").asComboBox;
        countBox = this.contentPane.GetChild("count").asComboBox;
        timesBox = this.contentPane.GetChild("times").asComboBox;

        countBox.onChanged.Add(onCountBoxChanged);
        roomTypeController.onChanged.Add(onCountBoxChanged);
        this.contentPane.GetChild("btnCreateClub").asButton.onClick.Add(onBtnCreateClubClick);
    }
    private void onCountBoxChanged(EventContext context)
    {
        payBox.items = new string[] {
            "房主付(" +((countBox.selectedIndex+1)*(roomTypeController.selectedIndex+3)) +"钻)",
            "AA支付("+(countBox.selectedIndex+1)+"钻)"
        };
    }

    private void onBtnCreateClubClick()
    {
        int roomType = roomTypeController.selectedIndex;
        int score = scoreBox.selectedIndex;
        int pay = payBox.selectedIndex;
        int tui = tuiBox.selectedIndex;
        int count = countBox.selectedIndex;
        int start = 1; // 固定只有首位可以开始游戏
        int times = timesBox.selectedIndex;

        int players = (roomType + 3) * 2 ;
        count = (count + 1) * 10;

        Api.Club.Create(players, score, start, count, pay, times,tui);
    }
}
