using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Concurrent;
using System.Reflection;
using Dapper;
using EliteGST.Data.Interfaces;
using EliteGST.Data.Models;

namespace EliteGST.Data.Repositories
{
    public abstract class BaseRepository<T> : IDisposable, IRepository<T> where T : class
    {
        public readonly string Table;
        protected readonly IDbConnection Connection;

        public BaseRepository(string table)
        {
            Table = table;
            Connection = Database.Connection;
        }

        public IEnumerable<T> GetAll(params string[] columns)
        {
            var cols = "*";
            if (columns.Count() > 0) cols = string.Join(",", columns);
            return Connection.Query<T>(string.Format("SELECT {0} FROM {1}", cols, Table));
        }

        public T GetById(int id, params string[] columns)
        {
            var cols = "*";
            if (columns.Count() > 0) cols = string.Join(",", columns);
            return Connection.QueryFirstOrDefault<T>(string.Format("SELECT {0} FROM {1} WHERE Id = @id", cols, Table), new { id = id });
        }

        public int? Insert(T entity)
        {
            var o = (object)entity;
            List<string> paramNames = GetParamNames(o);
            paramNames.Remove("Id");

            string cols = string.Join(",", paramNames);
            string colsParams = string.Join(",", paramNames.Select(p => "@" + p));
            var sql = string.Format("INSERT INTO {0} ({1}) VALUES ({2}); SELECT LAST_INSERT_ID();", Table, cols, colsParams);

            using (var transaction = Connection.BeginTransaction())
            {
                try
                {
                    var result = Connection.Query<int?>(sql, o).Single();
                    transaction.Commit();
                    return result;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
            
        }

        public int Update(T entity, int id)
        {
            var o = (object)entity;
            List<string> paramNames = GetParamNames(o);
            paramNames.Remove("Id");

            string paramStr = string.Join(",", paramNames.Select(p => p + " = @" + p));
            var sql = string.Format("UPDATE {0} SET {1} WHERE Id = {2};", Table, paramStr, id);

            using (var transaction = Connection.BeginTransaction())
            {
                try
                {
                    var result = Connection.Execute(sql, o);
                    transaction.Commit();
                    return result;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public bool Delete(int id)
        {
            var sql = string.Format("DELETE FROM {0} WHERE Id = @id", Table);

            using (var transaction = Connection.BeginTransaction())
            {
                try
                {
                    var result = Connection.Execute(sql, new { id = id }) > 0;
                    transaction.Commit();
                    return result;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        private static readonly ConcurrentDictionary<Type, List<string>> paramNameCache = new ConcurrentDictionary<Type, List<string>>();

        internal static List<string> GetParamNames(object o)
        {
            if (o is DynamicParameters)
            {
                var parameters  = o as DynamicParameters;
                return parameters.ParameterNames.ToList();
            }
            List<string> paramNames;
            if (!paramNameCache.TryGetValue(o.GetType(), out paramNames))
            {
                paramNames = new List<string>();
                foreach (var prop in o.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(p => p.GetGetMethod(false) != null))
                {
                    var attribs = prop.GetCustomAttributes(typeof(IgnorePropertyAttribute), true);
                    var attr = attribs.FirstOrDefault() as IgnorePropertyAttribute;
                    if (attr == null || (!attr.Value))
                    {
                        paramNames.Add(prop.Name);
                    }
                }
                paramNameCache[o.GetType()] = paramNames;
            }
            return paramNames;
        }

        public void Dispose()
        {
            Connection.Dispose();
        }
    }
}
