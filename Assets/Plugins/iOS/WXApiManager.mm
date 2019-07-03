
//
//  WXApiManager.m
//  Unity-iPhone
//
//  Created by Apple on 2018/8/2.
//

#import "WXApiManager.h"

@interface WXApiManager ()

@property (nonatomic, strong) NSString *authState;

@end

@implementation WXApiManager

#pragma mark - Life Cycle
+ (instancetype)sharedManager {
    static dispatch_once_t onceToken;
    static WXApiManager *instance = nil;
    dispatch_once(&onceToken, ^{
        instance = [[self alloc] init];
    });
    return instance;
}

//这是向微信终端注册你的appid
void RegToWechat(char* appId)
{
    NSString *weichatId = [NSString stringWithFormat:@"%s", appId];
    [WXApi registerApp:weichatId];
}

void LoginWeChat()
{
    NSLog(@"WXApiManager ios微信登陆");
    //构造SendAuthReq结构体
    SendAuthReq* req    =[[SendAuthReq alloc]init];
    req.scope           = @"snsapi_userinfo";
    req.state           = @"123";
    // req.openID          = kAuthOpenID;
    //第三方向微信终端发送一个SendAuthReq消息结构
    [WXApi sendReq:req];
}

#pragma mark - WXApiDelegate
-(void)onReq:(BaseReq*)req {
    // just leave it here, WeChat will not call our app
}

-(void)onResp:(BaseResp*)resp {
    if([resp isKindOfClass:[SendAuthResp class]]) {
        SendAuthResp* authResp = (SendAuthResp*)resp;
        switch (resp.errCode) {
            case WXSuccess:
                NSLog(@"RESP:code:%@,state:%@\n", authResp.code, authResp.state);
                UnitySendMessage("Login", "ResCode", [authResp.code  UTF8String]);
                break;
            case WXErrCodeAuthDeny:
                NSLog(@"WXErrCodeAuthDeny:code:%@,state:%@\n", authResp.code, authResp.state);
                break;
            case WXErrCodeUserCancel:
                NSLog(@"WXErrCodeUserCancel:code:%@,state:%@\n", authResp.code, authResp.state);
                break;
            default:
                NSLog(@"resp.errCode:code:%@,state:%@\n", authResp.code, authResp.state);
                break;
        }
    }
}

@end
