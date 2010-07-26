using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections;

using Common.Logging;
using Strive.Server.Model;
using Strive.Network.Messages;
using ToClient = Strive.Network.Messages.ToClient;
using ToServer = Strive.Network.Messages.ToServer;

namespace Strive.Network.Server
{
    /// <summary>
    /// Summary description for Client.
    /// </summary>
    public class Client
    {
        Mobile avatar = null;
        string authenticatedUsername = null;
        public int PlayerID = 0;
        Socket tcpsocket;
        byte[] tcpbuffer = new byte[MessageTypeMap.BufferSize]; // Receive buffer.
        int tcpoffset = 0;
        DateTime lastMessageTimestamp;
        Listener handler;
        EndPoint remoteEndPoint;
        Strive.Network.Messages.NetworkProtocolType protocol;
        public int latency = 0;
        public DateTime pingedAt = DateTime.MinValue;
        public int pingSequence = -1;
        ILog Log;

        public Client(Socket tcpsocket, Listener handler)
        {
            this.tcpsocket = tcpsocket;
            this.handler = handler;
            this.remoteEndPoint = tcpsocket.RemoteEndPoint;
            Log = LogManager.GetCurrentClassLogger();

            // Begin Reading.
            try
            {
                tcpsocket.BeginReceive(tcpbuffer, 0, MessageTypeMap.BufferSize, 0,
                    new AsyncCallback(ReadTCPCallback), this);
            }
            catch (Exception)
            {
                Close();
            }
        }

        public static void ReadTCPCallback(IAsyncResult ar)
        {
            Client client = (Client)ar.AsyncState;
            try
            {
                int bytesRead = client.tcpsocket.EndReceive(ar);
                client.tcpoffset += bytesRead;

                if (client.tcpoffset > MessageTypeMap.MessageLengthLength)
                {
                    int expected_length = BitConverter.ToInt32(client.tcpbuffer, 0);

                    while (client.tcpoffset >= expected_length)
                    {
                        IMessage message;
                        try
                        {
                            message = (IMessage)CustomFormatter.Deserialize(client.tcpbuffer, MessageTypeMap.MessageLengthLength);
                        }
                        catch (Exception e)
                        {
                            client.Log.Error("Invalid packet received, closing connection.", e);
                            client.Close();
                            return;
                        }
                        // TODO: use global now instead?
                        client.LastMessageTimestamp = DateTime.Now;
                        ClientMessage clientMessage = new ClientMessage(client, message);
                        // TODO: ensure threadsafe access to queue
                        client.handler.clientMessageQueue.Enqueue(clientMessage);
                        //Log.Info( "enqueued " + message.GetType() + " message" );

                        // copy the remaining data to the front of the buffer
                        client.tcpoffset -= expected_length;
                        for (int i = 0; i < client.tcpoffset; i++)
                        {
                            client.tcpbuffer[i] = client.tcpbuffer[i + expected_length];
                        }
                        if (client.tcpoffset > MessageTypeMap.MessageLengthLength)
                        {
                            expected_length = BitConverter.ToInt32(client.tcpbuffer, 0);
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                client.tcpsocket.BeginReceive(client.tcpbuffer, client.tcpoffset, MessageTypeMap.BufferSize - client.tcpoffset, 0,
                    new AsyncCallback(ReadTCPCallback), client);
            }
            catch (Exception)
            {
                // the underlying socket was closed
                client.Close();
            }
        }

        public bool Authenticated
        {
            get { return authenticatedUsername != null; }
        }

        public string AuthenticatedUsername
        {
            get { return authenticatedUsername; }
            set
            {
                authenticatedUsername = value;
            }
        }

        public Strive.Network.Messages.NetworkProtocolType Protocol
        {
            get
            {
                return protocol;
            }
            set
            {
                protocol = value;
            }
        }

        public void Send(IMessage message)
        {
            // TODO: some clients may prefer no UDP
            if (!Authenticated)
            {
                //Log.ErrorMessage( "Trying to send message without authenticated connection." );
                return;
            }

            // check if client allows UDP:
            switch (protocol)
            {
                case Strive.Network.Messages.NetworkProtocolType.TcpOnly:
                    {
                        SendTCP(message);
                        break;
                    }
                case Strive.Network.Messages.NetworkProtocolType.UdpAndTcp:
                    {
                        if (message is ToClient.Position)
                        {
                            SendUDP(message);
                        }
                        else
                        {
                            SendTCP(message);
                        }
                        break;
                    }
            }

        }

        void SendTCP(IMessage message)
        {
            // Custom serialization
            byte[] EncodedMessage = CustomFormatter.Serialize(message);

            // Begin sending the data to the remote device.
            try
            {
                tcpsocket.BeginSend(EncodedMessage, 0, EncodedMessage.Length, 0,
                    new AsyncCallback(SendTCPCallback), this);
            }
            catch (Exception)
            {
                // the underlying socket was closed
                Close();
            }
        }

        private static void SendTCPCallback(IAsyncResult ar)
        {
            // Retrieve the socket from the state object.
            Client client = (Client)ar.AsyncState;
            try
            {
                // Complete sending the data to the remote device.
                int bytesSent = client.tcpsocket.EndSend(ar);
            }
            catch (Exception)
            {
                // the underlying socket was closed
                client.Close();
            }
        }

        void SendUDP(IMessage message)
        {
            // Custom serialization
            byte[] EncodedMessage = CustomFormatter.Serialize(message);

            // Begin sending the data to the remote device.
            try
            {
                handler.udpsocket.BeginSendTo(EncodedMessage, 0, EncodedMessage.Length, 0,
                    tcpsocket.RemoteEndPoint, new AsyncCallback(SendToUDPCallback), this);
            }
            catch (Exception)
            {
                // the underlying socket was closed
                Close();
            }
        }

        private static void SendToUDPCallback(IAsyncResult ar)
        {
            // Retrieve the socket from the state object.
            Client client = (Client)ar.AsyncState;
            try
            {
                // Complete sending the data to the remote device.
                int bytesSent = client.handler.udpsocket.EndSendTo(ar);
            }
            catch (Exception)
            {
                // underlying socket was closed
                client.Close();
            }
        }

        public void Close()
        {
            if (tcpsocket != null)
            {
                Log.Info("Closing connection to " + EndPoint + ".");
                tcpsocket.Shutdown(SocketShutdown.Both);
                tcpsocket.Close();
                tcpsocket = null;
            }
            authenticatedUsername = null;
            handler.Clients.Remove(this);
        }

        public bool Active
        {
            get { return tcpsocket != null; }
        }

        public Mobile Avatar
        {
            get { return avatar; }
            set { avatar = value; }
        }

        public DateTime LastMessageTimestamp
        {
            get { return lastMessageTimestamp; }
            set { lastMessageTimestamp = value; }
        }

        public EndPoint EndPoint
        {
            get { return remoteEndPoint; }
        }

        public void SendLog(string message)
        {
            Send(new ToClient.LogMessage(message));
        }

        public void Ping()
        {
            pingSequence++;
            Send(new ToClient.Ping(pingSequence));
            pingedAt = DateTime.Now;
        }
    }
}
