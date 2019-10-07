using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using FluentAssertions;
using Queryize.Core;
using Queryize.Test.NorthwindDb;
using Xunit;

namespace Queryize.Test
{
    public class QueryTranslatorTests
    {

        [Fact]
        public void Test_VisitConstant_Int32()
        {
            var constant = Expression.Constant(1, typeof(int));

            var q = new QueryTranslator();

            var c = q.Translate(constant);

            c.Should().Be("1");
        }

        [Fact]
        public void Test_VisitConstant_TrueBool()
        {
            var constant = Expression.Constant(true, typeof(bool));

            var q = new QueryTranslator();

            var c = q.Translate(constant);

            c.Should().Be("1");
        }

        [Fact]
        public void Test_VisitConstant_FalseBool()
        {
            var constant = Expression.Constant(false, typeof(bool));

            var q = new QueryTranslator();

            var c = q.Translate(constant);

            c.Should().Be("0");
        }

        [Fact]
        public void Test_VisitConstant_String()
        {
            var constant = Expression.Constant("hello", typeof(string));

            var q = new QueryTranslator();

            var c = q.Translate(constant);

            c.Should().Be("'hello'");
        }

        [Fact]
        public void Test_VisitConstant_Null()
        {
            var constant = Expression.Constant(null, typeof(string));

            var q = new QueryTranslator();

            var c = q.Translate(constant);

            c.Should().Be("NULL");
        }

        [Fact]
        public void Test_VisitConstant_ObjectThrowsException()
        {
            var customers = new Customers();
            var constant = Expression.Constant(customers, typeof(Customers));

            var q = new QueryTranslator();

            Exception ex = Assert.Throws<NotSupportedException>(() => q.Translate(constant));

            ex.Message.Should().Be("The constant for Queryize.Test.NorthwindDb.Customers is not supported");
        }

        [Fact]
        public void Test_VisitConstant_IQueryable()
        {
            var queryCustomers = new Query<Customers>(new DbQueryProvider(null));
            var constant = Expression.Constant(queryCustomers, typeof(Query<Customers>));

            var q = new QueryTranslator();

            var c = q.Translate(constant);

            c.Should().Be("SELECT * FROM Customers");
        }

        [Fact]
        public void Test_VisitBinary_LessThan()
        {
            var binary = Expression.LessThan(
                Expression.Constant(1, typeof(int)),
                Expression.Constant(2, typeof(int)));

            var queryTranslator = new QueryTranslator();

            var lessThanString = queryTranslator.Translate(binary);

            lessThanString.Should().Be("(1 < 2)");
        }

        [Fact]
        public void Test_VisitBinary_LessThanOrEqual()
        {
            var binary = Expression.LessThanOrEqual(
                Expression.Constant(1, typeof(int)), 
                Expression.Constant(2, typeof(int)));

            var queryTranslator = new QueryTranslator();

            var lessThanOrEqualString = queryTranslator.Translate(binary);

            lessThanOrEqualString.Should().Be("(1 <= 2)");
        }

        [Fact]
        public void Test_VisitBinary_GreaterThan()
        {
            var binary = Expression.GreaterThan(
                Expression.Constant(1, typeof(int)),
                Expression.Constant(2, typeof(int)));

            var queryTranslator = new QueryTranslator();

            var greaterThanString = queryTranslator.Translate(binary);

            greaterThanString.Should().Be("(1 > 2)");
        }
    }
}
