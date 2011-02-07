using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Generic;

using Common.Logging;

using Strive.Server.Model;
using Strive.Network.Messages;
using Strive.Network.Messages.ToClient;


namespace Strive.Network.Server
{
    public class Client : Connection
    {
        Mobile avatar = null;
        public int PlayerID = 0;

        DateTime lastMessageTimestamp;
        public int latency = 0;
        public DateTime pingedAt = DateTime.MinValue;
        public int pingSequence = -1;

        public string AuthenticatedUsername { get; set; }
        public bool Authenticated { get { return AuthenticatedUsername != null; } }

        public void Send(IMessage message)
        {
            if (!Authenticated)
            {
                log.Error("Trying to send message " + message + " without authenticated connection.");
                return;
            }
            base.Send(message);
        }

        public void Close()
        {
            base.Stop();
            AuthenticatedUsername = null;
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

        public EndPoint RemoteEndPoint
        {
            get { return tcpSocket == null ? null : tcpSocket.RemoteEndPoint; }
        }

        #region MessageSending
        public void Log(string message)
        {
            Send(new LogMessage(message));
        }

        public void Ping()
        {
            pingSequence++;
            Send(new Ping(pingSequence));
            pingedAt = DateTime.Now;
        }

        public void CanPossess(Tuple<int, string>[] possessable)
        {
            Send(new CanPossess(possessable));
        }

        public void DropAll()
        {
            Send(new DropAll());
        }

        public void WhoList(Tuple<int,string>[] mobiles)
        {
            Send(new WhoList(mobiles));
        }

        public void SkillList(Tuple<int,double>[] competancy)
        {
            Send(new SkillList(competancy));
        }

        public void Communication(string name, string message, CommunicationType communicationType)
        {
            Send(new Communication(name, message, communicationType));
        }
        #endregion
    }
}
