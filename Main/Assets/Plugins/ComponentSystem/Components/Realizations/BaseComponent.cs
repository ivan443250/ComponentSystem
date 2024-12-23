using System;
using UnityEngine;

namespace ComponentSystem
{
    public class BaseComponent : IComponent
    {
        private ComponentState _currentState;

        public BaseComponent()
        {
            _currentState = ComponentState.Initialization;
        }

        public ComponentData GetComponentData()
        {
            throw new NotImplementedException();
        }

        public Type[] GetDependentComponentTypes()
        {
            throw new NotImplementedException();
        }

        public ComponentState GetComponentState()
        {
            return _currentState;
        }

        public virtual void Initialize(IInsideModifierCollection _dependentComponentsSet, SignalsBranch signalsPool)
        {
            _currentState = ComponentState.DefaultState;
        }

        public void Enable()
        {

        }

        public void Disable()
        {

        }

        public void Destroy()
        {
            if (_currentState == ComponentState.Destroying)
                return;

            Disable();
            _currentState = ComponentState.Destroying;
        }
    }

    public enum ComponentState
    {
        Initialization,
        DefaultState,
        Destroying
    }
}
