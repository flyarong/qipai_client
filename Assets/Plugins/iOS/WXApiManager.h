
//
//  WXApiManager.h
//  Unity-iPhone
//
//  Created by Apple on 2018/8/2.
//

#ifndef WXApiManager_h
#define WXApiManager_h

#import <UIKit/UIKit.h>
#import "WXApi.h"
#import "WXApiObject.h"

#import <Foundation/Foundation.h>

@interface WXApiManager : NSObject

#ifdef __cplusplus

extern "C" {
#endif
    void RegToWechat(const char * appId);
    void LoginWeChat();
//    void shareImg(const char * path);
//    void shareText(const char * text);
#ifdef __cplusplus
}
#endif
@end
#endif /* WXApiManager_h */
