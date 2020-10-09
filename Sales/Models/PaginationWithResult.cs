using System;
using System.Collections.Generic;

namespace Models
{
#pragma warning disable CS1591
    public class PaginationWithResult<T>
    {
        public IEnumerable<T> Results { get; set; }
        public Pagination Pagination { get; set; }
    }
#pragma warning disable CS1591
}
