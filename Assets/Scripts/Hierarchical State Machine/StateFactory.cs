using System;
using System.Collections.Generic;
using System.Linq;
using Bap.State_Machine.Player;
using UnityEngine;

namespace Bap.State_Machine
{
    /// <summary>
    /// Caching all states as a factory
    /// </summary>
    public class StateFactory : MonoBehaviour
    {
        [SerializeField] private PlayerContext _ctx;
        private Dictionary<Type, BaseState> _states = new();

        private void Awake()
        {
            _ctx ??= GetComponent<PlayerContext>();
            try
            {
                _states[typeof(Grounded)] = (BaseState)Activator.CreateInstance(typeof(Grounded), _ctx, this, true);
                _states[typeof(OnAir)] = (BaseState)Activator.CreateInstance(typeof(OnAir), _ctx, this, true);
                _states[typeof(Roll)] = (BaseState)Activator.CreateInstance(typeof(Roll), _ctx, this, true);
                _states[typeof(Idle)] = (BaseState)Activator.CreateInstance(typeof(Idle), _ctx, this, false);
                _states[typeof(Walk)] = (BaseState)Activator.CreateInstance(typeof(Walk), _ctx, this, false);
                _states[typeof(Jump)] = (BaseState)Activator.CreateInstance(typeof(Jump), _ctx, this, false);
                _states[typeof(Fall)] = (BaseState)Activator.CreateInstance(typeof(Fall), _ctx, this, false);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[HFSM] Failed to initialize state factory: {ex.Message}");
                throw; // Rethrow to ensure the issue is noticed during development
            }
        }
        
        public BaseState GetState<T>() where T : BaseState
        {
            if (_states.TryGetValue(typeof(T), out var state))
            {
                return state;
            }
            
            throw new ArgumentException($"State type {typeof(T).Name} not registered");
        }

        public List<string> GetStateList()
        {
            return _states.Values.Select(state => state.GetType().Name + ". IsRoot: " + state.IsRootState).ToList();
        }
    }
}