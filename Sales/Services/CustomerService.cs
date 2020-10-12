using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using Models;
using Services.Queries;
using static Dapper.SqlMapper;

namespace Services
{
    /// <summary>
    /// CustomerService that supports all the interaction with the data source for Customers
    /// </summary>
    public class CustomerService : DapperService, ICustomerService
    {
        private readonly ICommandText _commandText;
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="configuration">configuration</param>
        /// <param name="commandText">queries</param>
        /// <returns></returns>
        public CustomerService(IConfiguration configuration, ICommandText commandText) : base(configuration)
        {
            _commandText = commandText;
        }

        /// <summary>
        /// GetAll functionality and handles paging via urlQuery
        /// </summary>
        /// <param name="urlQuery">handles paging for the queries</param>
        /// <returns>customers properly paged if requested, or all if not requested</returns>
        public async Task<PaginationWithResult<Customer>> GetAll(UrlQuery urlQuery)
        {
            string query = _commandText.GetCustomers.QueryText;
            IEnumerable<Customer> customers = null;
            Pagination pagination = null;
            int? totalRecords = null;
            return await WithConnection(async c => {
                if (urlQuery.PageSize.HasValue)
                {
                    query += @" order by customers.customer_id
                                    offset @PageSize * (@PageNumber - 1) rows
                                    fetch next @PageSize rows only"; 

                    if(urlQuery.IncludeCount)
                    {
                        query += " select [totalCount] = count(*) from sales.customers";
                    }
                }
                using (GridReader results = c.QueryMultiple(query, urlQuery))
                {
                    customers = await results.ReadAsync<Customer>();

                    if (urlQuery.IncludeCount)
                    {
                        totalRecords = await results.ReadSingleAsync<int>();
                    }
                }

                if (urlQuery.PageSize.HasValue)
                {
                    pagination = new Pagination()
                    {
                        PageNumber = urlQuery.PageNumber.Value,
                        PageSize = urlQuery.PageSize.Value
                    };
                    if (urlQuery.IncludeCount)
                    {
                        pagination.TotalRecords = totalRecords.Value;
                    }
                }

                return new PaginationWithResult<Customer>()
                {
                    Results = customers,
                    Pagination = pagination
                };
            });
        }

        /// <summary>
        /// Get customers filtered by id
        /// </summary>
        /// <param name="id">customer id of the customer you are looking for</param>
        /// <returns>customer filtered by id</returns>
        public async ValueTask<Customer> GetById(int id)
        {
            return await WithConnection(async c => {
                string query = _commandText.GetCustomersById.QueryText;
                DynamicParameters dbParams = _commandText.GetCustomersById.Parameters(id);
                var result = await c.QueryFirstOrDefaultAsync<Customer>(query,dbParams);
                return result;
            });
        }

        /// <summary>
        /// Get customers filtered by name
        /// </summary>
        /// <param name="name">full or partial name of the customer(s) you are looking</param>
        /// <returns>customers filtered by name</returns>
        public async Task<IEnumerable<Customer>> GetByName(string name)
        {
            return await WithConnection(async c => {
                string query = _commandText.GetCustomersByName.QueryText;
                DynamicParameters dbParams = _commandText.GetCustomersByName.Parameters(name);
                var result = await c.QueryAsync<Customer>(query,dbParams);

                return result;
            });
        }

        /// <summary>
        /// Create customer
        /// </summary>
        /// <param name="customer">new customer you would like to create</param>
        /// <returns>newly created customer</returns>
        public async ValueTask<Customer> CreateCustomer(Customer customer)
        {
            return await WithConnection(async c => {
                string query = _commandText.CreateCustomer.QueryText;
                DynamicParameters dbParams = _commandText.CreateCustomer.Parameters(customer);
                var result = await c.QueryFirstOrDefaultAsync<Customer>(query, dbParams);

                return result;
            });
        }

        /// <summary>
        /// Update customer
        /// </summary>
        /// <param name="customer">new customer values you want to update to</param>
        /// <param name="id">customerId of the customer you want to update</param>
        /// <returns>newly updated customer</returns>
        public async ValueTask<Customer> UpdateCustomer(Customer customer, int id)
        {
            return await WithConnection(async c => {
                string query = _commandText.UpdateCustomer.QueryText;
                DynamicParameters dbParams = _commandText.UpdateCustomer.Parameters(customer,id);
                var result = await c.QueryFirstOrDefaultAsync<Customer>(query,dbParams);

                return result;
            });
        }

        /// <summary>
        /// delete customer
        /// </summary>
        /// <param name="id">id of the customer you want to delete</param>
        /// <returns>deleted customer</returns>
        public async ValueTask<Customer> DeleteCustomer(int id)
        {
            return await WithConnection(async c => {
                string query = _commandText.DeleteCustomer.QueryText;
                DynamicParameters dbParams = _commandText.DeleteCustomer.Parameters(id);

                var result = await c.QueryFirstOrDefaultAsync<Customer>(query, dbParams);

                return result;
            });
        }
    }
}