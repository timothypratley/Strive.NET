﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UpdateControls;


namespace Strive.Client.Model
{
    public class WorldModel
    {
        public Dictionary<string, EntityModel> _entities;

        public WorldModel()
        {
            _entities = new Dictionary<string, EntityModel>();
        }

        #region Independent properties
        // Generated by Update Controls --------------------------------
        private Independent _indEntities = new Independent();

        public void AddEntity(string id, EntityModel entity)
        {
            _indEntities.OnSet();
            _entities[id] = entity;
        }

        public void DeleteEntity(string id)
        {
            _indEntities.OnSet();
            _entities.Remove(id);
        }

        public IEnumerable<EntityModel> Entities
        {
            get { _indEntities.OnGet(); return _entities.Values; }
        }
        // End generated code --------------------------------
        #endregion
    }
}
