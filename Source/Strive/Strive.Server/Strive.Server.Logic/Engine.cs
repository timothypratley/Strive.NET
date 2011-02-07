using System;
using System.Net;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;

using Common.Logging;
using Strive.Common;
using Strive.Network.Server;
using Strive.Server.Model;
using Strive.Network.Messages;

namespace Strive.Server.Logic
{
    /// <summary>
    /// </summary>
    public class Engine
    {
        int port = Constants.DefaultPort;
        Listener networkHandler;
        World world;
        MessageProcessor messageProcessor;
        StoppableThread engineThread;
        ILog Log = LogManager.GetCurrentClassLogger();
        
        public ServerStatusModel ServerStatusModel { get; private set; }

        public Engine()
        {
            Log.Info("Creating " + Assembly.GetExecutingAssembly().GetName().FullName);
            Global.ReadConfiguration();
            //Global.worldFilename = "DefaultWorld.xml";
            engineThread = new StoppableThread(new StoppableThread.WhileRunning(UpdateLoop));
            networkHandler = new Listener(new IPEndPoint(Dns.GetHostEntry(Dns.GetHostName()).AddressList[0], port));
            Log = LogManager.GetCurrentClassLogger();
            ServerStatusModel = new ServerStatusModel();
            ServerStatusModel.Status = "Created";
        }

        public void Start()
        {
            ServerStatusModel.Status = "Starting"; 
            world = new World(Global.WorldID);
            messageProcessor = new MessageProcessor(world, networkHandler);
            Global.World = world;
            ServerStatusModel.Started = Global.Now;
            engineThread.Start();
            Log.Info("Listening for new connections...");
            networkHandler.Start();
            ServerStatusModel.Status = "Running";
        }

        public void Stop()
        {
            ServerStatusModel.Status = "Stopping";
            networkHandler.Stop();
            engineThread.Stop();
            Log.Info("Server stopped.");
            ServerStatusModel.Status = "Stopped";
        }

        public void UpdateLoop()
        {
            // need to send a beat message every MillisecondsPerBeat milliseconds:
            try
            {
                // handle world changes
                Global.Now = DateTime.Now;
                world.Update();

                // handle incomming messages

                // TODO: where should the message queue live?
                while (networkHandler.MessageCount > 0)
                {
                    messageProcessor.ProcessNextMessage();
                }

                CleanupLinkdead();

                if ((DateTime.Now - Global.Now) > TimeSpan.FromSeconds(1))
                {
                    Log.Warn("An update cycle took longer than one second.");
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
                Log.Error("Update loop exception caught", e);
                Stop();
                ServerStatusModel.Status = "Crashed";
            }
        }

        void CleanupLinkdead()
        {
            List<Client> remove = new List<Client>();
            foreach (Client client in networkHandler.Clients)
            {
                if (client.Status != ConnectionStatus.Connected
                    && (Global.Now - client.LastMessageTimestamp) > TimeSpan.FromSeconds(60))
                {
                    remove.Add(client);
                }
            }
            foreach (Client client in remove)
            {
                if (client.Avatar != null)
                {
                    ((MobileAvatar)client.Avatar).client = null;
                    world.Remove(client.Avatar);
                }
                else
                {
                    client.Avatar = null;
                }
                networkHandler.Clients.Remove(client);
            }
        }
    }
}
