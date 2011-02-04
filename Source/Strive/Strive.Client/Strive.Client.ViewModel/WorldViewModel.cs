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
using Strive.Client.NavigationModel;
using Strive.Network.Client;


namespace Strive.Client.ViewModel
{
    public class WorldViewModel
    {
        public InputBindings bindings;

        private WorldModel _world;
        public WorldModel World { get { return _world; } }
        private WorldNavigation _navigation;

        public WorldViewModel(WorldModel worldModel)
        {
            bindings = new InputBindings();
            _world = worldModel;
            _navigation = new WorldNavigation();
        }

        public List<EntityViewModel> Entities
        {
            get
            {
                return _world.Entities.Select(
                    em => EntityViewModel.Wrap(em, _navigation)).ToList();
            }
        }

        // TODO: make useful
        public ICommand CreateEntity
        {
            get
            {
                return MakeCommand
                    .When(() => true)
                    .Do(() => AddOrReplace("foo", "bar", new Vector3D(0,0,0), Quaternion.Identity));
            }
        }

        public void AddOrReplace(string name, string modelId, Vector3D position, Quaternion rotation)
        {
            var entity = new EntityModel(name, modelId, position, rotation);
            _world.AddEntity(name, entity);
        }

        public void ClearMouseOverEntity()
        {
            _navigation.MouseOverEntity = null;
        }

        public void SetMouseOverEntity(string name)
        {
            var entity = _world.Entities.Where(e => e.Name == name).FirstOrDefault();
            if (entity != null)
            {
                _navigation.MouseOverEntity = entity;
            }
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

        public List<EntityViewModel> SelectedEntities
        {
            get
            {
                return _navigation.SelectedEntities.Select(
                    em => EntityViewModel.Wrap(em, _navigation)).ToList();
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
                if (_navigation.MouseOverEntity == null)
                    return null;
                else
                    return EntityViewModel.Wrap(_navigation.MouseOverEntity, _navigation);
            }
        }
    }
}
