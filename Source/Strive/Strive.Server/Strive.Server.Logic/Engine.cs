using System;
using System.Linq;
using System.Net;
using System.Collections.Generic;
using System.Reflection;

using Common.Logging;
using Strive.Common;
using Strive.Network.Messaging;
using Strive.Server.Model;
using Strive.Network.Messages;

namespace Strive.Server.Logic
{
    public class Engine
    {
        int port = Constants.DefaultPort;
        readonly Listener _listener;
        World _world;
        MessageProcessor _messageProcessor;
        readonly StoppableThread _engineThread;
        readonly ILog _log = LogManager.GetCurrentClassLogger();
        
        public ServerStatusModel ServerStatusModel { get; private set; }

        public Engine()
        {
            _log.Info("Creating " + Assembly.GetExecutingAssembly().GetName().FullName);
            Global.ReadConfiguration();
            //Global.worldFilename = "DefaultWorld.xml";
            _engineThread = new StoppableThread(UpdateLoop);
            _listener = new Listener(new IPEndPoint(Dns.GetHostEntry(Dns.GetHostName()).AddressList[0], port));
            _log = LogManager.GetCurrentClassLogger();
            ServerStatusModel = new ServerStatusModel {Status = "Created"};
        }

        public void Start()
        {
            ServerStatusModel.Status = "Starting"; 
            _world = new World(Global.WorldId);
            _messageProcessor = new MessageProcessor(_world, _listener);
            Global.World = _world;
            ServerStatusModel.Started = Global.Now;
            _engineThread.Start();
            _log.Info("Listening for new connections...");
            _listener.Start();
            ServerStatusModel.Status = "Running";
        }

        public void Stop()
        {
            ServerStatusModel.Status = "Stopping";
            _listener.Stop();
            _engineThread.Stop();
            _log.Info("Server stopped.");
            ServerStatusModel.Status = "Stopped";
        }

        public void UpdateLoop()
        {
            // need to send a beat message every MillisecondsPerBeat milliseconds:
            try
            {
                // handle world changes
                Global.Now = DateTime.Now;
                _world.Update();

                // handle incomming messages

                _messageProcessor.ProcessMessages();

                CleanupLinkdead();

                if ((DateTime.Now - Global.Now) > TimeSpan.FromSeconds(1))
                {
                    _log.Warn("An update cycle took longer than one second.");
                }
                else
                {
                    System.Threading.Thread.Sleep(100);
                }
            }
            catch (Exception e)
            {
                ServerStatusModel.Status = "Crashing";
                // Just log exceptions and stop all threads
                _log.Error("Update loop exception caught", e);
                Stop();
                ServerStatusModel.Status = "Crashed";
            }
        }

        void CleanupLinkdead()
        {
            var remove = _listener.Clients
                .Where(client => client.Status != ConnectionStatus.Connected
                    && (Global.Now - client.LastMessageTimestamp) > TimeSpan.FromSeconds(60))
                .ToList();
            foreach (Client client in remove)
            {
                if (client.Avatar != null)
                {
                    ((MobileAvatar)client.Avatar).Client = null;
                    _world.Remove(client.Avatar);
                }
                else
                {
                    client.Avatar = null;
                }
                _listener.Clients.Remove(client);
            }
        }
    }
}
