using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using LinqToDB;

namespace Maple.Data
{
    internal static class DBGenericActions
    {
        public static async Task UpdateEntity<T>(T entity)
            where T : class
        {
            using (var db = new DataContext(""))
                await db.UpdateAsync(entity).ConfigureAwait(false);
        }

        public static async Task<object> InsertEntity<T>(T entity)
            where T : class
        {
            using (var db = new DataContext(""))
                return await db.InsertWithIdentityAsync(entity).ConfigureAwait(false);
        }

        public static async Task<object> InsertOrUpdateEntity<T>(T entity)
            where T : class
        {
            using (var db = new DataContext(""))
                return await db.InsertOrReplaceAsync(entity).ConfigureAwait(false);
        }

        public static async Task DeleteEntity<T>(T entity)
            where T : class
        {
            using (var db = new DataContext(""))
                await db.DeleteAsync(entity).ConfigureAwait(false);
        }

        public static async Task<List<T>> GetAllFromEntity<T>()
            where T : class
        {
            using (var db = new DataContext(""))
                return await db.GetTable<T>().ToListAsync().ConfigureAwait(false);
        }

        //public static List<T> GetEntitiesByParameters<T>(Func<T, bool> where)
        //    where T : class
        //{
        //    using (var db = new DataContext(""))
        //    {
        //        return db.GetTable<T>().Where<T>(where).Where<T>(GetLogicExclusion<T>()).ToList());
        //    }
        //}

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T">linqToDb Table mapped</typeparam>
        /// <param name="pk"> Have to be of the same type of primary key atribute of T table mapped</param>
        /// <returns>T linqToDb mapped class</returns>
        public static async Task<T> GetEntityByPK<T>(object pk)
            where T : class
        {
            using (var db = new DataContext(""))
            {
                var pkName = typeof(T).GetProperties().Where(prop => prop.GetCustomAttributes(typeof(LinqToDB.Mapping.PrimaryKeyAttribute), false).Count() > 0).First();
                var expression = SimpleComparison<T>(pkName.Name, pk);

                return await db.GetTable<T>().Where<T>(expression).FirstOrDefaultAsync().ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Excelent to use to get entities by FK
        /// </summary>
        /// <typeparam name="T">Entity To Filter From DB Mapped</typeparam>
        /// <typeparam name="D">Type of property to filter using Equals Comparer</typeparam>
        /// <param name="propertyName">Name of property</param>
        /// <param name="valueToFilter">Value to filter query</param>
        /// <returns>List of T</returns>
        public static async Task<ICollection<T>> GetAllEntititiesByPropertyValue<T, D>(string propertyName, D valueToFilter)
            where T : class
        {
            if (string.IsNullOrWhiteSpace(propertyName))
                return await GetAllFromEntity<T>().ConfigureAwait(false);

            var expression = SimpleComparison<T, D>(propertyName, valueToFilter);

            using (var db = new DataContext(""))
            {
                var data = await db.GetTable<T>().Where<T>(expression).ToListAsync().ConfigureAwait(false);
                return data;
            }
        }

        public static Expression<Func<T, bool>> SimpleComparison<T>(string property, object value)
            where T : class
        {
            var type = typeof(T);
            var pe = Expression.Parameter(type, "p");
            var propertyReference = Expression.Property(pe, property);
            var constantReference = Expression.Constant(value);

            return Expression.Lambda<Func<T, bool>>
                (Expression.Equal(propertyReference, constantReference),
                new[] { pe });
        }

        private static Expression<Func<T, bool>> SimpleComparison<T, D>(string propertyName, D value)
            where T : class
        {
            var type = typeof(T);
            var pe = Expression.Parameter(type, "p");
            var constantReference = Expression.Constant(value);
            var propertyReference = Expression.Property(pe, propertyName);

            return Expression.Lambda<Func<T, bool>>(
                Expression.Equal(propertyReference, constantReference),
                new[] { pe });
        }
    }
}
