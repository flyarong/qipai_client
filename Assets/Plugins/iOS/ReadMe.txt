
//CocoaPods安装及使用自行百度

通过CocoaPods集成集成微信库文件：
   1，打开终端
   2，cd 文件路径
   3，终端输入：$ touch Podfile
   4，终端输入：$ vim Podfile
进来之后紧接着按键盘上的英文'i'键 下面的"Podsfile" 0L, 0C将变成-- INSERT    -- 然后就可以编辑文字了，输入以下文字
platform :ios, ‘7.0’
target 'Unity-iPhone' do
pod 'AFNetworking', '~> 3.1.0'
pod 'WechatOpenSDK'
end
此时该退出去了，先按左上角的esc键，再按：键，再输入wq，点击回车，就保存并退出去了。
终端输入：$ pod install
这个时候关闭所有的Xcode窗口，再次打开工程目录会看到多了一个后缀名为.xcworkspace文件。双击打开编辑xcode
//
配置微信
1，找到UnityAppController.mm 
   添加  #import <WXApi.h>  #import "WXApiManager.h"
   找到 application didFinishLaunchingWithOptions
    添加：
     //注册微信id
    [WXApi registerApp:@"你的微信注册ID"];
   重写AppDelegate的handleOpenURL和openURL方法：
   return  [WXApi handleOpenURL:url delegate:[WXApiManager sharedManager]];

2，找到url type设置 “你的微信注册ID”