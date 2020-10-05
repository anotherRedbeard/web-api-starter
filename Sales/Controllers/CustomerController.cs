using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        Customer[] customers = new Customer[]
        {
            new Customer {CustomerId=1,FirstName="Bob",LastName="Smith",Phone="5555555555",Email="bs@aol.com",Street="123 Any Street",City="Atlanta",State="GA",ZipCode="123456"},
            new Customer {CustomerId=2,FirstName="Tom",LastName="Glavin",Phone="6666666666",Email="tg@aol.com",Street="124 Any Street",City="Atlanta",State="GA",ZipCode="123457"},
        };

        [HttpGet]
        public async Task<IActionResult> ListAllCustomers()
        {
            return Ok(customers);
        }

        [HttpGet("name/{name}")]
        public async Task<IActionResult> ListCustomerByName(string name)
        {
            IEnumerable<Customer> retVal = customers.Where(x => x.FirstName.ToLower().Contains(name.ToLower())).OrderBy(x => x.LastName);

            return Ok(retVal);
        }
    }
}