using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Media3D;

namespace Strive.DataModel
{
    internal class PropertyMap
    {
        public enum Property
        {
            Id,
            Weight,
            Postition,
            Rotation,
            Model3D,
            Health,
            Speed
        }

        public static readonly IDictionary<Property, Type> PropertyTypes
            = new Dictionary<Property, Type>
                  {
                      {Property.Id, typeof (int)},
                      {Property.Weight, typeof (double)},
                      {Property.Postition, typeof (Vector3D)},
                      {Property.Rotation, typeof (Quaternion)},
                      {Property.Model3D, typeof (string)},
                      {Property.Health, typeof (double)},
                      {Property.Speed, typeof (double)}
                  };

        public enum DuckType
        {
            Task,
            Entity,
            Mobile,
            Item
        }

        // TODO: using hashes is really overhead given the collections are so small
        public static readonly IDictionary<DuckType, ISet<Property>> Types =
            new Dictionary<DuckType, ISet<Property>>
                {
                    {DuckType.Task, new HashSet<Property>()},
                    {DuckType.Entity, new HashSet<Property> {Property.Postition, Property.Rotation}},
                    {DuckType.Item, new HashSet<Property> {Property.Weight}},
                    {DuckType.Mobile, new HashSet<Property> {Property.Speed}}
                };

        public static readonly IDictionary<DuckType, ISet<DuckType>> Inheritance =
            new Dictionary<DuckType, ISet<DuckType>>
                {
                    {DuckType.Task, new HashSet<DuckType>()},
                    {DuckType.Item, new HashSet<DuckType> {DuckType.Entity}},
                    {DuckType.Mobile, new HashSet<DuckType> {DuckType.Mobile}}
                };

        public bool IsA(DuckType type)
        {
            return Types[type].All(Properties.ContainsKey) && Inheritance[type].All(IsA);
        }

        // TODO: rewrite in F# for immutable map
        public readonly IDictionary<Property, object> Properties = new Dictionary<Property, object>();
    }
}
