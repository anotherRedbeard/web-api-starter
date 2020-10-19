using System;
using Dapper;

namespace Services.Queries
{
#pragma warning disable CS1591
    public class DapperQuery
    {
        public string QueryText { get; set; }
    }
    public class DapperQuery<T>
    {
        public string QueryText { get; set; }
        public Func<T, DynamicParameters> Parameters;
    }

    public class DapperQuery<T,U>
    {
        public string QueryText { get; set; }
        public Func<T,U,DynamicParameters> Parameters;
    }
#pragma warning disable CS1591
}