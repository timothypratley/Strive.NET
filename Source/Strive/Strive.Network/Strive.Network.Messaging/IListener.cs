﻿using System;
using System.Collections.Generic;
using Strive.Network.Messages;

namespace Strive.Network.Messaging
{
    public interface IListener
    {
        List<ClientConnection> Clients { get; }
        void SendToAll(object message);
        void Start();
        void Stop();
    }
}