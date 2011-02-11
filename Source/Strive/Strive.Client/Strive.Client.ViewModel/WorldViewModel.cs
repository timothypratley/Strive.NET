using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Media3D;
using Strive.Client.Model;
using Strive.Network.Messaging;


namespace Strive.Client.ViewModel
{
    public class WorldViewModel
    {
        public InputBindings Bindings { get; private set; }
        public DictionaryModel<string, EntityModel> WorldModel { get { return ServerConnection.WorldModel; } }
        public WorldNavigation Navigation { get; private set; }
        public ServerConnection ServerConnection { get; private set; }

        public WorldViewModel(ServerConnection connection)
        {
            ServerConnection = connection;
            Bindings = new InputBindings();
            Navigation = new WorldNavigation();
        }

        public IEnumerable<EntityViewModel> Entities
        {
            get
            {
                return WorldModel.Entities
                    .Select(em => new EntityViewModel(em, Navigation));
            }
        }

        public void AddOrReplace(string name, string modelId, Vector3D position, Quaternion rotation)
        {
            WorldModel.AddEntity(name, new EntityModel(name, modelId, position, rotation));
        }

        public void ClearMouseOverEntity()
        {
            Navigation.MouseOverEntity = null;
        }

        public void SetMouseOverEntity(string name)
        {
            Navigation.MouseOverEntity = WorldModel.GetEntity(name);
        }

        public void SelectAdd(string name)
        {
            var entity = WorldModel.GetEntity(name);
            if (entity != null)
            {
                Navigation.AddSelectedEntity(entity);
            }
        }

        public void Select(string name)
        {
            var entity = WorldModel.GetEntity(name);
            if (entity != null)
            {
                Navigation.SetSelectedEntity(entity);
            }
        }

        public List<EntityViewModel> SelectedEntities
        {
            get
            {
                return Navigation.SelectedEntities
                    .Select(em => new EntityViewModel(em, Navigation))
                    .ToList();
            }
        }

        // TODO: can xaml just use the MouseOverEntity instead?
        public bool IsMouseOverEntity
        {
            get
            {
                return Navigation.MouseOverEntity != null;
            }
        }

        public EntityViewModel MouseOverEntity
        {
            get
            {
                return Navigation.MouseOverEntity == null ? null
                    : new EntityViewModel(Navigation.MouseOverEntity, Navigation);
            }
        }
    }
}
