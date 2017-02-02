using System.Collections.Generic;

namespace Maple.Data
{
    public interface IRepository<T>
    {
        string Path { get; }
        IEnumerable<T> GetAll();
        List<T> GetAllById(params int[] ids);
        T Create(T item);
        int Create(IEnumerable<T> items);
        T Read(int id);
        T Update(T item);
        int Delete(int id);
        int Delete(T item);
        T Save(T item);

        void CreateTable();
    }
}
