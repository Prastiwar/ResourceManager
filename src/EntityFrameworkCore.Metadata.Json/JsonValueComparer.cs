using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;

namespace EntityFrameworkCore.Metadata.Json
{
    internal class JsonValueComparer<TEntity> : ValueComparer<TEntity> where TEntity : class
    {
        public JsonValueComparer(IJsonValueConverter<TEntity> converter)
            : base((t1, t2) => Equals(converter, t1, t2),
                   t => GetHashCode(converter, t),
                   t => GetSnapshot(converter, t)) => Converter = converter;

        protected IJsonValueConverter<TEntity> Converter { get; }

        private static TEntity GetSnapshot(IJsonValueConverter<TEntity> converter, TEntity instance)
        {
            if (instance is ICloneable cloneable)
            {
                return (TEntity)cloneable.Clone();
            }
            TEntity result = converter.Deserialize(converter.Serialize(instance));
            return result;
        }

        private static int GetHashCode(IJsonValueConverter<TEntity> converter, TEntity instance)
        {
            if (instance is IEquatable<TEntity>)
            {
                return instance.GetHashCode();
            }
            return converter.Serialize(instance).GetHashCode();
        }

        private static bool Equals(IJsonValueConverter<TEntity> converter, TEntity left, TEntity right)
        {
            if (left is IEquatable<TEntity> equatable)
            {
                return equatable.Equals(right);
            }
            bool result = converter.Serialize(left).Equals(converter.Serialize(right));
            return result;
        }

    }
}