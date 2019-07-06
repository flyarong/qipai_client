using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using Data;
namespace Club
{
    public class EditClubRoomWindow : Window
    {
        Controller roomTypeController; // 房间类型
        GComboBox scoreBox; // 底分
        GComboBox countBox; // 局数
        GComboBox timesBox; // 翻倍规则
        int clubId;
        int tableId;

        public EditClubRoomWindow(int clubId, int tableId)
        {
            this.clubId = clubId;
            this.tableId = tableId;
        }

        protected override void OnInit()
        {
            this.contentPane = UIPackage.CreateObject("qipai", "windowEditClubRoom").asCom;
            this.Center();
            this.modal = true;

            roomTypeController = this.contentPane.GetController("roomType");
            scoreBox = this.contentPane.GetChild("score").asComboBox;
            countBox = this.contentPane.GetChild("count").asComboBox;
            timesBox = this.contentPane.GetChild("times").asComboBox;
            
            this.contentPane.GetChild("btnCreateClub").asButton.onClick.Add(onBtnCreateClubClick);
        }

        protected override void OnShown()
        {
            base.OnShown();
            
        }

        private void onBtnCreateClubClick()
        {
            int roomType = roomTypeController.selectedIndex;
            int score = scoreBox.selectedIndex;
            int count = countBox.selectedIndex;
            int start = 1; // 固定只有首位可以开始游戏
            int times = timesBox.selectedIndex;

            int players = (roomType + 3) * 2;
            count = (count + 1) * 10;

            Api.Club.EditClubRoom(clubId, tableId, players, score, start, count, times);
        }
    }
}