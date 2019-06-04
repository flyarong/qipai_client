using UnityEngine;
using System.Collections;
using Network;
using Network.Msg;
using System.Collections.Generic;
using System;
using System.Reflection;
using UnityEngine.SceneManagement;
using Notification;

namespace Utils
{

    public static class Handler
    {

        private static Dictionary<MsgID, KeyValuePair<Notification.NotificationType, string>> handlers = new Dictionary<MsgID, KeyValuePair<Notification.NotificationType, string>>();
        public static void HandleMessage()
        {
            Manager.Inst.HandleMessage(HandleMessage);
        }
        private static void HandleMessage(Message msg)
        {
            var msgID = (MsgID)msg.MessageID;

            // 没有权限，统一退回到登录界面
            if(msgID == MsgID.NoPermission)
            {
                SceneManager.LoadScene("Login");
                return;
            }
            
            if (!handlers.ContainsKey(msgID))
            {
                Debug.LogWarning("存在没有处理的消息，编号为：" + msgID);
                return;
            }

            var kv = handlers[msgID];
            
            Type type = Type.GetType(kv.Value);
            object obj = type.Assembly.CreateInstance(kv.Value);

            MethodInfo method = type.GetMethod("FromMessage", new Type[] { typeof(Message) });
            object[] parameters = new object[] { msg };
            method.Invoke(obj, parameters);

            NotificationCenter.Inst.PushEvent(kv.Key, obj);
        }

        /// <summary>
        /// 初始化时间处理器和消息字典，必须在Awake函数一开始调用
        /// </summary>
        public static void Init()
        {
            handlers.Clear();
            NotificationCenter.Inst = null;
        }

        /// <summary>
        /// 添加消息和通知类型以及处理数据类型之间的关联
        /// </summary>
        /// <typeparam name="T">用什么类型接收服务端返回的数据</typeparam>
        /// <param name="msgId">消息类型编号</param>
        /// <param name="type">msgId消息和什么通知关联</param>
        public static void Add<T>(MsgID msgId, Notification.NotificationType type) where T : new()
        {
            handlers.Add(msgId, new KeyValuePair<Notification.NotificationType, string>(type, new T().GetType().FullName));
        }

        public static void AddListenner(NotificationType type, Notification.NotificationHandler listener)
        {
            NotificationCenter.Inst.AddEventListener(type, listener);
        }
    }
}
