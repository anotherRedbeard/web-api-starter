namespace Services.Queries
{
    /// <summary>
    /// Location to store all the base queries for the api
    /// </summary>
    public class CommandText : ICommandText
    {
        /// <summary>
        /// GetCustomers query
        /// </summary>
        public string GetCustomers => @"select *, customer_id as CustomerId, first_name as FirstName, last_name as LastName, zip_code as ZipCode from sales.customers where 1=1";
        /// <summary>
        /// GetCustomersById query
        /// </summary>
        public string GetCustomersById => @"select *, customer_id as CustomerId, first_name as FirstName, last_name as LastName, zip_code as ZipCode from sales.customers where customerid = @id";
        /// <summary>
        /// GetCustomersByName query
        /// </summary>
        /// <returns></returns>
        public string GetCustomersByName => @"select *, customer_id as CustomerId, first_name as FirstName, last_name as LastName, zip_code as ZipCode 
                                                from sales.customers 
                                                where concat(first_name,' ',last_name) like '%' + @name + '%'";
    }
}