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

#pragma mark - Public Methods
void LoginWeiCha()
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
extern "C"
{
    void WechatLogin()
    {
        NSLog(@"ios微信登陆");
        //构造SendAuthReq结构体
        SendAuthReq* req    =[[SendAuthReq alloc]init];
        req.scope           = @"snsapi_userinfo";
        req.state           = @"123";
        // req.openID          = kAuthOpenID;
        //第三方向微信终端发送一个SendAuthReq消息结构
        [WXApi sendReq:req];
    }
    
    void ShareText(const char *text)
    {
        SendMessageToWXReq *req = [[SendMessageToWXReq alloc] init];
        req.bText = YES;
        req.text = [NSString stringWithUTF8String:text];
        req.scene = WXSceneSession;
        [WXApi sendReq:req];
    }
    
    void ShareImage(const char *path)
    {
        NSLog(@"图片分享开始：%s\n", path);
        UIImage *image = [UIImage imageNamed:[NSString stringWithUTF8String:path]];
        NSData* imageData = UIImageJPEGRepresentation(image, 0.3);
        
        WXImageObject *imageObject = [WXImageObject object];
        imageObject.imageData = imageData;
        
        WXMediaMessage *message = [WXMediaMessage message];
        NSString *filePath = [[NSBundle mainBundle] pathForResource:@"res5"
                                                             ofType:@"jpg"];
        message.thumbData = [NSData dataWithContentsOfFile:filePath];
        message.mediaObject = imageObject;
        
        SendMessageToWXReq *req = [[SendMessageToWXReq alloc] init];
        req.bText = NO;
        req.message = message;
        req.scene = WXSceneSession;
        NSLog(@"图片分享结果：%d\n",[WXApi sendReq:req]);
    }
}

#pragma mark - WXApiDelegate
-(void)onReq:(BaseReq*)req {
    // just leave it here, WeChat will not call our app
    NSLog(@"req login\n");
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
