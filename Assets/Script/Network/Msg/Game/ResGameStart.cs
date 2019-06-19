using System;
using Newtonsoft.Json;

namespace Network.Msg
{
    /*
    Banker     bool   `json:"banker"`                  // 是否是庄家 true表示是庄家
	PlayerId   uint   `json:"playerId"`                // 玩家编号
	RoomId     uint   `json:"roomId"`                  // 房间编号
	DeskId     int    `json:"deskId"`                  // 座位号
	Score      int    `json:"score"`                   // 闲家下注
	Times      int    `gorm:"default:-1" json:"times"` // 抢庄倍数
	CardType   int    `json:"cardType"`                // 牌类型，记录是牛几
	Special    int    `json:"special"`                 // 特殊牌型加倍
	TotalScore int    `json:"totalScore"`              // 输赢积分，通过底分*庄家倍数*特殊牌型加倍 计算
	Cards      string `json:"cards"`                   // 用户所拥有的牌
	Current    int    `json:"current"`                 // 这是第几局
	Auto       bool   `json:"auto"`                    // 是否自动托管
    */

    [Serializable]
    public class GameInfo
    {
        public bool banker;
        public int playerId;
        public int roomId;
        public int deskId;
        public int score;
        public int times;
        public int cardType;
        public int special;
        public int totalScore;
        public string cards;
        public int current;
        public bool auto;
    }

    [Serializable]
    public class ResGameStart : BaseMsg
    {
        public int code;
        public string msg;
        public GameInfo game;

        public ResGameStart() : base(MsgID.ResGameStart)
        {
        }

        public override void FromData(byte[] data)
        {
            var jsonString = System.Text.Encoding.UTF8.GetString(data);
            var jsonData = JsonConvert.DeserializeObject<ResGameStart>(jsonString);
            this.code = jsonData.code;
            this.msg = jsonData.msg;
            this.game = jsonData.game;
        }

        public override byte[] ToData()
        {
            return new byte[0];
        }
    }
}