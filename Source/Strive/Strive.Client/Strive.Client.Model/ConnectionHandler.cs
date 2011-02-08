using System;
using System.Windows.Media.Media3D;

using Common.Logging;

using Strive.Network.Client;
using Strive.Network.Messages.ToClient;
using Strive.Network.Messages;


namespace Strive.Client.Model
{
    public class ConnectionHandler
    {
        ServerConnection _connection;
        static ILog Log = LogManager.GetCurrentClassLogger();
        DictionaryModel<string, EntityModel> _world;

        public ConnectionHandler(ServerConnection connection, DictionaryModel<string, EntityModel> world)
        {
            _connection = connection;
            _world = world;
            _connection.MessageRecieved += ConnectionMessageRecieved;
        }

        void ConnectionMessageRecieved(object sender, EventArgs e)
        {
            dynamic m = _connection.PopNextMessage();
            Log.Trace("Received " + m.GetType() + " message: " + m);
            Process(m);
            var x = e as IMessage;
            Log.Trace("x " + x);
        }

        void Process(AddTerrain m)
        {
            Log.Trace("foo");
        }
        void Process(Position m)
        {
            Log.Trace("bar");
            EntityModel e = _world.EntityDictionary[m.instance_id.ToString()];
            e.Position = m.position;
            e.Rotation = m.rotation;
        }

        public void SendPosition(Vector3D position, Quaternion rotation)
        {
            _connection.Position(position, rotation);
        }
    }
}
