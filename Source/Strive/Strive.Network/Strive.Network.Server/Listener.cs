using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
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
        // TODO: hook up to the right queues!
        public Queue<IMessage> ClientMessageQueue = new Queue<IMessage>();
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
                _tcpSocket.BeginAccept(new AsyncCallback(AcceptCallback), this);
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

        public static void AcceptCallback(IAsyncResult ar)
        {
            try
            {
                // Get the socket that handles the client request.
                var handler = (Listener)ar.AsyncState;
                Socket clientSocket = handler._tcpSocket.EndAccept(ar);

                // Create the state object.
                var client = new Client();
                handler.Clients.Add(client);
                handler._log.Info("New connection from " + client.RemoteEndPoint);
                client.Start(clientSocket);

                // The next connection
                handler._tcpSocket.BeginAccept(
                    new AsyncCallback(AcceptCallback),
                    handler);
            }
            catch (ObjectDisposedException)
            {
                // the underlying socket was closed
            }
        }

        public int MessageCount
        {
            get { return ClientMessageQueue.Count; }
        }

        public ClientMessage PopNextMessage()
        {
            return (ClientMessage)ClientMessageQueue.Dequeue();
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
