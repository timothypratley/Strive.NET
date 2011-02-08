using System;
using System.Net;
using Strive.Server.Model;
using Strive.Network.Messages;
using Strive.Network.Messages.ToClient;


namespace Strive.Network.Server
{
    public class Client : Connection
    {
        Mobile _avatar;
        public int PlayerId;

        DateTime _lastMessageTimestamp;
        public int Latency;
        public DateTime PingedAt;
        public int PingSequence = -1;

        public string AuthenticatedUsername { get; set; }
        public bool Authenticated { get { return AuthenticatedUsername != null; } }

        public override void Send(IMessage message)
        {
            if (!Authenticated)
            {
                Log.Error("Trying to send message " + message + " without authenticated connection.");
                return;
            }
            base.Send(message);
        }

        public void Close()
        {
            Stop();
            AuthenticatedUsername = null;
        }

        public Mobile Avatar
        {
            get { return _avatar; }
            set { _avatar = value; }
        }

        public DateTime LastMessageTimestamp
        {
            get { return _lastMessageTimestamp; }
            set { _lastMessageTimestamp = value; }
        }

        public EndPoint RemoteEndPoint
        {
            get { return TcpSocket == null ? null : TcpSocket.RemoteEndPoint; }
        }

        #region MessageSending
        public void LogMessage(string message)
        {
            Send(new LogMessage(message));
        }

        public void Ping()
        {
            PingSequence++;
            Send(new Ping(PingSequence));
            PingedAt = DateTime.Now;
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
