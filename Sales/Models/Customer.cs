using System.ComponentModel.DataAnnotations;

namespace Models
{
    /// <summary>
    /// Customer object
    /// </summary>
    public class Customer
    {
        /// <summary>
        /// CustomerId
        /// </summary>
        /// <value></value>
        public int CustomerId { get; set; }
        /// <summary>
        /// FirstName
        /// </summary>
        /// <value></value>
        public string FirstName { get; set; }
        /// <summary>
        /// LastName
        /// </summary>
        /// <value></value>
        public string LastName { get; set; }
        /// <summary>
        /// Phone
        /// </summary>
        /// <value></value>
        public string Phone { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        /// <value></value>
        public string Email { get; set; }
        /// <summary>
        /// Street
        /// </summary>
        /// <value></value>
        public string Street { get; set; }
        /// <summary>
        /// City
        /// </summary>
        /// <value></value>
        public string City { get; set; }
        /// <summary>
        /// State
        /// </summary>
        /// <value></value>
        public string State { get; set; }
        /// <summary>
        /// ZipCode
        /// </summary>
        /// <value></value>
        public string ZipCode { get; set; }
    }
}