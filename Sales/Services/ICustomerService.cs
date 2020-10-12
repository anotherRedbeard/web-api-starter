using System.Collections.Generic;
using System.Threading.Tasks;
using Models;

namespace Services
{
#pragma warning disable CS1591
    public interface ICustomerService
    {
        ValueTask<Customer> GetById(int id);
        ValueTask<Customer> CreateCustomer(Customer customer);
        ValueTask<Customer> UpdateCustomer(Customer customer, int id);
        ValueTask<Customer> DeleteCustomer(int id);
        Task<IEnumerable<Customer>> GetByName(string name);
        Task<PaginationWithResult<Customer>> GetAll(UrlQuery urlQuery);
    }
#pragma warning disable CS1591
}