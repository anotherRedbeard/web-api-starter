using Models;

namespace Services.Queries
{
#pragma warning disable CS1591
    public interface ICommandText
    {
        DapperQuery GetCustomers {get;}
        DapperQuery<int> GetCustomersById {get;}
        DapperQuery<string> GetCustomersByName {get;}
        DapperQuery<Customer> CreateCustomer {get;}
        DapperQuery<Customer,int> UpdateCustomer {get;}
        DapperQuery<int> DeleteCustomer {get;}
    }
#pragma warning disable CS1591
}