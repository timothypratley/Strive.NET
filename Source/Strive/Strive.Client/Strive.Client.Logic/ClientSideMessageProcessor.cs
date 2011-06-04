using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Reflection;
using Common.Logging;
using Microsoft.CSharp.RuntimeBinder;
using Strive.Model;
using Strive.Network.Messages.ToClient;
using Strive.Network.Messaging;


namespace Strive.Client.Logic
{
    public partial class ClientSideMessageProcessor
    {
        public History History { get; private set; }
        public TraceListenerCollection ChatListeners;
        public ServerConnection ServerConnection;

        readonly ILog _log = LogManager.GetCurrentClassLogger();

        public ClientSideMessageProcessor(ServerConnection connection, History history)
        {
            ServerConnection = connection;
            History = history;
            ServerConnection.MessageRecieved += ConnectionMessageRecieved;

            // I wish there were a public constructor for TraceListenerCollection, but there is not
            ConstructorInfo constructor = typeof(TraceListenerCollection).GetConstructor(
                BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[] { }, null);
            ChatListeners = (TraceListenerCollection)constructor.Invoke(new object[0] { });
        }

        public void ProcessMessages()
        {
            lock (ServerConnection)
            {
                try
                {
                    CheckAndProcessMessage(ServerConnection.PopNextMessage());
                }
                catch (Exception e)
                {
                    _log.Error("Message was not processed correctly, resuming.", e);
                }
            }
        }

        public void CheckAndProcessMessage(dynamic message)
        {
            Contract.Requires<ArgumentNullException>(message != null);

            try
            {
                Process(message);
                _log.Trace("Processed message " + message);
            }
            catch
            {
                _log.Warn("ERROR: Unable to process message " + message);
            }
        }

        void ConnectionMessageRecieved(object sender, EventArgs e)
        {
            dynamic m = ServerConnection.PopNextMessage();
            _log.Trace("Processing " + m.GetType() + " message: " + m);
            try
            {
                Process(m);
            }
            catch (RuntimeBinderException)
            {
                _log.Error("Received unknown message type " + m);
            }
            catch (Exception ex)
            {
                _log.Error("Failed to process message " + m, ex);
            }
        }

        void Process(TimeAndWeather m)
        {
            _log.Info("Received Time and Weather " + m.ServerNow);
        }

        void Process(EntityModel m)
        {
            History.Add(m);
        }

        void Process(TaskModel m)
        {
            History.Add(m);
        }

        void Process(MissionModel m)
        {
            History.Add(m);
        }

        void Process(PositionUpdate m)
        {
            History.Move(m.Id, m.State, m.Position, m.Rotation, DateTime.Now);
        }

        void Process(MobileState m)
        {
            EntityModel e = History.GetEntity(m.ObjectInstanceId);
            if (e == null)
                _log.Error("Could not find entity " + m.ObjectInstanceId + " to update.");
            History.Add(e.WithState(m.State));
        }

        void Process(Communication m)
        {
            foreach (TraceListener listener in ChatListeners)
                listener.WriteLine("[" + m.CommunicationType + "] " + m.Name + ": " + m.Message);
        }

        void Process(LogMessage m)
        {
            _log.Info(m.Message);
        }
    }
}
