using System;
using Dapper;

namespace Services.Queries
{
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
}