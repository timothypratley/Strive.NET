using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Threading;
using System.IO;

using Strive.Network.Messages;
using Common.Logging;

namespace Strive.Network.Server
{
    /// <summary>
    /// Summary description for Listener.
    /// </summary>
    public class Listener
    {
        public List<Client> Clients { get; private set; }
        public Queue<IMessage> _clientMessageQueue = new Queue<IMessage>();
        Socket _tcpSocket;
        IPEndPoint _localEndPoint;
        EndPoint _remoteEndPoint;
        byte[] _udpBuffer = new byte[MessageTypeMap.BufferSize]; // Receive buffer.
        ILog _log;

        public Listener(IPEndPoint localEndPoint)
        {
            _localEndPoint = localEndPoint;
            _remoteEndPoint = new IPEndPoint(localEndPoint.AddressFamily == AddressFamily.InterNetworkV6 ? IPAddress.IPv6Any : IPAddress.Any, 0);
            _log = LogManager.GetCurrentClassLogger();
        }

        public void Start()
        {
            Clients = new List<Client>();
            try
            {
                _tcpSocket = new Socket(_localEndPoint.Address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                _tcpSocket.Bind(_localEndPoint);
                _tcpSocket.Listen(10);
            }
            catch (Exception e)
            {
                _log.Error("Unable to start listening", e);
                return;
            }

            try
            {
                _tcpSocket.BeginAccept(new AsyncCallback(acceptCallback), this);
                _log.Info("Started listening on " + _localEndPoint);
            }
            catch (ObjectDisposedException)
            {
                // the underlying socket was closed
            }

        }

        public void Stop()
        {
            if (_tcpSocket != null)
            {
                _tcpSocket.Close();
                _tcpSocket = null;
            }
            _log.Info("Stopped listening on" + _localEndPoint);
        }

        public static void acceptCallback(IAsyncResult ar)
        {
            try
            {
                // Get the socket that handles the client request.
                Listener handler = (Listener)ar.AsyncState;
                Socket clientSocket = handler._tcpSocket.EndAccept(ar);

                // Create the state object.
                Client client = new Client();
                handler.Clients.Add(client);
                handler._log.Info("New connection from " + client.RemoteEndPoint);
                client.Start(clientSocket);

                // The next connection
                handler._tcpSocket.BeginAccept(
                    new AsyncCallback(acceptCallback),
                    handler);
            }
            catch (ObjectDisposedException)
            {
                // the underlying socket was closed
            }
        }

        public int MessageCount
        {
            get { return _clientMessageQueue.Count; }
        }

        public ClientMessage PopNextMessage()
        {
            return (ClientMessage)_clientMessageQueue.Dequeue();
        }

        public void SendToAll(IMessage message)
        {
            foreach (Client c in Clients)
            {
                if (c.Authenticated)
                {
                    c.Send(message);
                }
            }
        }
    }
}
