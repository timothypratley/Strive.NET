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
                    .Do(() => WorldModel.Set(
                        new EntityModel("foo", "bar", new Vector3D(1, 2, 3), Quaternion.Identity)));
            }
        }

        public PerspectiveViewModel CurrentPerspective { get; set; }

        public IEnumerable<EntityViewModel> Entities
        {
            get
            {
                return WorldModel.Values
                    .Select(em => new EntityViewModel(em, Navigation));
            }
        }

        public void Set(string name, string modelId, Vector3D position, Quaternion rotation)
        {
            WorldModel.Set(new EntityModel(name, modelId, position, rotation));
        }

        public void ClearMouseOverEntity()
        {
            Navigation.MouseOverEntity = null;
        }

        public void SetMouseOverEntity(string name)
        {
            Navigation.MouseOverEntity = WorldModel.Get(name);
        }

        public void SelectAdd(string name)
        {
            var entity = WorldModel.Get(name);
            if (entity != null)
                Navigation.AddSelectedEntity(entity);
        }

        public void Select(string name)
        {
            var entity = WorldModel.Get(name);
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

        // TODO: can xaml just use the MouseOverEntity instead?
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
