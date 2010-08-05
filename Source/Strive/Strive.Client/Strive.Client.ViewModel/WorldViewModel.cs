using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Strive.Network.Client;
using ToClient = Strive.Network.Messages.ToClient;

namespace Strive.Client.ViewModel
{
    public class WorldViewModel
    {
        public event CollectionChangeEventHandler CollectionChanged;

        public ServerConnection connection;
        public InputBindings bindings;

        Dictionary<int, ViewEntity> ViewEntities = new Dictionary<int, ViewEntity>();

        public WorldViewModel(ServerConnection connection)
        {
            bindings = new InputBindings();
            this.connection = connection;
            connection.OnPositionSent += new ServerConnection.OnPositionSentHandler(UpdatePositions);
        }

        public void OnCollectionChanged(object sender, CollectionChangeEventArgs e)
        {
            if (CollectionChanged != null)
            {
                CollectionChanged(sender, e);
            }
        }

        void UpdatePositions(ToClient.Position Position)
        {
            ViewEntity entity = ViewEntities[Position.instance_id];
            entity.X = Position.position.X;
            entity.Y = Position.position.Y;
            entity.Z = Position.position.Z;
            OnCollectionChanged(this, new CollectionChangeEventArgs(CollectionChangeAction.Refresh, entity));
        }
    }
}
