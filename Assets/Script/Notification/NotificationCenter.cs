using System.Collections.Generic;
using UnityEngine;

namespace Notification
{
    public class NotificationCenter
    {
        private static NotificationCenter inst;
        public static NotificationCenter Inst {
            get
            {
                if (inst == null)
                {
                    new NotificationCenter().Init();
                }
                return inst;
            }
            set
            {
                inst = value;
            }
        }

        private Dictionary<NotificationType, NotificationHandler> handlers = new Dictionary<NotificationType, NotificationHandler>();
        private object thisLock = new object();

        public void Init()
        {
            if (inst == null)
            {
                inst = this;
            }
        }

        public void AddEventListener(NotificationType type, NotificationHandler listener)
        {
            if (handlers.ContainsKey(type))
            {
                handlers[type] += listener;
            }
            else
            {
                handlers.Add(type, listener);
            }
        }

        public void PushEvent(NotificationType type, object arg)
        {
            lock (thisLock)
            {
                if (!handlers.ContainsKey(type))
                {
                    return;
                }

                if (handlers[type] != null)
                {
                    handlers[type](new NotificationArg(arg));
                }
            }
        }
    }
}