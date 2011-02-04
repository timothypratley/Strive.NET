using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Media3D;

using Common.Logging;

using Strive.Network.Client;
using ToClient = Strive.Network.Messages.ToClient;
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
            e.Position = m.position;
            e.Rotation = m.rotation;
        }

        public void SendPosition(Vector3D position, Quaternion rotation)
        {
            _connection.Position(position, rotation);
        }
    }
}
