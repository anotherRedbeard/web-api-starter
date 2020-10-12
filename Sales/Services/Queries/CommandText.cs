using Dapper;
using Models;

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
        public DapperQuery GetCustomers {
            get
            {
                return new DapperQuery() {
                    QueryText = @"select *, customer_id as CustomerId, first_name as FirstName, last_name as LastName, zip_code as ZipCode from sales.customers where 1=1"
                };
            }
        } 
        /// <summary>
        /// GetCustomersById query
        /// </summary>
        public DapperQuery<int> GetCustomersById {
            get
            {
                return new DapperQuery<int>() {
                    QueryText = @"select *, customer_id as CustomerId, first_name as FirstName, last_name as LastName, zip_code as ZipCode from sales.customers where customer_id = @id",
                    Parameters = p => {
                        return new DynamicParameters(new {id=p});
                    }
                };
            }
        } 
        /// <summary>
        /// GetCustomersByName query
        /// </summary>
        /// <returns></returns>
        public DapperQuery<string> GetCustomersByName {
            get
            {
                return new DapperQuery<string>() {
                    QueryText = @"select *, customer_id as CustomerId, first_name as FirstName, last_name as LastName, zip_code as ZipCode 
                                    from sales.customers 
                                    where concat(first_name,' ',last_name) like '%' + @name + '%'",
                    Parameters = p => {
                        return new DynamicParameters(new {name=p});
                    }
                };
            }

        }
        /// <summary>
        /// CreateCustomer query
        /// </summary>
        /// <returns></returns>
        public DapperQuery<Customer> CreateCustomer {
            get
            {
                return new DapperQuery<Customer>() {
                    QueryText = @"insert sales.customers
                                output inserted.*, inserted.customer_id as CustomerId, inserted.first_name as FirstName, inserted.last_name as LastName, inserted.zip_code as ZipCode 
                                values (@firstName,@lastName,@phone,@email,@street,@city,@state,@zipCode)",
                    Parameters = p => {
                        return new DynamicParameters(new {
                            firstName = p.FirstName, 
                            lastName = p.LastName,
                            phone = p.Phone,
                            email = p.Email,
                            street = p.Street,
                            city = p.City,
                            state = p.State,
                            zipCode = p.ZipCode
                        });
                    }
                };
            }
        }

        /// <summary>
        /// UpdateCustomer query
        /// </summary>
        public DapperQuery<Customer,int> UpdateCustomer {
            get
            {
                return new DapperQuery<Customer,int>() {
                    QueryText = @"update sales.customers set first_name = @firstName, last_name = @lastName, phone = @phone, email = @email, street = @street, city = @city, state = @state, zip_code = @zipCode
                                output inserted.*, inserted.customer_id as CustomerId, inserted.first_name as FirstName, inserted.last_name as LastName, inserted.zip_code as ZipCode 
                                where customer_id = @id",
                    Parameters = (p1,p2) => {
                        return new DynamicParameters( new {
                            id = p2,
                            firstName = p1.FirstName,
                            lastName = p1.LastName,
                            phone = p1.Phone,
                            email = p1.Email,
                            street = p1.Street,
                            city = p1.City,
                            state = p1.State,
                            zipCode = p1.ZipCode
                        });
                    }
                };
            }
        }
        /// <summary>
        /// DeleteCustomer query
        /// </summary>
        /// <returns></returns>
        public DapperQuery<int> DeleteCustomer {
            get
            {
                return new DapperQuery<int>() {
                    QueryText = @"delete from sales.customers
                                output deleted.*, deleted.customer_id as CustomerId, deleted.first_name as FirstName, deleted.last_name as LastName, deleted.zip_code as ZipCode 
                                where customer_id = @id",
                    Parameters = (p => {
                        return new DynamicParameters(new {id = p});
                    })
                };
            }
        }
    }
}