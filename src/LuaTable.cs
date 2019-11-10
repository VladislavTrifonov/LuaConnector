using System;
using System.Collections;
using System.Collections.Generic;
using LuaConnector.Exceptions;

namespace LuaConnector
{
    public class LuaTable : IDictionary<object, object>
    {
        private Dictionary<object, object> dict = new Dictionary<object, object>();

        public object this[object key] { get => dict[key]; set => dict[key] = value; }

        public ICollection<object> Keys => dict.Keys;

        public ICollection<object> Values => dict.Values;

        public int Count => dict.Count;

        public bool IsReadOnly => false;

        public void Clear() => dict.Clear();

        public bool Contains(KeyValuePair<object, object> item) => dict.ContainsKey(item.Key) && dict.ContainsValue(item.Value);
        public bool ContainsKey(object key) => dict.ContainsKey(key);

        public void CopyTo(KeyValuePair<object, object>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<object, object>> GetEnumerator() => dict.GetEnumerator();

        public bool Remove(object key) => dict.Remove(key);

        public bool Remove(KeyValuePair<object, object> item) => dict.Remove(item);

        public bool TryGetValue(object key, out object value) => dict.TryGetValue(key, out value);

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)dict).GetEnumerator();

        public void Add(object key, object value)
        {
            ValidateKey(key);
            ValidateValue(value);
            dict.Add(key, value);
        }

        public void Add(KeyValuePair<object, object> item) => Add(item.Key, item.Value);

        private void ValidateKey(object key)
        {
            if (!(key is bool || key is long || key is double || key is string))
                throw new LuaInvalidArgumentException($"Key in LuaTable doesn't {key.ToString()} type");
        }

        private void ValidateValue(object value)
        {
            if (!(value is bool || value is long || value is double || value is string || value is LuaTable))
                throw new LuaInvalidArgumentException($"Value in LuaTable doesn't {value.ToString()} type");

        }
    }
}
