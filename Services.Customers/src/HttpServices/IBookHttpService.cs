using System;
using System.Threading.Tasks;

namespace Services.Customers.HttpServices
{
    public interface IBookHttpService
    {
        Task<Data.Book> GetAsync(Guid id);
    }
}