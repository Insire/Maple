using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reflection;

namespace Maple
{
    public static class ContextExtensions
    {
        /// <summary>
        /// Adds an entity (if newly created) or update (if has non-default Id).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context">The db context.</param>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        /// <remarks>
        /// Will not work for HasDatabaseGeneratedOption(DatabaseGeneratedOption.None).
        /// Will not work for composite keys.
        /// </remarks>
        public static T AddOrUpdate<T>(this DbContext context, T entity)
            where T : class
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            if (IsTransient(context, entity))
            {
                context.Set<T>().Add(entity);
            }
            else
            {
                context.Set<T>().Attach(entity);
                context.Entry(entity).State = EntityState.Modified;
            }
            return entity;
        }

        /// <summary>
        /// Determines whether the specified entity is newly created (Id not specified).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context">The context.</param>
        /// <param name="entity">The entity.</param>
        /// <returns>
        ///   <c>true</c> if the specified entity is transient; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// Will not work for HasDatabaseGeneratedOption(DatabaseGeneratedOption.None).
        /// Will not work for composite keys.
        /// </remarks>
        public static bool IsTransient<T>(this DbContext context, T entity)
            where T : class
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var propertyInfo = FindPrimaryKeyProperty<T>(context);
            var propertyType = propertyInfo.PropertyType;

            //what's the default value for the type?
            var transientValue = propertyType.IsValueType
                ? Activator.CreateInstance(propertyType)
                : null;

            //is the pk the same as the default value (int == 0, string == null ...)
            return Equals(propertyInfo.GetValue(entity, null), transientValue);
        }

        private static PropertyInfo FindPrimaryKeyProperty<T>(IObjectContextAdapter context)
            where T : class
        {
            //find the primary key
            var objectContext = context.ObjectContext;
            //this will error if it's not a mapped entity
            var objectSet = objectContext.CreateObjectSet<T>();
            var elementType = objectSet.EntitySet.ElementType;
            var pk = elementType.KeyMembers.First();
            //look it up on the entity
            var propertyInfo = typeof(T).GetProperty(pk.Name);
            return propertyInfo;
        }
    }
}
