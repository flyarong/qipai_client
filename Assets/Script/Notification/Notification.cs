using System;
using UnityEngine;

namespace Notification
{
    public delegate void NotificationHandler(NotificationArg arg);

    public class Event
    {
        public string type;
        public NotificationArg arg;

        public Event(string _type, NotificationArg _arg)
        {
            type = _type;
            arg = _arg;
        }
    }

    public class NotificationArg
    {
        private object value;

        public NotificationArg(object v)
        {
            value = v;
        }

        public T GetValue<T>()
        {
            try
            {
                return (T)value;

            }
            catch (InvalidCastException ue)
            {
                Debug.Log(ue.ToString());
            }

            return default(T);
        }
    }
}