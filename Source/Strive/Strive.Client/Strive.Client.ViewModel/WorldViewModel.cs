using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media.Media3D;
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
                    .Do(() => {
                        var p = CurrentPerspective;
                        if (p != null)
                            p.FollowSelected.Execute(null);
                    });
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
                return MakeCommand
                    .When(() => IsMouseOverEntity)
                    .Do(() =>
                    {
                        var x = MouseOverEntity;
                        if (x == null)
                            return;
                        ServerConnection.ProduceEntity(
                            rand.Next(), "Robot", "RTSRobot", x.Entity);
                    });
            }
        }

        public ICommand CreatePlan
        {
            get
            {
                return MakeCommand
                    .When(() => WorldNavigation.SelectedEntities.Any() && IsMouseOverEntity)
                    .Do(() =>
                    {
                        var selected = WorldNavigation.SelectedEntities;
                        var mouseOver = MouseOverEntity;
                        if (!selected.Any() || mouseOver == null)
                            return;

                        ServerConnection.CreatePlan(
                                    rand.Next(), EnumPlanAction.Move,
                                    new EntityModel(rand.Next(), "Protagonist", "Bar", new Vector3D(), Quaternion.Identity, 100, 100, Common.EnumMobileState.Standing, 1.7f),
                                    DateTime.Now, History.Current.Entity[selected.First()],
                                    DateTime.Now + TimeSpan.FromMinutes(1), mouseOver.Entity, 0.2f);
                    });
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
            WorldNavigation.MouseOverEntity = null;
        }

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
