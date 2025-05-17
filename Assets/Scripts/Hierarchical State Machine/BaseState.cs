using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Bap.State_Machine
{
    public abstract class BaseState
    {
        protected PlayerContext _ctx;
        protected StateFactory _factory;
        protected BaseState _currentSuperState;
        protected BaseState _currentSubState;

        public bool IsRootState;

        public BaseState CurrentSubState => _currentSubState; 
        public BaseState CurrentSuperState => _currentSuperState;

        public BaseState(PlayerContext ctx, StateFactory factory, bool isRoot)
        {
            _ctx = ctx;
            _factory = factory;
            IsRootState = isRoot;
        }

        public void Init()
        {
            Enter();
            InitializeSubState();
        }

        public abstract void Enter();
        public abstract void Update();
        public abstract void Exit();
        protected abstract void CheckTransition();
        public virtual void InitializeSubState(){}

        public void SetSubState(BaseState subState)
        {
            if (subState != null)
            {
                _currentSubState = subState;
                subState.SetSuperState(this);
            }
            else
            {
                Debug.LogError("Set SubState to null");
            }
        }

        protected void SetSuperState(BaseState superState)
        {
            if (superState != null)
            {
                _currentSuperState = superState;
            }
            else
            {
                Debug.LogError("Set SuperState to null");
            }
        }

        public void UpdateStates()
        {
            Update();
            _currentSubState?.UpdateStates();
            CheckTransition();
        }
    }
}