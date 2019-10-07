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

                IQueryable<Customers> query =
                    db.Customers.Where(c => c.City == "London");

                Console.WriteLine($"Query:\n{query.ToString()}");

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
