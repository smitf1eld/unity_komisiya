using System.Collections.Generic;
using _Source.GameScript.Interfaces;
using UnityEngine;

namespace _Source.GameScript
{
    public class DayNightCycleManager : MonoBehaviour, IObserverable
    {
        [SerializeField] private float dayDuration = 60f;
        private float _timeOfDay = 0f;

        private List<IObserver> _observers = new List<IObserver>();

        public void RegisterObserver(IObserver observer)
        {
            if (!_observers.Contains(observer))
            {
                _observers.Add(observer);
            }
        }

        public void RemoveObserver(IObserver observer)
        {
            if (_observers.Contains(observer))
            {
                _observers.Remove(observer);
            }
        }

        private void Update()
        {
            _timeOfDay += Time.deltaTime / dayDuration;
            if (_timeOfDay >= 1f)
            {
                _timeOfDay = 0f;
            } 

            NotifyObservers();
        }

        public void NotifyObservers()
        {
            foreach (var observer in _observers)
            {
                observer.UpdateTime(_timeOfDay);
            }
        }
    }
}