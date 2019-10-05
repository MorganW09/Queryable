using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace Queryize.NorthwindDb
{
    public class Northwind
    {
        public Query<Customers> Customers;
        public Query<Orders> Orders;

        public Northwind(DbConnection connection)
        {
            QueryProvider provider = new DbQueryProvider(connection);
            Customers = new Query<Customers>(provider);
            Orders = new Query<Orders>(provider);
        }
    }
}
