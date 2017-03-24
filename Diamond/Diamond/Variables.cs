using Diamond.Formulas;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diamond
{
    public class Variables : IDictionary<string, Value>
    {
        private Func<string, Value> Lookup { get; set; }

        public Variables(Func<string, Value> lookup)
        {
            Lookup = lookup;
        }

        public Value this[string key]
        {
            get
            {
                return Lookup(key) ?? new Value(new MissingVariables(key));
            }

            set
            {
                throw new InvalidOperationException();
            }
        }

        public int Count
        {
            get
            {
                return 1; //Just lie and say that there is always at least one.
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return true;
            }
        }

        public ICollection<string> Keys
        {
            get
            {
                throw new InvalidOperationException();
            }
        }

        public ICollection<Value> Values
        {
            get
            {
                throw new InvalidOperationException();
            }
        }

        public void Add(KeyValuePair<string, Value> item)
        {
            throw new InvalidOperationException();
        }

        public void Add(string key, Value value)
        {
            throw new InvalidOperationException();
        }

        public void Clear()
        {
            throw new InvalidOperationException();
        }

        public bool Contains(KeyValuePair<string, Value> item)
        {
            throw new InvalidOperationException();
        }

        public bool ContainsKey(string key)
        {
            return true;
        }

        public void CopyTo(KeyValuePair<string, Value>[] array, int arrayIndex)
        {
            throw new InvalidOperationException();
        }

        public IEnumerator<KeyValuePair<string, Value>> GetEnumerator()
        {
            throw new InvalidOperationException();
        }

        public bool Remove(KeyValuePair<string, Value> item)
        {
            throw new InvalidOperationException();
        }

        public bool Remove(string key)
        {
            throw new InvalidOperationException();
        }

        public bool TryGetValue(string key, out Value value)
        {
            throw new InvalidOperationException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new InvalidOperationException();
        }
    }
}
