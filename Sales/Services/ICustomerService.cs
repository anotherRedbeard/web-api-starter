using System.Collections.Generic;
using System.Threading.Tasks;
using Models;

namespace Services
{
    public interface ICustomerService
    {
        ValueTask<Customer> GetById(int id);
        Task<IEnumerable<Customer>> GetAll();
    }
}