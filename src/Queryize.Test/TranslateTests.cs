using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using FluentAssertions;
using Queryize.Core;
using Queryize.Test.NorthwindDb;
using Xunit;

namespace Queryize.Test
{
    public class TranslateTests
    {
        [Fact]
        public void Test_CityTranslation()
        {
            var translation = string.Empty;
            string dbConnectionString = @"Server=(localdb)\\mssqllocaldb;Database=Northwind;Trusted_Connection=True;MultipleActiveResultSets=true";
            using (SqlConnection con = new SqlConnection(dbConnectionString))
            {

                Northwind db = new Northwind(con);
                var query =
                            db.Customers.Where(c => c.City == "London");
                
                translation = new QueryTranslator().Translate(query.Expression);
            }

            translation.Should().Be("SELECT * FROM (SELECT * FROM Customers) AS T WHERE(City = 'London')");

        }

        [Fact]
        public void Test_CityAndCountryTranslation()
        {
            var translation = string.Empty;
            string dbConnectionString = @"Server=(localdb)\\mssqllocaldb;Database=Northwind;Trusted_Connection=True;MultipleActiveResultSets=true";
            using (SqlConnection con = new SqlConnection(dbConnectionString))
            {

                Northwind db = new Northwind(con);
                var query =
                            db.Customers.Where(c => c.City == "London" && c.Country == "UK");

                translation = new QueryTranslator().Translate(query.Expression);
            }

            translation.Should().Be("SELECT * FROM (SELECT * FROM Customers) AS T WHERE((City = 'London') AND (Country = 'UK'))");
        }

        [Fact]
        public void Test_CityCountryAndCompanyNameTranslation()
        {
            var translation = string.Empty;
            string dbConnectionString = @"Server=(localdb)\\mssqllocaldb;Database=Northwind;Trusted_Connection=True;MultipleActiveResultSets=true";
            using (SqlConnection con = new SqlConnection(dbConnectionString))
            {

                Northwind db = new Northwind(con);
                var query =
                            db.Customers.Where(c => c.City == "London" && c.Country == "UK" && c.ContactName == "Thomas Hardy");

                translation = new QueryTranslator().Translate(query.Expression);
            }

            translation.Should().Be("SELECT * FROM (SELECT * FROM Customers) AS T WHERE(((City = 'London') AND (Country = 'UK')) AND (ContactName = 'Thomas Hardy'))");
        }

        [Fact]
        public void Test_CityCountryCompanyNameAndPhoneTranslation()
        {
            var translation = string.Empty;
            string dbConnectionString = @"Server=(localdb)\\mssqllocaldb;Database=Northwind;Trusted_Connection=True;MultipleActiveResultSets=true";
            using (SqlConnection con = new SqlConnection(dbConnectionString))
            {

                Northwind db = new Northwind(con);
                var query = db.Customers.Where(c => c.City == "London" 
                    && c.Country == "UK"
                    && c.ContactName == "Thomas Hardy"
                    && c.Phone == "1234567890");

                translation = new QueryTranslator().Translate(query.Expression);
            }

            translation.Should().Be("SELECT * FROM (SELECT * FROM Customers) AS T WHERE((((City = 'London') AND (Country = 'UK')) AND (ContactName = 'Thomas Hardy')) AND (Phone = '1234567890'))");
        }
    }
}
