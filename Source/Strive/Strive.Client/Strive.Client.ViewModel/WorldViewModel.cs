using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Media3D;
using Strive.Client.Model;
using Strive.Network.Messaging;


namespace Strive.Client.ViewModel
{
    public class WorldViewModel
    {
        public InputBindings Bindings;

        private DictionaryModel<string, EntityModel> _world;
        public DictionaryModel<string, EntityModel> World { get { return _world; } }
        private WorldNavigation _navigation;
        public WorldNavigation Navigation { get { return _navigation; } }
        public ConnectionHandler ConnectionHandler { get; private set; }

        public WorldViewModel(DictionaryModel<string, EntityModel> worldModel, ConnectionHandler connectionHandler)
        {
            Bindings = new InputBindings();
            _world = worldModel;
            _navigation = new WorldNavigation();
            ConnectionHandler = connectionHandler;
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
