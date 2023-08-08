using System;
using UnityEngine;

namespace HerosJourney.Core.WorldGeneration
{
    public static class Timer
    {
        private static bool _isActive = false;

        public static void Start(float time, Action action)
        {
            if (_isActive)
                return;

            _isActive = true;

            float elapsedTime = 0;

            while (elapsedTime < time)
                elapsedTime += Time.deltaTime;

            action.Invoke();

            _isActive = false;
        }
    }
}