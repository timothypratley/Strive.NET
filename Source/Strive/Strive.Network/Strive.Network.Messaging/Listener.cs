using System;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Common.Logging;
using Strive.Network.Messages;

namespace Strive.Network.Messaging
{
    public class Listener
    {
        public HashSet<ClientConnection> Clients { get; private set; }
        Socket _tcpSocket;
        readonly IPEndPoint _localEndPoint;
        readonly ILog _log = LogManager.GetCurrentClassLogger();

        public Listener(IPEndPoint localEndPoint)
        {
            _localEndPoint = localEndPoint;
            Clients = new HashSet<ClientConnection>();
        }

        public void Start()
        {
            lock (this)
            {
                if (_tcpSocket != null)
                {
                    _log.Info("Restarting");
                    Stop();
                }
                Clients.Clear();

                try
                {
                    _tcpSocket = new Socket(_localEndPoint.Address.AddressFamily, SocketType.Stream,
                                            ProtocolType.Tcp);
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

                foreach (var c in Clients)
                    c.Close();
            }
        }

        public static void AcceptCallback(IAsyncResult ar)
        {
            try
            {
                // Get the socket that handles the client request.
                var listener = (Listener)ar.AsyncState;
                lock (listener)
                {
                    if (listener._tcpSocket == null)
                        return;

                    Socket socket;
                    try
                    {
                        socket = listener._tcpSocket.EndAccept(ar);
                    }
                    catch (ArgumentException)
                    {
                        // TODO: hmmm I really wish there was a nicer way
                        // this is the result of a previous binding, we discard it
                        return;
                    }

                    // Create the state object.
                    var client = new ClientConnection();
                    client.Start(socket);
                    listener.Clients.Add(client);
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
    }
}
