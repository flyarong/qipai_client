using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Network.Msg
{
    /*
     *  Id        uint           `json:"id" xml:"ID"`
		Name      string         `json:"name"`       // 俱乐部名称
		Check     bool           `json:"check"`      // 是否审查
		Notice    string         `json:"notice"`     // 公告
		RollText  string         `json:"rollText"`  // 俱乐部大厅滚动文字
		Score     enum.ScoreType `json:"score"`      // 底分 以竖线分割的底分方式
		Players   int            `json:"players"`    // 玩家个数
		Count     int            `json:"count"`      // 局数
		StartType enum.StartType `json:"startType"` // 游戏开始方式 只支持1 首位开始
		Pay       enum.PayType   `json:"pay"`        // 付款方式 0 俱乐部老板付 1 AA
		Times     enum.TimesType `json:"times"`      // 翻倍规则，预先固定的几个选择，比如：牛牛x3  牛九x2
		Special   int            `json:"special"`    // 特殊牌型,二进制位表示特殊牌型翻倍规则，一共7类特殊牌型，用最低的7位二进制表示，1表示选中0表示没选中。
		King      enum.KingType  `json:"king"`       // 王癞 0 无王癞  1 经典王癞 2 疯狂王癞
		Uid       uint           `json:"uid"`        // 老板
		Close     bool           `json:"close"`      // 是否打烊
		PayerUid  uint           `json:"payerUid"`  // 代付用户id
		BossNick  string         `json:"boss"`
     */
    [Serializable]
    public class ClubInfo
    {
        public int id;
        public string name;
        public bool check;
        public string notice;
        public string rollText;
        public int score;
        public int players;
        public int count;
        public int startType;
        public int pay;
        public int times;
        public bool tui;
        public int special;
        public int uid;
        public bool close;
        public int payerUid;
        public string boss;
    }

    [Serializable]
    public class ResClub : BaseMsg
    {
        public int code;
        public string msg;
        public ClubInfo club;
        public List<ClubInfo> tables;

        public ResClub() : base(MsgID.ResClub)
        {
        }

        public override void FromData(byte[] data)
        {
            var jsonString = System.Text.Encoding.UTF8.GetString(data);
            var jsonData = JsonConvert.DeserializeObject<ResClub>(jsonString);
            this.code = jsonData.code;
            this.msg = jsonData.msg;
            this.club = jsonData.club;
            tables = jsonData.tables;
        }

        public override byte[] ToData()
        {
            return new byte[0];
        }
    }
}