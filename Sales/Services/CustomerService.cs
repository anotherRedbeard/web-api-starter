using System;
using System.Collections.Generic;
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
            string query = _commandText.GetCustomers;
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
                var result = await c.QueryFirstOrDefaultAsync<Customer>(_commandText.GetCustomersById,new {CustomerId = id});
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
                var result = await c.QueryAsync<Customer>(_commandText.GetCustomersByName,new {name = name});
                return result;
            });
        }
    }
}