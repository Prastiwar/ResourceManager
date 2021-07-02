using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Reflection;

namespace ResourceManager.DataSource.Sql
{
    public static class EntityFrameworkCoreExtensions
    {
        public static IQueryable<object> Set(this DbContext context, Type entityType)
        {
            MethodInfo method = typeof(DbContext).GetMethod(nameof(DbContext.Set), BindingFlags.Public | BindingFlags.Instance, null, Type.EmptyTypes, null);
            method = method.MakeGenericMethod(entityType);
            return (method.Invoke(context, null) as IQueryable).Cast<object>();
        }
    }
}
