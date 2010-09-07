using System;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

using Common.Logging;

using Strive.Network.Messages;
using ToClient = Strive.Network.Messages.ToClient;
using ToServer = Strive.Network.Messages.ToServer;
using Strive.Math3D;
using Strive.Server.Model;
using Strive.Common;


namespace Strive.Network.Client
{
    public class ServerConnection
    {
        Queue messageQueue = new Queue();
        IPEndPoint remoteEndPoint;
        IPEndPoint endpoint = new IPEndPoint(IPAddress.Any, 0);
        Socket udpsocket;
        Socket tcpsocket;
        byte[] udpbuffer = new byte[MessageTypeMap.BufferSize];  // Receive buffer.
        byte[] tcpbuffer = new byte[MessageTypeMap.BufferSize];  // Receive buffer.
        int tcpoffset = 0;
        bool connected = false;
        public Strive.Network.Messages.NetworkProtocolType protocol;
        public bool isRunning = false;

        public delegate void OnConnectHandler();
        public delegate void OnConnectFailedHandler();
        public delegate void OnDisconnectHandler();
        public delegate void OnPositionSentHandler(ToClient.Position Position);

        public event OnPositionSentHandler OnPositionSent;
        public event OnConnectHandler OnConnect;
        public event OnConnectFailedHandler OnConnectFailed;
        public event OnDisconnectHandler OnDisconnect;
        public event EventHandler MessageRecieved;

        ILog Log;
        public ServerConnection()
        {
            Log = LogManager.GetCurrentClassLogger();
        }

        public class AlreadyRunningException : Exception { }
        public void Start(IPEndPoint remoteEndPoint)
        {
            if (isRunning) throw new AlreadyRunningException();
            isRunning = true;
            this.remoteEndPoint = remoteEndPoint;
            messageQueue.Clear();
            tcpoffset = 0;

            // Connect to the remote endpoint.
            tcpsocket = new Socket(remoteEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            udpsocket = new Socket(remoteEndPoint.AddressFamily, SocketType.Dgram, ProtocolType.Udp);
            tcpsocket.BeginConnect(remoteEndPoint, new AsyncCallback(ConnectTCPCallback), this);
        }

        public void Stop()
        {
            if (tcpsocket != null)
            {
                if (tcpsocket.Connected)
                    tcpsocket.Shutdown(SocketShutdown.Both);
                tcpsocket = null;
            }
            if (udpsocket != null)
            {
                if (udpsocket.Connected)
                    udpsocket.Shutdown(SocketShutdown.Both);
                udpsocket.Close();
                udpsocket = null;
            }
            if (connected)
            {
                connected = false;
                if (OnDisconnect != null)
                {
                    OnDisconnect();
                }
            }
            isRunning = false;
        }

        private static void ConnectTCPCallback(IAsyncResult ar)
        {
            ServerConnection client = (ServerConnection)ar.AsyncState;
            try
            {

                // Complete the connection.
                client.tcpsocket.EndConnect(ar);
                client.connected = true;
                if (client.OnConnect != null)
                {
                    client.OnConnect();
                }

                // Begin reading.
                client.tcpsocket.BeginReceive(client.tcpbuffer, 0, MessageTypeMap.BufferSize, 0,
                    new AsyncCallback(ReceiveTCPCallback), client);

                client.udpsocket.Bind(client.tcpsocket.LocalEndPoint);
                client.udpsocket.BeginConnect(client.tcpsocket.RemoteEndPoint,
                    new AsyncCallback(ConnectUDPCallback), client);
            }
            catch (Exception e)
            {
                client.Log.Error("Error connecting, closing connection.", e);
                client.Stop();
                if (client.OnConnectFailed != null) client.OnConnectFailed();
            }
        }

        private static void ConnectUDPCallback(IAsyncResult ar)
        {
            ServerConnection client = (ServerConnection)ar.AsyncState;
            try
            {

                // Complete the connection.
                client.udpsocket.EndConnect(ar);

                // Begin reading.
                client.udpsocket.BeginReceive(client.udpbuffer, 0, MessageTypeMap.BufferSize, 0,
                    new AsyncCallback(ReceiveUDPCallback), client);
            }
            catch (Exception e)
            {
                client.Log.Error("Error connecting, closing connection.", e);
                client.Stop();
            }
        }

        private static void ReceiveTCPCallback(IAsyncResult ar)
        {
            // Retrieve the state object and the client socket 
            // from the async state object.
            ServerConnection client = (ServerConnection)ar.AsyncState;
            try
            {
                // Read data from the remote device.
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
                            client.Stop();
                            return;
                        }
                        client.messageQueue.Enqueue(message);
                        client.Log.Trace("enqueued " + message.GetType() + " message");

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

                // listen for the next message
                client.tcpsocket.BeginReceive(client.tcpbuffer, client.tcpoffset, MessageTypeMap.BufferSize - client.tcpoffset, 0,
                    new AsyncCallback(ReceiveTCPCallback), client);
            }
            catch (Exception e)
            {
                client.Log.Error("Error reading, closing connection.", e);
                client.Stop();
            }
        }

