using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Reflection;
using System.Text;

namespace Queryize
{
    internal class ObjectReader<T> : IEnumerable<T>, IEnumerable where T : class, new()
    {
        Enumerator enumerator;

        class Enumerator : IEnumerable<T>, IEnumerator, IDisposable
        {
            DbDataReader reader;
            FieldInfo[] fields;
            int[] fieldLookup;
            T current;

            internal Enumerator(DbDataReader reader)
            {
                this.reader = reader;
                this.fields = typeof(T).GetFields();
            }

            public T Current => current;

            object IEnumerator.Current => current;

            public bool MoveNext()
            {
                if (this.reader.Read())
                {
                    if (this.fieldLookup == null)
                    {
                        this.InitFieldLookup();
                    }

                    T instance = new T();

                    for (int i = 0, n = this.fields.Length; i < n; i++)
                    {
                        int index = this.fieldLookup[i];
                        if (index >= 0)
                        {
                            FieldInfo fi = this.fields[i];
                            if (this.reader.IsDBNull(index))
                            {
                                fi.SetValue(instance, null);
                            }
                            else
                            {
                                fi.SetValue(instance, this.reader.GetValue(index));
                            }
                        }
                    }

                    this.current = instance;
                    return true;
                }
                return false;
            }

            public void Reset()
            {

            }

            public void Dispose()
            {
                reader.Dispose();
            }

            private void InitFieldLookup()
            {
                Dictionary<string, int> map = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase);


                for (int i = 0, n = reader.FieldCount; i < n; i++)
                {
                    map.Add(reader.GetName(i), i);
                }
                fieldLookup = new int[fields.Length];

                for (int i = 0, n = fields.Length; i < n; i++)
                {
                    int index;
                    if (map.TryGetValue(fields[i].Name, out index))
                    {
                        fieldLookup[i] = index;
                    }
                    else
                    {
                        .fieldLookup[i] = -1;
                    }
                }
            }
        }
    }
}
