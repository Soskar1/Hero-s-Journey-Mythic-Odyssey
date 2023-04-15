using System.Collections.Generic;
using System;
using UnityEngine;

namespace HerosJourney.Core.Databases
{
    public abstract class Database<T> : MonoBehaviour where T : class
    {
        private Dictionary<int, T> _database;

        public virtual T Get(IIdentifiable identifiable)
        {
            T result;

            if (_database.TryGetValue(identifiable.Id, out result) == false)
                throw new NullReferenceException();

            return result;
        }

        public virtual void Add(IIdentifiable identifiable, T value) => _database.TryAdd(identifiable.Id, value);
    }
}