        private static void ReceiveUDPCallback(IAsyncResult ar)
        {
            // Retrieve the state object and the client socket 
            // from the async state object.
            ServerConnection client = (ServerConnection)ar.AsyncState;

            // TODO: is this wrong? shouldn't we endreceive?
            if (client.udpsocket == null) return;
            try
            {
                // Read data from the remote device.
                int bytesRead = client.udpsocket.EndReceive(ar);

                // There might be more data, so store the data received so far.
                IMessage message = (IMessage)CustomFormatter.Deserialize(client.udpbuffer, MessageTypeMap.MessageLengthLength);
                client.messageQueue.Enqueue(message);
                //Log.Info( "enqueued " + message.GetType() + " message" );

                // Get the next message
                client.udpsocket.BeginReceive(client.udpbuffer, 0, MessageTypeMap.BufferSize, 0,
                    new AsyncCallback(ReceiveUDPCallback), client);
            }
            catch (Exception e)
            {
                client.Log.Error("Error reading, closing connection.", e);
                client.Stop();
            }
        }

        public void Send(IMessage message)
        {

            if (!connected)
            {
                return;
            }
            try
            {
                switch (protocol)
                {
                    case Strive.Network.Messages.NetworkProtocolType.TcpOnly:
                        {
                            SendTCP(message);
                            break;
                        }
                    case Strive.Network.Messages.NetworkProtocolType.UdpAndTcp:
                        {
                            // TODO: some clients may not want to send UDP messages
                            if (message is ToServer.Position)
                            {
                                SendUDP(message);
                            }
                            else
                            {
                                SendTCP(message);
                            } break;
                        }
                }
            }
            catch (Exception e)
            {
                Log.Error("Error sending, closing connection", e);
                Stop();
            }
        }

        void SendTCP(IMessage message)
        {
            // Custom serialization
            byte[] buffer = CustomFormatter.Serialize(message);
            try
            {
                tcpsocket.BeginSend(buffer, 0, buffer.Length, 0,
                    new AsyncCallback(SendTCPCallback), this);
            }
            catch (Exception e)
            {
                Log.Error("Error sending, closing connection", e);
                Stop();
            }
        }

        private static void SendTCPCallback(IAsyncResult ar)
        {
            ServerConnection client = (ServerConnection)ar.AsyncState;
            try
            {
                // Complete sending the data to the remote device.
                int bytesSent = client.tcpsocket.EndSend(ar);
            }
            catch (Exception)
            {
                client.Stop();
            }
        }

        void SendUDP(IMessage message)
        {
            // Custom serialization
            byte[] buffer = CustomFormatter.Serialize(message);

            // NB: for UDP we don't need to send the message length,
            // as packets arrive as sent, not in a stream.
            int offset = MessageTypeMap.MessageLengthLength;
            try
            {
                udpsocket.BeginSend(buffer, offset, buffer.Length - offset, 0,
                    new AsyncCallback(SendUDPCallback), this);
            }
            catch (Exception)
            {
                Stop();
            }
        }

        private static void SendUDPCallback(IAsyncResult ar)
        {
            ServerConnection client = (ServerConnection)ar.AsyncState;
            try
            {
                // Complete sending the data to the remote device.
                int bytesSent = client.udpsocket.EndSend(ar);
            }
            catch (Exception)
            {
                client.Stop();
            }
        }

        public int MessageCount
        {
            get { return messageQueue.Count; }
        }

        public IMessage PopNextMessage()
        {
            return (IMessage)messageQueue.Dequeue();
        }

        #region Simple Message API

        public void Chat(string message)
        {
            Send(new ToServer.Communication(CommunicationType.Chat, message));
        }

        public void PossessMobile(int mobileId)
        {
            Send(new ToServer.EnterWorldAsMobile(mobileId));
        }

        public void Login(string username, string password, Strive.Network.Messages.NetworkProtocolType protocol)
        {
            Send(new ToServer.Login(username, password, protocol));
        }

        public void Logout()
        {
            Send(new ToServer.Logout());
        }

        public void SkillList()
        {
            Send(new ToServer.SkillList());
        }

        public void WhoList()
        {
            Send(new ToServer.WhoList());
        }

        public void UseSkill(EnumSkill Skill, int InvokationID)
        {
            Send(new ToServer.UseSkill(Skill, InvokationID));
        }

        public void UseSkill(EnumSkill Skill, int InvokationID, int[] Targets)
        {
            Send(new ToServer.UseSkill(Skill, InvokationID, Targets));
        }

        public void UseSkill(int SkillID, int InvokationID)
        {
            this.UseSkill((EnumSkill)SkillID, InvokationID);
        }

        public void UseSkill(int SkillID, int InvokationID, int[] Targets)
        {
            this.UseSkill((EnumSkill)SkillID, InvokationID, Targets);
        }

        public void Position(Vector3D position, Vector3D rotation)
        {
            Send(new ToServer.Position(position, rotation));
        }

        public void RequestPossessable()
        {
            Send(new ToServer.RequestPossessable());
        }

        public void Pong(int SequenceNumber)
        {
            Send(new ToServer.Pong(SequenceNumber));
        }

        public void ReloadWorld()
        {
            Send(new ToServer.ReloadWorld());
        }

        #endregion

    }
}
