using System;
using System.Diagnostics.Contracts;
using Strive.Common;
using Strive.Model;


namespace Strive.Client.ViewModel
{
    public class PlanViewModel
    {
        private readonly PlanModel _plan;
        private readonly WorldNavigation _entityNavigation;

        public PlanViewModel(PlanModel plan, WorldNavigation navigation)
        {
            Contract.Requires<ArgumentNullException>(plan != null);
            Contract.Requires<ArgumentNullException>(navigation != null);

            _plan = plan;
            _entityNavigation = navigation;
        }

        public PlanModel Plan { get { return _plan; } }

        public bool IsSelected
        {
            get { return _entityNavigation.IsSelected(_plan); }
            set
            {
                if (value)
                    _entityNavigation.AddSelected(_plan);
                else
                    _entityNavigation.RemoveSelected(_plan);
            }
        }

        public override string ToString()
        {
            return _plan.Action
                + " " + _plan.Start.Name
                + " " + (DateTime.Now - _plan.StartTime).Description()
                + " to " + _plan.Finish.Name
                + " " + (DateTime.Now - _plan.FinishTime).Description();
        }

        public override bool Equals(object obj)
        {
            if (obj == this)
                return true;
            var that = obj as PlanViewModel;
            if (that == null)
                return false;
            return Equals(Plan, that.Plan);
        }

        public override int GetHashCode()
        {
            return Plan.GetHashCode();
        }
    }
}
