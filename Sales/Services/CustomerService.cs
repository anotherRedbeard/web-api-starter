using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using Models;

namespace Services
{
    public class CustomerService : DapperService, ICustomerService
    {
        public CustomerService(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<IEnumerable<Customer>> GetAll()
        {
            return await WithConnection(async c => {
                var result = await c.QueryAsync<Customer>("select * from sales.customers");
                return result;
            });
        }

        public async ValueTask<Customer> GetById(int id)
        {
            return await WithConnection(async c => {
                var result = await c.QueryFirstOrDefaultAsync<Customer>("select * from sales.customer where customerid = @id",new {CustomerId = id});
                return result;
            });
        }
    }
}