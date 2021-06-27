using System;
using System.Collections.Generic;

namespace ResourceManager
{
    public class IdentityEqualityComparer : IEqualityComparer<object>
    {
        static IdentityEqualityComparer() => Default = new IdentityEqualityComparer();

        private IdentityEqualityComparer() { }

        public static IEqualityComparer<object> Default { get; }

        public int GetHashCode(object obj) => obj.GetHashCode();

        public new bool Equals(object x, object y) => ((IEqualityComparer<object>)this).Equals(x, y);

        int IEqualityComparer<object>.GetHashCode(object obj) => obj.GetHashCode();

        bool IEqualityComparer<object>.Equals(object x, object y)
        {
            if (x is null && y is null)
            {
                return true;
            }
            if (x is null || y is null)
            {
                return false;
            }
            bool sameType = x.GetType() == y.GetType();
            if (sameType)
            {
                return EqualityComparer<object>.Default.Equals(x, y);
            }
            if (TryChangeType(y, x.GetType(), out object newY))
            {
                return EqualityComparer<object>.Default.Equals(x, newY);
            }
            if (TryChangeType(x, y.GetType(), out object newX))
            {
                return EqualityComparer<object>.Default.Equals(newX, y);
            }
            return false;
        }

        private bool TryChangeType(object value, Type type, out object newValue)
        {
            try
            {
                newValue = Convert.ChangeType(value, type);
            }
            catch (Exception)
            {
                newValue = null;
                return false;
            }
            return true;
        }
    }
}
