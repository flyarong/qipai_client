using System;
using UnityEngine;

namespace Network
{

    public delegate void EventHandler(EventArg arg);

    public class Event
    {
        public string type;
        public EventArg arg;

        public Event(string _type, EventArg _arg)
        {
            type = _type;
            arg = _arg;
        }
    }

    public class EventArg
    {
        private object value;

        public EventArg(object v)
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