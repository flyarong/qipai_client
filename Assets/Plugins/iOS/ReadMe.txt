
1，找到UnityAppController.mm 
   添加  #import <WXApi.h>  #import "WXApiManager.h"
   找到 application didFinishLaunchingWithOptions
    添加：
     //注册微信id
    [WXApi registerApp:@"你的微信注册ID"];
   重写AppDelegate的handleOpenURL和openURL方法：
   return  [WXApi handleOpenURL:url delegate:[WXApiManager sharedManager]];

2，找到url type设置 “你的微信注册ID”
快捷操作：复制插件目录下的wechat.plist中的CFBundleURLTypes健 粘贴到Info.plist中

3. 添加链接参数，否则会报错。 Build Settings -> Linking -> Other Linker Flags 添加两个选项 -ObjC -all_load