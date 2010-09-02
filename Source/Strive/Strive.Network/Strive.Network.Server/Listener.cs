using System;
using System.Net;
using System.Net.Sockets;
using System.Collections;
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
        Hashtable clients = new Hashtable();
        // TODO: refactor don't expose underlying queue
        public Queue clientMessageQueue = new Queue();
        public Socket _udpSocket;
        Socket _tcpSocket;
        IPEndPoint _localEndPoint;
        EndPoint _remoteEndPoint;
        byte[] _udpBuffer = new byte[MessageTypeMap.BufferSize]; // Receive buffer.

        ILog Log;
        public Listener(IPEndPoint localEndPoint)
        {
            _localEndPoint = localEndPoint;
            _remoteEndPoint = new IPEndPoint(localEndPoint.AddressFamily == AddressFamily.InterNetworkV6 ? IPAddress.IPv6Any : IPAddress.Any, 0);
            Log = LogManager.GetCurrentClassLogger();
        }

        public void Start()
        {
            try
            {
                _tcpSocket = new Socket(_localEndPoint.Address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                _tcpSocket.Bind(_localEndPoint);
                _tcpSocket.Listen(10);
                _tcpSocket.BeginAccept(new AsyncCallback(acceptCallback), this);

                _udpSocket = new Socket(_localEndPoint.Address.AddressFamily, SocketType.Dgram, ProtocolType.Udp);
                _udpSocket.Bind(_localEndPoint);

                // Begin reading udp packets
                _udpSocket.BeginReceiveFrom(_udpBuffer, 0, MessageTypeMap.BufferSize, 0, ref _remoteEndPoint,
                    new AsyncCallback(ReceiveFromUDPCallback), this);

                Log.Info("Started listening on" + _localEndPoint);
            }
            catch (ObjectDisposedException)
            {
                Log.Info("The underlying socket was closed");
            }
        }

        public static void ReceiveFromUDPCallback(IAsyncResult ar)
        {
            try
            {
                Listener handler = (Listener)ar.AsyncState;
                int bytesRead = handler._udpSocket.EndReceiveFrom(ar, ref handler._remoteEndPoint);
                Client client = handler.clients[handler._remoteEndPoint] as Client;
                if (client == null || !client.Authenticated)
                {
                    // ignore the packet
                    return;
                }
                if (bytesRead == MessageTypeMap.BufferSize)
                {
                    throw new Exception("Reached max buffer size, increase this limit.");
                }

                IMessage message;
                try
                {
                    message = (IMessage)CustomFormatter.Deserialize(handler._udpBuffer, 0);
                }
                catch (Exception e)
                {
                    handler.Log.Error("Invalid packet received", e);
                    return;
                }
                client.LastMessageTimestamp = DateTime.Now;
                ClientMessage clientMessage = new ClientMessage(client, message);
                // TODO: ensure threadsafe access to queue
                handler.clientMessageQueue.Enqueue(clientMessage);

                handler._udpSocket.BeginReceiveFrom(handler._udpBuffer, 0, MessageTypeMap.BufferSize, 0, ref handler._remoteEndPoint,
                    new AsyncCallback(ReceiveFromUDPCallback), handler);
            }
            catch (Exception)
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
            if (_udpSocket != null)
            {
                _udpSocket.Close();
                _udpSocket = null;
            }
            Log.Info("Stopped listening on" + _localEndPoint);
        }

        public static void acceptCallback(IAsyncResult ar)
        {
            try
            {
                // Get the socket that handles the client request.
                Listener handler = (Listener)ar.AsyncState;
                Socket clientsocket = handler._tcpSocket.EndAccept(ar);

                // Create the state object.
                Client client = new Client(clientsocket, handler);
                handler.clients.Add(client.EndPoint, client);
                handler.Log.Info("New connection from " + client.EndPoint);

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
            get { return clientMessageQueue.Count; }
        }

        public ClientMessage PopNextMessage()
        {
            return (ClientMessage)clientMessageQueue.Dequeue();
        }

        public Hashtable Clients
        {
            get { return clients; }
        }


        public void SendToAll(IMessage message)
        {
            foreach (Client c in clients.Values)
            {
                if (c.Authenticated)
                {
                    c.Send(message);
                }
            }
        }
    }
}
