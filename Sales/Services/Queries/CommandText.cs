namespace Services.Queries
{
    public class CommandText : ICommandText
    {
        public string GetCustomers => @"select *, customer_id as CustomerId, first_name as FirstName, last_name as LastName, zip_code as ZipCode from sales.customers where 1=1";
        public string GetCustomersById => @"select *, customer_id as CustomerId, first_name as FirstName, last_name as LastName, zip_code as ZipCode from sales.customers where customerid = @id";
    }
}