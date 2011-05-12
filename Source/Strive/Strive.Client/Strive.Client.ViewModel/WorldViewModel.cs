using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using Strive.Model;
using Strive.Network.Messaging;
using UpdateControls.XAML;
using System.Windows.Threading;


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

        // TODO: use the application random somehow
        private Random rand = new Random();
        public ICommand ProduceEntity
        {
            get
            {
                // TODO: There is a race condition where MouseOverEntity can be updated between the when and do
                return MakeCommand
                    .When(() => IsMouseOverEntity)
                    .Do(() => ServerConnection.ProduceEntity(
                        rand.Next(), "Robot", "RTSRobot", MouseOverEntity.Entity));
            }
        }

        public ICommand CreatePlan
        {
            get
            {
                // TODO: use real values, remember entities are actually filters (make explicit)
                var e = new EntityModel(rand.Next(), "Protagonist", "Bar", new Vector3D(), Quaternion.Identity, 100, 100, Common.EnumMobileState.Standing, 1.7f);
                return MakeCommand
                    .When(() => WorldNavigation.SelectedEntities.Any() && IsMouseOverEntity)
                    .Do(() => ServerConnection.CreatePlan(
                        rand.Next(), EnumPlanAction.Move, e,
                        DateTime.Now, History.Current.Entity[WorldNavigation.SelectedEntities.First()],
                        DateTime.Now + TimeSpan.FromMinutes(1), MouseOverEntity.Entity, 0.2f));
            }
        }


        public PerspectiveViewModel CurrentPerspective { get; set; }

        public IEnumerable<EntityViewModel> Entities
        {
            get { return History.Current.Entity.Select(e => new EntityViewModel(e.Value, WorldNavigation)); }
        }

        public IEnumerable<PlanViewModel> Plans
        {
            get { return History.Current.Plan.Select(p => new PlanViewModel(p.Value, WorldNavigation)); }
        }

        public int CurrentVersion
        {
            get { return History.CurrentVersion; }
            set { History.CurrentVersion = value; }
        }
        public int MaxVersion { get { return History.MaxVersion; } }

        public void ClearMouseOverEntity()
        {
            Dispatcher.CurrentDispatcher.BeginInvoke(
                new Action<WorldViewModel>((sender) => { WorldNavigation.MouseOverEntity = null; }),
                null);
        }

        // TODO: can XAML just use the MouseOverEntity instead?
        public bool IsMouseOverEntity { get { return WorldNavigation.MouseOverEntity.HasValue; } }

        public EntityViewModel MouseOverEntity
        {
            get
            {
                return WorldNavigation.MouseOverEntity.HasValue
                    ? new EntityViewModel(History.Current.Entity[WorldNavigation.MouseOverEntity.Value], WorldNavigation)
                    : null;
            }
        }
    }
}
