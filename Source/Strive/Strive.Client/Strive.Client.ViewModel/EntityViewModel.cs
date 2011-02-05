using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Strive.Client.Model;


namespace Strive.Client.ViewModel
{
    public class EntityViewModel
    {
        private EntityModel _entity;
        private WorldNavigation _entityNavigation;

        public EntityViewModel(EntityModel entity, WorldNavigation navigation)
        {
            _entity = entity; _entityNavigation = navigation;
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
            EntityViewModel that = obj as EntityViewModel;
            if (that == null)
                return false;
            return object.Equals(this.Entity, that.Entity);
        }

        public override int GetHashCode()
        {
            return Entity.GetHashCode();
        }
    }
}
