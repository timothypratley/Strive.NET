using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using System.Linq;
using System.Text;

using Strive.Client.Model;
using Strive.Client.NavigationModel;
using Strive.Network.Client;

namespace Strive.Client.ViewModel
{
    public class WorldViewModel
    {
        public InputBindings bindings;

        private WorldModel _world;
        private WorldNavigation _navigation;

        public WorldViewModel(ServerConnection serverConnection)
        {
            bindings = new InputBindings();
            _world = new WorldModel(serverConnection);
            _navigation = new WorldNavigation();
        }

        public IEnumerable<EntityViewModel> Entities
        {
            get
            {
                return _world.Entities.Select(em => EntityViewModel.Wrap(em, _navigation));
            }
        }

        public void AddOrReplace(string name, string modelId, double x, double y, double z)
        {
            var entity = new EntityModel(name, modelId, x, y, z, 0, 0, 0);
            _world.AddEntity(entity);
        }

        public void SelectAdd(string name)
        {
            var entity = _world.Entities.Where(e => e.Name == name).FirstOrDefault();
            if (entity != null)
            {
                _navigation.AddSelectedEntity(entity);
            }
        }

        public void Select(string name)
        {
            var entity = _world.Entities.Where(e => e.Name == name).FirstOrDefault();
            if (entity != null)
            {
                _navigation.SetSelectedEntity(entity);
            }
        }

        public IEnumerable<EntityModel> SelectedEntities
        {
            get { return _navigation.SelectedEntities; }
        }
    }
}
