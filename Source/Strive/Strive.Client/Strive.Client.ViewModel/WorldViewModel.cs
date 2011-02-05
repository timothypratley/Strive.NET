using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using System.Linq;
using System.Text;
using System.Windows.Media.Media3D;
using System.Windows.Input;

using UpdateControls.XAML;

using Strive.Client.Model;
using Strive.Network.Client;


namespace Strive.Client.ViewModel
{
    public class WorldViewModel
    {
        public InputBindings bindings;

        private DictionaryModel<string, EntityModel> _world;
        public DictionaryModel<string, EntityModel> World { get { return _world; } }
        private WorldNavigation _navigation;
        public WorldNavigation Navigation { get { return _navigation; } }

        public WorldViewModel(DictionaryModel<string, EntityModel> worldModel)
        {
            bindings = new InputBindings();
            _world = worldModel;
            _navigation = new WorldNavigation();
        }

        public IEnumerable<EntityViewModel> Entities
        {
            get
            {
                return _world.Entities
                    .Select(em => new EntityViewModel(em, _navigation));
            }
        }

        public void AddOrReplace(string name, string modelId, Vector3D position, Quaternion rotation)
        {
            _world.AddEntity(name, new EntityModel(name, modelId, position, rotation));
        }

        public void ClearMouseOverEntity()
        {
            _navigation.MouseOverEntity = null;
        }

        public void SetMouseOverEntity(string name)
        {
            _navigation.MouseOverEntity = _world.GetEntity(name);
        }

        public void SelectAdd(string name)
        {
            var entity = _world.GetEntity(name);
            if (entity != null)
            {
                _navigation.AddSelectedEntity(entity);
            }
        }

        public void Select(string name)
        {
            var entity = _world.GetEntity(name);
            if (entity != null)
            {
                _navigation.SetSelectedEntity(entity);
            }
        }

        public List<EntityViewModel> SelectedEntities
        {
            get
            {
                return _navigation.SelectedEntities
                    .Select(em => new EntityViewModel(em, _navigation))
                    .ToList();
            }
        }

        // TODO: can xaml just use the MouseOverEntity instead?
        public bool IsMouseOverEntity
        {
            get
            {
                return _navigation.MouseOverEntity != null;
            }
        }

        public EntityViewModel MouseOverEntity
        {
            get
            {
                return _navigation.MouseOverEntity == null ? null
                    : new EntityViewModel(_navigation.MouseOverEntity, _navigation);
            }
        }
    }
}
