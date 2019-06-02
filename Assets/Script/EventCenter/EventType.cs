

public enum NoticeType
{
    ClubList,
    RoomList,
    FreshClub,

    // 发牌
    PutCard,
    // 发小牌
    PutSmallCard,
    // 创建房间
    RoomCreate,
    // 进入房间
    PlayerSitDown,

    // 退出房间
    RoomExit,

    // 房间解散
    RoomDelete,

    // 进入俱乐部
    JoinClub,
    // 退出俱乐部
    ExitClub,

    // 游戏开始
    RoomStart,

    // 新游戏开局
    GameBegin,

    // 游戏结束
    GameOver,

    // 有人提交了抢庄倍数
    SetTimes,

    // 确定了庄家
    SetBanker,

    // 有人下注
    SetScore,

    // 全部下注完毕
    SetScoreAll,

    // 获取到牌型
    CardTypes,
}
