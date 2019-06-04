using Notification;
namespace Network
{

    using System;
    using System.Collections.Generic;
    using Msg;

    public delegate void MessageHandler(Message msg);

    public class Manager
    {
        private static Manager inst;
        public static Manager Inst
        {
            get
            {
                if (inst == null)
                {
                    new Manager().Init();
                    inst.Connect();
                }
                return inst;
            }
        }

        public string host = "192.168.1.103";
        public int port = 8899;

        public Queue<Message> m_msgQueue = new Queue<Message>();
        // for queue
        private object thisLock = new object();

        private GameSocket m_gameSocket;

        public void Init()
        {
            inst = this;
            m_gameSocket = new GameSocket(OnConnected, OnDisconnect, OnMessage);
        }

        public void HandleMessage(MessageHandler handle)
        {
            lock (thisLock)
            {
                if (m_msgQueue.Count > 0)
                {
                    Message msg = m_msgQueue.Dequeue();
                    handle(msg);
                }
            }
        }

        /// <summary>
        /// Connect to server.
        /// </summary>
        public void Connect()
        {
            if (m_gameSocket == null)
            {
                return;
            }
            TimeSpan timeout = new TimeSpan(0, 0, 5);
            m_gameSocket.Connect(host, port, timeout);
        }

        /// <summary>
        /// Sends message.
        /// </summary>
        /// <param name="msg">Message.</param>
        public void SendMessage(Message msg)
        {
            if (m_gameSocket != null)
            {
                m_gameSocket.PushMessage(msg);
            }
        }

        public void OnConnected(Message msg)
        {
            NotificationCenter.Inst.PushEvent(NotificationType.Network_OnConnected, null);
        }

        public void OnDisconnect(Message msg)
        {
            m_gameSocket.Reset();
            NotificationCenter.Inst.PushEvent(NotificationType.Network_OnDisconnected, null);
        }

        public void OnMessage(Message msg)
        {
            lock (thisLock)
            {
                m_msgQueue.Enqueue(msg);
            }
        }

        // 关闭连接
        private void Close()
        {
            m_gameSocket.Reset();
        }
    }
}