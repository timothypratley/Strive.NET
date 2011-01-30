using System;
using System.Net;
using System.Collections;
using System.Configuration;
using System.Reflection;

using Common.Logging;
using Strive.Common;
using Strive.Network.Server;
using Strive.Server.Model;

namespace Strive.Server.Logic
{
    /// <summary>
    /// </summary>
    public class Engine
    {
        int port = Constants.DefaultPort;
        Queue packetQueue = new Queue();
        Listener networkhandler;
        World world;
        MessageProcessor mp;
        StoppableThread engine_thread;
        ILog Log = LogManager.GetCurrentClassLogger();
        
        public ServerStatusModel ServerStatusModel { get; private set; }

        public Engine()
        {
            Log.Info("Creating " + Assembly.GetExecutingAssembly().GetName().FullName);
            Global.ReadConfiguration();
            //Global.worldFilename = "DefaultWorld.xml";
            engine_thread = new StoppableThread(new StoppableThread.WhileRunning(UpdateLoop));
            networkhandler = new Listener(new IPEndPoint(Dns.GetHostEntry(Dns.GetHostName()).AddressList[0], port));
            Log = LogManager.GetCurrentClassLogger();
            ServerStatusModel = new ServerStatusModel();
            ServerStatusModel.Status = "Created";
        }

        public void Start()
        {
            ServerStatusModel.Status = "Starting"; 
            world = new World(Global.WorldID);
            mp = new MessageProcessor(world, networkhandler);
            Global.World = world;
            ServerStatusModel.Started = Global.Now;
            engine_thread.Start();
            Log.Info("Listening for new connections...");
            networkhandler.Start();
            ServerStatusModel.Status = "Running";
        }

        public void Stop()
        {
            ServerStatusModel.Status = "Stopping";
            networkhandler.Stop();
            engine_thread.Stop();
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
                while (networkhandler.MessageCount > 0)
                {
                    mp.ProcessNextMessage();
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
            // TODO: instead of looping through the entire world,
            // we should keep a list of players
            foreach (MobileAvatar ma in (ArrayList)world.mobilesArrayList.Clone())
            {
                if (ma.IsPlayer)
                {
                    if (ma.client == null || (!ma.client.Active && (Global.Now - ma.client.LastMessageTimestamp) > TimeSpan.FromSeconds(60)))
                    {
                        ma.client = null;
                        world.Remove(ma);
                    }
                }
            }
        }
    }
}
