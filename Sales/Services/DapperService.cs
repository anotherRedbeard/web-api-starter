using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Services
{
    /// <summary>
    /// Class to wrap all Dapper implementation details
    /// </summary>
    public abstract class DapperService
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configuration">configuration to get config from</param>
        protected DapperService(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }



        /// <summary>
        /// Creates a connection to run buffered queries that will return a type
        /// </summary>
        /// <param name="getData">Function that will be responsible for executing the query and returning the data</param>
        /// <typeparam name="T">Type that will be returned by the getData function</typeparam>
        /// <returns>returns the results from the getData function</returns>
        protected async Task<T> WithConnection<T>(Func<IDbConnection, Task<T>> getData)
        {
            try
            {
                await using (var connection = new SqlConnection(_connectionString))
                {
                    var azureServiceTokenProvider = new AzureServiceTokenProvider();
                    connection.AccessToken = await azureServiceTokenProvider.GetAccessTokenAsync("https://database.windows.net/");

                    await connection.OpenAsync();
                    return await getData(connection);
                }
            }
            catch (TimeoutException ex)
            {
                throw new Exception(String.Format("{0}.WithConnection() experienced a SQL timeout", GetType().FullName), ex);
            }
            catch (SqlException ex)
            {
                throw new Exception(String.Format("{0}.WithConnection() experienced a SQL exception (not a timeout)", GetType().FullName), ex);
            }
        }

        /// <summary>
        /// Creates a connection to run buffered queries that will not return a type
        /// </summary>
        /// <param name="getData">Function that is responsible for executing the query</param>
        /// <returns>a task is returned</returns>
        protected async Task WithConnection(Func<IDbConnection, Task> getData)
        {
            try
            {
                await using (var connection = new SqlConnection(_connectionString))
                {
                    var azureServiceTokenProvider = new AzureServiceTokenProvider();
                    connection.AccessToken = await azureServiceTokenProvider.GetAccessTokenAsync("https://database.windows.net/");

                    await connection.OpenAsync();
                    await getData(connection);
                }
            }
            catch (TimeoutException ex)
            {
                throw new Exception(String.Format("{0}.WithConnection() experienced a SQL timeout", GetType().FullName), ex);
            }
            catch (SqlException ex)
            {
                throw new Exception(String.Format("{0}.WithConnection() experienced a SQL exception (not a timeout)", GetType().FullName), ex);
            }
        }

        //use for non-buffered queries that return a type
        /// <summary>
        /// Create a connection to run a non-buffered (streaming) query that will return a type
        /// </summary>
        /// <param name="getData">function that is responsible for executing the query and returnings an object to stream</param>
        /// <param name="process">function that will stream the data back to the client</param>
        /// <typeparam name="TRead">object that will return from executing the query</typeparam>
        /// <typeparam name="TResult">result of the streaming process</typeparam>
        /// <returns>final results</returns>
        protected async Task<TResult> WithConnection<TRead, TResult>(Func<IDbConnection, Task<TRead>> getData, Func<TRead, Task<TResult>> process)
        {
            try
            {
                await using (var connection = new SqlConnection(_connectionString))
                {
                    var azureServiceTokenProvider = new AzureServiceTokenProvider();
                    connection.AccessToken = await azureServiceTokenProvider.GetAccessTokenAsync("https://database.windows.net/");

                    await connection.OpenAsync();
                    var data = await getData(connection);
                    return await process(data);
                }
            }
            catch (TimeoutException ex)
            {
                throw new Exception(String.Format("{0}.WithConnection() experienced a SQL timeout", GetType().FullName), ex);
            }
            catch (SqlException ex)
            {
                throw new Exception(String.Format("{0}.WithConnection() experienced a SQL exception (not a timeout)", GetType().FullName), ex);
            }
        }
    }
}