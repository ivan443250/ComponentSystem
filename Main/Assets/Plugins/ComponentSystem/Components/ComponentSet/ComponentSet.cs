using System;
using System.Collections.Generic;

namespace ComponentSystem
{
    public sealed class ComponentSet
    {
        public ComponentGetter Getter => _componentGetter;

        private ComponentDictionary _componentDictionary;
        private ComponentGetter _componentGetter;

        public ComponentSet(ComponentDictionary componentDictionary)
        {
            _componentDictionary = componentDictionary;
            _componentGetter = new(_componentDictionary); 
        }

        public ComponentSet() 
            : this(new ComponentDictionary()) { }

        public ComponentSet(IEnumerable<IComponent> components) 
            : this (new ComponentDictionary(components)) { }

        public void InitializeComponents(SignalsBranch dataSignalsBranch)
        {
            AlternateInitializator
                .InitializeAlternately(
                new AlternateInitializableComponentSet(_componentDictionary, _componentGetter, dataSignalsBranch));
        }
    }

    public interface IInsideModifierCollection
    {
        object GetComponent(Type type);
    }

    public interface IOutsideModifierCollection
    {
        object GetComponent(Type type);
    }
}
