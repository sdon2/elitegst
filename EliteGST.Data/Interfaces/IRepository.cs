using System.Collections.Generic;

namespace EliteGST.Data.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll(params string[] columns);
        T GetById(int id, params string[] columns);
        int? Insert(T entity);
        int Update(T entity, int id);
        bool Delete(int id);
    }
}
