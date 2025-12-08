using System;
using System.Collections.Generic;
using UnityEngine;

namespace CitySim.Core
{
    /// <summary>
    /// Sistema de eventos global desacoplado para comunicação entre sistemas.
    /// Segue o padrão Observer para broadcast de eventos.
    /// </summary>
    public class EventSystem : Singleton<EventSystem>
    {
        private Dictionary<string, Action> _eventListeners = new Dictionary<string, Action>();
        private Dictionary<string, Delegate> _genericEventListeners = new Dictionary<string, Delegate>();

        /// <summary>
        /// Registra um ouvinte para um evento sem parâmetros.
        /// </summary>
        public void Subscribe(string eventName, Action callback)
        {
            if (callback == null) return;

            if (!_eventListeners.ContainsKey(eventName))
            {
                _eventListeners[eventName] = null;
            }
            _eventListeners[eventName] += callback;
        }

        /// <summary>
        /// Remove um ouvinte de um evento sem parâmetros.
        /// </summary>
        public void Unsubscribe(string eventName, Action callback)
        {
            if (callback == null) return;

            if (_eventListeners.ContainsKey(eventName))
            {
                _eventListeners[eventName] -= callback;
                if (_eventListeners[eventName] == null)
                {
                    _eventListeners.Remove(eventName);
                }
            }
        }

        /// <summary>
        /// Registra um ouvinte para um evento com parâmetro genérico.
        /// </summary>
        public void Subscribe<T>(string eventName, Action<T> callback) where T : class
        {
            if (callback == null) return;

            if (!_genericEventListeners.ContainsKey(eventName))
            {
                _genericEventListeners[eventName] = null;
            }
            _genericEventListeners[eventName] = (Action<T>)_genericEventListeners[eventName] + callback;
        }

        /// <summary>
        /// Remove um ouvinte de um evento com parâmetro genérico.
        /// </summary>
        public void Unsubscribe<T>(string eventName, Action<T> callback) where T : class
        {
            if (callback == null) return;

            if (_genericEventListeners.ContainsKey(eventName))
            {
                _genericEventListeners[eventName] = (Action<T>)_genericEventListeners[eventName] - callback;
                if (_genericEventListeners[eventName] == null)
                {
                    _genericEventListeners.Remove(eventName);
                }
            }
        }

        /// <summary>
        /// Dispara um evento sem parâmetros para todos os ouvintes.
        /// </summary>
        public void Emit(string eventName)
        {
            if (_eventListeners.ContainsKey(eventName))
            {
                _eventListeners[eventName]?.Invoke();
            }
        }

        /// <summary>
        /// Dispara um evento com parâmetro genérico para todos os ouvintes.
        /// </summary>
        public void Emit<T>(string eventName, T data) where T : class
        {
            if (_genericEventListeners.ContainsKey(eventName))
            {
                var action = _genericEventListeners[eventName] as Action<T>;
                action?.Invoke(data);
            }
        }

        /// <summary>
        /// Limpa todos os eventos registrados (use com cautela).
        /// </summary>
        public void Clear()
        {
            _eventListeners.Clear();
            _genericEventListeners.Clear();
        }
    }
}
