using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;

public class CreateRoomWindow : Window
{
    Controller roomTypeController; // 房间类型
    GComboBox scoreBox; // 底分
    GComboBox payBox; // 房费付款方式
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
        payBox = this.contentPane.GetChild("pay").asComboBox;
        countBox = this.contentPane.GetChild("count").asComboBox;
        startBox = this.contentPane.GetChild("start").asComboBox;
        timesBox = this.contentPane.GetChild("times").asComboBox;
        kingBox = this.contentPane.GetChild("king").asComboBox;

        for (var i = 0; i < 7; i++)
        {
            sps[i] = this.contentPane.GetChild("sp" + (i + 1)).asButton;
        }

        this.contentPane.GetChild("btnCreateRoom").asButton.onClick.Add(onBtnCreateRoomClick);
    }

    private void onBtnCreateRoomClick()
    {
        int roomType = roomTypeController.selectedIndex;
        int score = scoreBox.selectedIndex;
        int pay = payBox.selectedIndex;
        int count = countBox.selectedIndex;
        int start = startBox.selectedIndex;
        int times = timesBox.selectedIndex;
        int king = kingBox.selectedIndex;
        int sp = 0;
        for (var i = 0; i < 7; i++)
        {
            if (sps[i].selected)
            {
                sp |= (1 << (6 - i));
            }

        }

        int players = 6;
        if (roomType == 0)
        {
            players = 6;
        }
        else if (roomType == 1)
        {
            players = 8;
        }
        else if (roomType == 2)
        {
            players = 10;
        }

        var j = new Api.Room()
            .Create(players, score, start, count, pay, king, sp, times);
        if (j["code"].n != 0)
        {
            Utils.MsgBox.ShowErr(j["msg"].str);
            return;
        }

        var roomId = j["data"]["id"].n + "";
        j = new Api.Room().Join(roomId);

        if (j["code"].n != 0)
        {
            if (j["msg"].str == "该用户已经进入房间，不得重复进入")
            {
                EventCenter.Broadcast<string>(NoticeType.PlayerSitDown, roomId);
                return;
            }

            Utils.MsgBox.ShowErr(j["msg"].str);
            return;
        }
        
        this.Hide();
    }
}
