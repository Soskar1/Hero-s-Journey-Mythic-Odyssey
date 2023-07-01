using Newtonsoft.Json.Converters;
using System;
using UnityEngine;

namespace HerosJourney.Utils
{
    public class SOConverter<T> : CustomCreationConverter<T> where T : ScriptableObject
    {
        public override T Create(Type aObjectType)
        {
            if (typeof(T).IsAssignableFrom(aObjectType))
                return (T)ScriptableObject.CreateInstance(aObjectType);
            return null;
        }
    }
}