using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Strive.Model;
using Strive.Network.Messaging;
using UpdateControls.XAML;


namespace Strive.Client.ViewModel
{
    public class WorldViewModel
    {
        public ServerConnection ServerConnection { get; private set; }
        public History History { get; private set; }
        public WorldNavigation WorldNavigation { get; private set; }
        public InputBindings InputBindings { get; private set; }

        public WorldViewModel(ServerConnection connection, History history, WorldNavigation worldNavigation, InputBindings inputBindings)
        {
            ServerConnection = connection;
            History = history;
            WorldNavigation = worldNavigation;
            InputBindings = inputBindings;
        }

        public ICommand FollowSelected
        {
            get
            {
                return MakeCommand
                    .When(() => WorldNavigation.SelectedEntities.Any() && CurrentPerspective != null)
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

        public ICommand CreateMobile
        {
            get
            {
                return MakeCommand
                    .When(() => CurrentPerspective != null)
                    .Do(() => CurrentPerspective.CreateMobile.Execute(null));
            }
        }

        public ICommand CreateFactory
        {
            get
            {
                return MakeCommand
                    .When(() => CurrentPerspective != null)
                    .Do(() => CurrentPerspective.CreateFactory.Execute(null));
            }
        }

        public ICommand ProduceEntity
        {
            get
            {
                return MakeCommand
                    .When(() => CurrentPerspective != null)
                    .Do(() => CurrentPerspective.ProduceEntity.Execute(null));
            }
        }

        public PerspectiveViewModel CurrentPerspective { get; set; }

        public IEnumerable<EntityViewModel> Entities
        {
            get { return History.Entities.Select(em => new EntityViewModel(em, WorldNavigation)); }
        }

        public int CurrentVersion
        {
            get { return History.CurrentVersion; }
            set { History.CurrentVersion = value; }
        }
        public int MaxVersion { get { return History.MaxVersion; } }

        public void ClearMouseOverEntity()
        {
            WorldNavigation.MouseOverEntity = null;
        }

        public void SetMouseOverEntity(int id)
        {
            WorldNavigation.MouseOverEntity = History.GetEntity(id);
        }

        public void SelectAdd(int id)
        {
            var entity = History.GetEntity(id);
            if (entity != null)
                WorldNavigation.AddSelectedEntity(entity);
        }

        public void Select(int id)
        {
            var entity = History.GetEntity(id);
            if (entity != null)
                WorldNavigation.SetSelectedEntity(entity);
        }

        public IEnumerable<EntityViewModel> SelectedEntities
        {
            get
            {
                return WorldNavigation.SelectedEntities
                    .Select(em => new EntityViewModel(em, WorldNavigation));
            }
        }

        // TODO: can XAML just use the MouseOverEntity instead?
        public bool IsMouseOverEntity { get { return WorldNavigation.MouseOverEntity != null; } }

        public EntityViewModel MouseOverEntity
        {
            get
            {
                return WorldNavigation.MouseOverEntity == null ? null
                    : new EntityViewModel(WorldNavigation.MouseOverEntity, WorldNavigation);
            }
        }
    }
}
