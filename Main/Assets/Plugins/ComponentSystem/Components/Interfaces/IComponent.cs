using System;

namespace ComponentSystem
{
    public interface IComponent
    {
        Type[] GetDependentComponentTypes();

        void Initialize(IInsideModifierCollection _dependentComponentsSet, SignalsBranch signalsPool);

        ComponentData GetComponentData();
    }
}
