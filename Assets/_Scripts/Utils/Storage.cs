using System.Collections.Generic;
using System.Linq;

namespace HerosJourney.Utils
{
    public abstract class Storage<Key, Value>
    {
        private Dictionary<Key, Value> _storage = new Dictionary<Key, Value>();

        protected void Add(Key key, Value value) => _storage.Add(key, value);

        public virtual Value Get(Key key)
        {
            if (_storage.ContainsKey(key))
                return _storage[key];

            throw new System.Exception($"Key {key} does not exist in the storage!");
        }

        public List<Value> GetValues() => _storage.Values.ToList();
    }
}