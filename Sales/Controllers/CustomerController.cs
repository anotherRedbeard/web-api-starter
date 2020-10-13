using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
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
        /// Get a customer by the customer id
        /// </summary>
        /// <param name="id">id of the customer you want to get</param>
        /// <returns>customer you are looking for by id</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomerById(int id)
        {
            var result = await _customerService.GetById(id);

            if (result == null || result.CustomerId < 1)
            {
                return NotFound();
            }
            return Ok(result);
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

        /// <summary>
        /// Create a new customer
        /// </summary>
        /// <param name="customer">new customer to create</param>
        /// <returns>newly created customer</returns>
        [HttpPost]
        public async Task<IActionResult> CreateCustomer(Customer customer)
        {
            var result = await _customerService.CreateCustomer(customer);
            if (result != null && result.CustomerId >= 1)
            {
                Response.Headers.Add("Location",$"{Request.Scheme}://{Request.Host}{Request.Path}/{result.CustomerId}");
            }

            return Ok(result);
        }

        /// <summary>
        /// Update an existing customer
        /// </summary>
        /// <param name="id">customer id of the customer you want to update</param>
        /// <param name="customer">customer information you want to update customer with</param>
        /// <returns>newly updated customer</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(Customer customer, int id)
        {
            //check to see if it exists, return NotFound
            var result = await _customerService.GetById(id);
            if (result == null || result.CustomerId < 1)
            {
                return NotFound();
            }
            //update if exists
            var updateResult = await _customerService.UpdateCustomer(customer,id);
            return Ok(updateResult);
        }

        /// <summary>
        /// Delete an existing customer
        /// </summary>
        /// <param name="id">customer id of the customer you want to update</param>
        /// <returns>customer that was just deleted</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            //check to see if it exists, return NotFound
            var result = await _customerService.GetById(id);
            if (result == null || result.CustomerId < 1)
            {
                return NotFound();
            }

            //delete if exists
            var deleteResult = await _customerService.DeleteCustomer(id);
            return Ok(deleteResult);
        }
    }
}