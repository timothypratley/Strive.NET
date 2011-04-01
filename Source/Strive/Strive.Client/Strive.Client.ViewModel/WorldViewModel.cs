using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Strive.Client.Model;
using Strive.Network.Messaging;
using UpdateControls.XAML;


namespace Strive.Client.ViewModel
{
    public class WorldViewModel
    {
        public InputBindings Bindings { get; private set; }
        public History WorldModel { get { return ServerConnection.WorldModel; } }
        public WorldNavigation Navigation { get; private set; }
        public ServerConnection ServerConnection { get; private set; }

        public WorldViewModel(ServerConnection connection)
        {
            ServerConnection = connection;
            Bindings = new InputBindings();
            Navigation = new WorldNavigation();
        }

        public ICommand FollowSelected
        {
            get
            {
                return MakeCommand
                    .When(() => Navigation.SelectedEntities.Any() && CurrentPerspective != null)
                    .Do(() => CurrentPerspective.FollowSelected.Execute(null));
            }
        }

        public ICommand CreateEntity
        {
            get
            {
                return MakeCommand
                    .When(() => CurrentPerspective != null)
                    .Do(() => CurrentPerspective.CreateEntity.Execute(null));
            }
        }

        public PerspectiveViewModel CurrentPerspective { get; set; }

        public IEnumerable<EntityViewModel> Entities
        {
            get { return WorldModel.Entities.Select(em => new EntityViewModel(em, Navigation)); }
        }

        public int CurrentVersion
        {
            get { return WorldModel.CurrentVersion; }
            set { WorldModel.CurrentVersion = value; }
        }
        public int MaxVersion { get { return WorldModel.MaxVersion; } }

        public void ClearMouseOverEntity()
        {
            Navigation.MouseOverEntity = null;
        }

        public void SetMouseOverEntity(int id)
        {
            Navigation.MouseOverEntity = WorldModel.Get(id);
        }

        public void SelectAdd(int id)
        {
            var entity = WorldModel.Get(id);
            if (entity != null)
                Navigation.AddSelectedEntity(entity);
        }

        public void Select(int id)
        {
            var entity = WorldModel.Get(id);
            if (entity != null)
                Navigation.SetSelectedEntity(entity);
        }

        public IEnumerable<EntityViewModel> SelectedEntities
        {
            get
            {
                return Navigation.SelectedEntities
                    .Select(em => new EntityViewModel(em, Navigation));
            }
        }

        // TODO: can XAML just use the MouseOverEntity instead?
        public bool IsMouseOverEntity { get { return Navigation.MouseOverEntity != null; } }

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
