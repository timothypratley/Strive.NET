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

        public ObservableViewEntityDictionary ViewEntities = new ObservableViewEntityDictionary();

        public WorldViewModel(ServerConnection connection)
        {
            bindings = new InputBindings();
            this.connection = connection;
            connection.OnPositionSent += new ServerConnection.OnPositionSentHandler(UpdatePositions);
            ViewEntities.Add("StormSworder1", new ViewEntity("StormSworder1", 1, 0, 0, 0, 0, 0, 0));
            ViewEntities.Add("Farmer1", new ViewEntity("Farmer1", 2, 0, 0, 0, 0, 0, 0));
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
            ViewEntity entity = ViewEntities[Position.instance_id.ToString()];
            entity.X = Position.position.X;
            entity.Y = Position.position.Y;
            entity.Z = Position.position.Z;
            OnCollectionChanged(this, new CollectionChangeEventArgs(CollectionChangeAction.Refresh, entity));
        }
    }
}
