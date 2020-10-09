using System.Collections.Generic;
using System.Threading.Tasks;
using Models;

namespace Services
{
#pragma warning disable CS1591
    public interface ICustomerService
    {
        ValueTask<Customer> GetById(int id);
        Task<IEnumerable<Customer>> GetByName(string name);
        Task<PaginationWithResult<Customer>> GetAll(UrlQuery urlQuery);
    }
#pragma warning disable CS1591
}