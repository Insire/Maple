using System;
using System.Collections.Generic;

namespace InsireBot.Data
{
    public interface IRepository<T>
    {
        IEnumerable<T> GetAll();
        List<T> GetAllById(params int[] ids);
        T Create(T item);
        int Create(IEnumerable<T> item);
        T Read(int id);
        T Update(T item);
        int Delete(int id);
    }
}
