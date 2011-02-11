using System;
using System.Net;
using Strive.Server.Model;
using Strive.Network.Messages;
using Strive.Network.Messages.ToClient;


namespace Strive.Network.Messaging
{
    public class ClientConnection : Connection
    {
        public int PlayerId;

        public int Latency;
        public DateTime PingedAt;
        public int PingSequence = -1;

        public string AuthenticatedUsername { get; set; }
        public bool Authenticated { get { return AuthenticatedUsername != null; } }

        public override bool Send(IMessage message)
        {
            if (!Authenticated)
            {
                Log.Error("Trying to send message " + message + " without authenticated connection.");
                return false;
            }
            return base.Send(message);
        }

        public void Close()
        {
            Stop();
            AuthenticatedUsername = null;
        }

        public Mobile Avatar { get; set; }
        public DateTime LastMessageTimestamp { get; set; }

        public EndPoint RemoteEndPoint
        {
            get { return TcpSocket == null ? null : TcpSocket.RemoteEndPoint; }
        }

        #region MessageSending
        public bool LogMessage(string message)
        {
            return Send(new LogMessage(message));
        }

        public bool Ping()
        {
            PingSequence++;
            if (Send(new Ping(PingSequence)))
            {
                PingedAt = DateTime.Now;
                return true;
            }
            return false;
        }

        public bool CanPossess(Tuple<int, string>[] possessable)
        {
            return Send(new CanPossess(possessable));
        }

        public bool DropAll()
        {
            return Send(new DropAll());
        }

        public bool WhoList(Tuple<int,string>[] mobiles)
        {
            return Send(new WhoList(mobiles));
        }

        public bool SkillList(Tuple<int,double>[] competancy)
        {
            return Send(new SkillList(competancy));
        }

        public bool Communication(string name, string message, CommunicationType communicationType)
        {
            return Send(new Communication(name, message, communicationType));
        }
        #endregion
    }
}
