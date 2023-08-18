using System;
using Unity.Collections;

namespace HerosJourney.Utils
{
    public abstract class TSStorage<Key, Value>
        where Key : struct, IEquatable<Key>
        where Value : struct
    {
        private NativeHashMap<Key, Value> _storage;

        protected void Add(Key key, Value value) => _storage.Add(key, value);

        public virtual Value Get(Key key)
        {
            if (_storage.ContainsKey(key))
                return _storage[key];

            throw new Exception($"Key {key} does not exist in the storage!");
        }

        public NativeArray<Value> GetValues(Allocator allocator) => _storage.GetValueArray(allocator);
    }
}