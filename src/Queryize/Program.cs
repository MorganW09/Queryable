using System;
using System.Data.SqlClient;
using System.Linq;
using Queryize.NorthwindDb;

namespace Queryize
{
    class Program
    {
        static void Main(string[] args)
        {
            string dbConnectionString = @"Server=(localdb)\mssqllocaldb;Database=Northwind;Trusted_Connection=True;MultipleActiveResultSets=true";
            using (SqlConnection con = new SqlConnection(dbConnectionString))
            {
                con.Open();
                var db = new Northwind(con);

                var city = "London";

                IQueryable<Customers> query =
                    db.Customers.Where(c => c.City == city);

                Console.WriteLine($"Query: {query.Expression.ToString()}");
                //Console.WriteLine($"Query:\n{query.ToString()}");

                var list = query.ToList();

                foreach (var item in list)
                {
                    Console.WriteLine("Name: {0}", item.ContactName);
                }
            }

            Console.ReadLine();
        }
    }
}
