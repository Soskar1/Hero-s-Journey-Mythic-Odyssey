using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HerosJourney.Utils
{
    public abstract class Storage<Key, Value> 
        where Value : class
    {
        private Dictionary<Key, Value> _storage = new Dictionary<Key, Value>();

        protected void Add(Key key, Value value) => _storage.Add(key, value);

        public virtual Value Get(Key key)
        {
            if (_storage.ContainsKey(key))
                return _storage[key];

            Debug.LogError($"Key {key} does not exist in the storage!");
            return null;
        }

        public List<Value> GetValues() => _storage.Values.ToList();
    }
}