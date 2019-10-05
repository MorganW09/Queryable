using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Queryize
{
    public class DbQueryProvider : QueryProvider
    {
        DbConnection Connection;

        public DbQueryProvider(DbConnection connection)
        {
            Connection = connection;
        }

        public override object Execute(Expression expression)
        {
            DbCommand cmd = Connection.CreateCommand();

            cmd.CommandText = this.Translate(expression);

            DbDataReader reader = cmd.ExecuteReader();

            Type elementType = TypeSystem.GetElementType(expression.Type);

            return Activator.CreateInstance(

                typeof(ObjectReader<>).MakeGenericType(elementType),
                BindingFlags.Instance | BindingFlags.NonPublic, null,
                new object[] { reader },
                null);
        }

        public override string GetQueryText(Expression expression) => Translate(expression);

        internal string Translate(Expression expression) => new QueryTranslator().Translate(expression);
    }
}
