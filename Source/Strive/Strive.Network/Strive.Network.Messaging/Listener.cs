using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using Strive.Network.Messages;
using Common.Logging;

namespace Strive.Network.Messaging
{
    public class Listener
    {
        public List<Client> Clients { get; private set; }
        Socket _tcpSocket;
        readonly IPEndPoint _localEndPoint;
        readonly ILog _log;

        public Listener(IPEndPoint localEndPoint)
        {
            _localEndPoint = localEndPoint;
            _log = LogManager.GetCurrentClassLogger();
        }

        public void Start()
        {
            Clients = new List<Client>();
            try
            {
                lock (this)
                {
                    _tcpSocket = new Socket(_localEndPoint.Address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                    _tcpSocket.Bind(_localEndPoint);
                    _tcpSocket.Listen(10);
                }
            }
            catch (Exception e)
            {
                _log.Error("Unable to start listening", e);
                return;
            }

            try
            {
                lock (this)
                {
                    _tcpSocket.BeginAccept(new AsyncCallback(AcceptCallback), this);
                    _log.Info("Started listening on " + _localEndPoint);
                }
            }
            catch (ObjectDisposedException)
            {
                // the underlying socket was closed
            }

        }

        public void Stop()
        {
            lock (this)
            {
                if (_tcpSocket != null)
                {
                    _tcpSocket.Close();
                    _tcpSocket = null;
                }
                _log.Info("Stopped listening on" + _localEndPoint);
            }
        }

        public static void AcceptCallback(IAsyncResult ar)
        {
            try
            {
                // Get the socket that handles the client request.
                var listener = (Listener) ar.AsyncState;
                lock (listener)
                {
                    if (listener._tcpSocket == null) return;

                    // Create the state object.
                    var client = new Client();
                    client.Start(listener._tcpSocket.EndAccept(ar));
                    lock (listener.Clients)
                    {
                        listener.Clients.Add(client);
                    }
                    listener._log.Info("New connection from " + client.RemoteEndPoint);

                    // The next connection
                    listener._tcpSocket.BeginAccept(
                        new AsyncCallback(AcceptCallback),
                        listener);
                }
            }
            catch (ObjectDisposedException)
            {
                // the underlying socket was closed
            }
        }

        public void SendToAll(IMessage message)
        {
            lock (Clients)
            {
                foreach (Client c in Clients.Where(c => c.Authenticated))
                {
                    c.Send(message);
                }
            }
        }
    }
}
