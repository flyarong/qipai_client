namespace Network.Msg
{
    public enum MsgID
    {
        NoPermission = 100,

        ReqLogin,     // 登录请求

        ResLogin,     // 响应登录结果

        ReqReg,       // 用户注册

        ResReg,       // 响应注册结果

        ReqReset,     // 重置密码

        ResReset,     // 响应重置密码结果

        ReqBind,      // 账号绑定

        ResBind,      // 响应绑定结果

        ReqGetUserInfo, // 获取用户信息

        ResGetUserInfo, // 响应用户信息

        ReqCode,    // 请求手机验证码

        ResCode,     // 返回验证码发送结果

        ReqLoginByToken, // 通过token登录

        ResLoginByToken, 


        /*************房间相关************/

        // 创建房间
        ReqCreateRoom = 201,

        ResCreateRoom,
        // 房间列表
        ReqGetRoomList,

        ResGetRoomList,
        // 房间信息
        ReqRoom,

        ResRoom,
        // 进入房间
        ReqJoinRoom,

        ResJoinRoom, // 此处返回当前房间的所有玩家

        // 广播进入房间
        BroadcastJoinRoom,

        // 坐下
        ReqSit,

        ResSit,

        // 离开房间
        ReqLeaveRoom,

        ResLeaveRoom,
        // 解散房间
        ReqDeleteRoom,

        ResDeleteRoom,
    }
}
