using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common.Logging;

using Strive.Network.Client;
using ToClient = Strive.Network.Messages.ToClient;
using Strive.Math3D;
using Strive.Network.Messages;


namespace Strive.Client.Model
{
    public class ConnectionHandler
    {
        ServerConnection _connection;
        static ILog Log = LogManager.GetCurrentClassLogger();
        WorldModel _world;

        public ConnectionHandler(ServerConnection connection, WorldModel world)
        {
            _connection = connection;
            _world = world;
            _connection.MessageRecieved += new EventHandler(_connection_MessageRecieved);
        }

        void _connection_MessageRecieved(object sender, EventArgs e)
        {
            dynamic m = _connection.PopNextMessage();
            Log.Trace("Received " + m.GetType() + " message: " + m);
            Process(m);
            var x = e as IMessage;
            Log.Trace("x " + x);
        }

        void Process(ToClient.AddTerrain m)
        {
            Log.Trace("foo");
        }
        void Process(ToClient.Position m)
        {
            Log.Trace("bar");
            EntityModel e = _world._entities[m.instance_id.ToString()];
            e.X = m.position.X;
            e.Y = m.position.Y;
            e.Z = m.position.Z;
            e.DirX = m.rotation.X;
            e.DirY = m.rotation.Y;
            e.DirZ = m.rotation.Z;
        }

        public void SendPosition(Vector3D position, Vector3D rotation)
        {
            _connection.Position(position, rotation);
        }
    }
}
