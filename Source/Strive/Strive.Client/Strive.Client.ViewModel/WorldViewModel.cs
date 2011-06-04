using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Microsoft.FSharp.Collections;
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
                    .Do(() =>
                    {
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

        public ICommand CreateMission
        {
            get
            {
                return MakeCommand
                    .When(() => WorldNavigation.SelectedEntities.Any() && IsMouseOverEntity)
                    .Do(() =>
                    {
                        var selected = WorldNavigation.SelectedEntities;
                        if (!selected.Any())
                            return;

                        var mouseOver = MouseOverEntity;
                        var destination = mouseOver.Entity.Position;

                        ServerConnection.CreateMission(
                                    rand.Next(), EnumMissionAction.Move, selected.First(),
                                    DateTime.Now, SetModule.Empty<int>(),
                                    DateTime.Now + TimeSpan.FromMinutes(1), destination, 0.2f);
                    });
            }
        }

        public PerspectiveViewModel CurrentPerspective { get; set; }

        public IEnumerable<EntityViewModel> Entities
        {
            get { return History.Current.Entity.Select(e => new EntityViewModel(e.Value, WorldNavigation)); }
        }

        public IEnumerable<MissionViewModel> Missions
        {
            get { return History.Current.Mission.Select(p => new MissionViewModel(p.Value, WorldNavigation)); }
        }

        public IEnumerable<TaskViewModel> Tasks
        {
            get { return History.Current.Task.Select(p => new TaskViewModel(p.Value, WorldNavigation)); }
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
