using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models;
using Newtonsoft.Json;
using Services;

namespace Controllers
{
    /// <summary>
    /// Customer endpoint
    /// </summary>
    [ApiController]
    [ApiConventionType(typeof(DefaultApiConventions))]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        /// <summary>
        /// Customer controller constructor
        /// </summary>
        /// <param name="customerService">injected CustomerService to support all customer actions</param>
        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        /// <summary>
        /// Get a list of all customers
        /// </summary>
        /// <returns>IEnumerable of all Customers</returns>
        [HttpGet]
        public async Task<IActionResult> ListAllCustomers([FromQuery] UrlQuery urlQuery)
        {
            try
            {
                var result = await _customerService.GetAll(urlQuery);

                //check to see if we need to include a pagination header
                if (result.Pagination == null)
                {
                    return Ok(result.Results);
                }
                else
                {
                    Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(result.Pagination));
                    return Ok(result.Results);
                }

            }
            catch (ArgumentException ex)
            {
                return BadRequest(new {error = ex.Message, stack = ex.StackTrace.ToString()});
            }
        }

        /// <summary>
        /// Get a list of customers filtered by name
        /// </summary>
        /// <param name="name">full or partial name to filter the customer on</param>
        /// <returns>List of customers that are filtered by incomming name</returns>
        [HttpGet("name/{name}")]
        public async Task<IActionResult> ListCustomerByName(string name)
        {
            var result = await _customerService.GetByName(name);
            return Ok(result);
        }
    }
}