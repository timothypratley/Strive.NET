using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using System.Linq;
using System.Text;

using Strive.Network.Client;
using ToClient = Strive.Network.Messages.ToClient;

namespace Strive.Client.ViewModel
{
    public class WorldViewModel
    {
        public ServerConnection connection;
        public InputBindings bindings;

        public ICollectionView EntitiesView
        {
            get
            {
                if (_entitiesView == null)
                {
                    _entitiesView = CollectionViewSource.GetDefaultView(Entities);
                }
                return _entitiesView;
            }
        }
        private ICollectionView _entitiesView;

        private ObservableViewEntityDictionary Entities = new ObservableViewEntityDictionary();

        public WorldViewModel(ServerConnection connection)
        {
            bindings = new InputBindings();
            this.connection = connection;
            connection.OnPositionSent += new ServerConnection.OnPositionSentHandler(UpdatePositions);
            //Entities.Add("StormSworder1", new ViewEntity("StormSworder1", 1, 0, 0, 0, 0, 0, 0));
            //Entities.Add("Farmer1", new ViewEntity("Farmer1", 2, 0, 0, 0, 0, 0, 0));
        }

        public void AddOrReplace(string name, ViewEntity entity)
        {
            Entities.Add(name, entity);
        }

        void UpdatePositions(ToClient.Position Position)
        {
            ViewEntity entity = Entities[Position.instance_id.ToString()];
            entity.X = Position.position.X;
            entity.Y = Position.position.Y;
            entity.Z = Position.position.Z;
        }
    }
}
