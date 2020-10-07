using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using Models;
using Services.Queries;

namespace Services
{
    public class CustomerService : DapperService, ICustomerService
    {
        private readonly ICommandText _commandText;
        public CustomerService(IConfiguration configuration, ICommandText commandText) : base(configuration)
        {
            _commandText = commandText;
        }

        public async Task<IEnumerable<Customer>> GetAll()
        {
            return await WithConnection(async c => {
                var result = await c.QueryAsync<Customer>(_commandText.GetCustomers);
                return result;
            });
        }

        public async ValueTask<Customer> GetById(int id)
        {
            return await WithConnection(async c => {
                var result = await c.QueryFirstOrDefaultAsync<Customer>(_commandText.GetCustomersById,new {CustomerId = id});
                return result;
            });
        }
    }
}