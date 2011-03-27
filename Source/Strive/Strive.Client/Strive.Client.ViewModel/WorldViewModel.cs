using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using System.Windows.Threading;
using UpdateControls;
using UpdateControls.XAML;
using Strive.Client.Model;
using Strive.Network.Messaging;


namespace Strive.Client.ViewModel
{
    public class WorldViewModel
    {
        public InputBindings Bindings { get; private set; }
        public WorldModel WorldModel { get { return ServerConnection.WorldModel; } }
        public WorldNavigation Navigation { get; private set; }
        public ServerConnection ServerConnection { get; private set; }

        public WorldViewModel(ServerConnection connection)
        {
            ServerConnection = connection;
            Bindings = new InputBindings();
            Navigation = new WorldNavigation();

            _depWorldModel = new Dependent(UpdateWorldModel);
            _depWorldModel.Invalidated += () =>
                Dispatcher.CurrentDispatcher.BeginInvoke((Action)_depWorldModel.OnGet);
            _depWorldModel.OnGet();
        }

        public event EventHandler WorldChanged;
        private readonly Dependent _depWorldModel;
        void UpdateWorldModel()
        {
            var discard = WorldModel.Values;
            var eh = WorldChanged;
            if (eh != null)
                eh(this, new EventArgs());
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
            get { return WorldModel.Values.Select(em => new EntityViewModel(em, Navigation)); }
        }

        public int CurrentVersion
        {
            get { return WorldModel.History.CurrentVersion; }
            set { WorldModel.History.CurrentVersion = value; }
        }
        public int MaxVersion { get { return WorldModel.History.MaxVersion; } }

        public void Set(int id, string name, string modelId, Vector3D position, Quaternion rotation)
        {
            WorldModel.Set(new EntityModel(id, name, modelId, position, rotation));
        }

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
