using System.Diagnostics.Contracts;
using Strive.Client.Model;
using System;


namespace Strive.Client.ViewModel
{
    public class EntityViewModel
    {
        private readonly EntityModel _entity;
        private readonly WorldNavigation _entityNavigation;

        public EntityViewModel(EntityModel entity, WorldNavigation navigation)
        {
            Contract.Requires<ArgumentNullException>(entity != null);
            Contract.Requires<ArgumentNullException>(navigation != null);

            _entity = entity;
            _entityNavigation = navigation;
        }

        public EntityModel Entity { get { return _entity; } }

        public bool IsSelected {
            get { return _entityNavigation.IsSelected(_entity); }
            set {
                if (value)
                    _entityNavigation.AddSelectedEntity(_entity);
                else
                    _entityNavigation.RemoveSelectedEntity(_entity);
            }
        }

        public override string ToString()
        {
            return _entity.Name;
        }

        public override bool Equals(object obj)
        {
            if (obj == this)
                return true;
            var that = obj as EntityViewModel;
            if (that == null)
                return false;
            return Equals(Entity, that.Entity);
        }

        public override int GetHashCode()
        {
            return Entity.GetHashCode();
        }
    }
}
