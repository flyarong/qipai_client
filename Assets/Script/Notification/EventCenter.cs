using System.Collections.Generic;
using UnityEngine;

namespace Network
{
    public class EventCenter
    {
        private static EventCenter inst;
        public static EventCenter Inst {
            get
            {
                if (inst == null)
                {
                    new EventCenter().Init();
                }
                return inst;
            }
        }

        private Dictionary<EventType, EventHandler> handlers = new Dictionary<EventType, EventHandler>();
        private object thisLock = new object();

        public void Init()
        {
            if (inst == null)
            {
                inst = this;
            }
        }

        public void AddEventListener(EventType type, EventHandler listener)
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

        public void PushEvent(EventType type, object arg)
        {
            lock (thisLock)
            {
                if (!handlers.ContainsKey(type))
                {
                    return;
                }

                if (handlers[type] != null)
                {
                    handlers[type](new EventArg(arg));
                }
            }
        }
    }
}