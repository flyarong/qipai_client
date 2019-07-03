//
//  WXApiManager.h
//  Unity-iPhone
//
//  Created by Apple on 2018/8/2.
//

#ifndef WXApiManager_h
#define WXApiManager_h


#endif /* WXApiManager_h */

#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>
#import "WXApi.h"
#import "WXApiObject.h"

@protocol WXAuthDelegate <NSObject>

@optional
- (void)wxAuthSucceed:(NSString*)code;
- (void)wxAuthDenied;
- (void)wxAuthCancel;

@end

@interface WXApiManager : NSObject <WXApiDelegate>

/**
 *  严格单例，唯一获得实例的方法.
 *
 *  @return 实例对象.
 */
+ (instancetype)sharedManager;
@end
