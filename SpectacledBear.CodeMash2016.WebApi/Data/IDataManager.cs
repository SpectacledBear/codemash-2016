using System.Collections.Generic;

namespace SpectacledBear.CodeMash2016.WebApi.Data
{
    internal interface IDataManager<T>
    {
        IEnumerable<T> GetAll();
        bool TryGet(long id, out T result);
        bool TryGet(T reference, out long id);
        T Update(T input, long id);
        T Insert(T input);
        bool Delete(long id);
    }
}
