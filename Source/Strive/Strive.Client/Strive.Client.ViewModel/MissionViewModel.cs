using System;
using System.Diagnostics.Contracts;
using Strive.Common;
using Strive.Model;


namespace Strive.Client.ViewModel
{
    public class MissionViewModel
    {
        private readonly MissionModel _mission;
        private readonly WorldNavigation _entityNavigation;

        public MissionViewModel(MissionModel mission, WorldNavigation navigation)
        {
            Contract.Requires<ArgumentNullException>(mission != null);
            Contract.Requires<ArgumentNullException>(navigation != null);

            _mission = mission;
            _entityNavigation = navigation;
        }

        public MissionModel Mission { get { return _mission; } }

        public bool IsSelected
        {
            get { return _entityNavigation.IsSelected(_mission); }
            set
            {
                if (value)
                    _entityNavigation.AddSelected(_mission);
                else
                    _entityNavigation.RemoveSelected(_mission);
            }
        }

        public override string ToString()
        {
            return _mission.Action
                + " " + _mission.Targets
                + " " + (DateTime.Now - _mission.StartTime).Description()
                + " to " + _mission.Destination
                + " " + (DateTime.Now - _mission.FinishTime).Description();
        }

        public override bool Equals(object obj)
        {
            if (obj == this)
                return true;
            var that = obj as MissionViewModel;
            if (that == null)
                return false;
            return Equals(Mission, that.Mission);
        }

        public override int GetHashCode()
        {
            return Mission.GetHashCode();
        }
    }
}
