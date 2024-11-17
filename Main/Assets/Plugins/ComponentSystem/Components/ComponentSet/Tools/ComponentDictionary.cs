using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ComponentSystem
{
    public class ComponentDictionary : IEnumerable<IComponent>
    {
        private Dictionary<Type, IComponent> _uniqueComponents;
        private Dictionary<Type, List<IComponent>> _repeatableComponents;

        public ComponentDictionary()
        {
            _uniqueComponents = new();
            _repeatableComponents = new();
        }

        public ComponentDictionary(IEnumerable<IComponent> components)
        {
            Initialize(components);
        }

        public IEnumerator<IComponent> GetEnumerator()
        {
            foreach (KeyValuePair<Type, IComponent> pair in _uniqueComponents)
                yield return pair.Value;

            foreach (KeyValuePair<Type, List<IComponent>> pair in _repeatableComponents)
                foreach (IComponent component in pair.Value)
                    yield return component;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public static ComponentDictionary operator +(ComponentDictionary dictionary1, ComponentDictionary dictionary2)
        {
            return new ComponentDictionary(dictionary1.Union(dictionary2));
        }

        public List<IComponent> FindComponent(Type type)
        {
            List<IComponent> needComponents = new();

            if (type.IsArray == false)
            {
                if (_uniqueComponents.ContainsKey(type) == false)
                    throw new Exception($"Component of type {type} must be single");

                needComponents.Add(_uniqueComponents[type]);
                return needComponents;
            }

            Type elementType = type.GetElementType();

            if (_uniqueComponents.ContainsKey(elementType))
            {
                needComponents.Add(_uniqueComponents[elementType]);
                return needComponents;
            }

            if (_repeatableComponents.ContainsKey(elementType) == false)
                return null;

            needComponents.AddRange(_repeatableComponents[elementType]);
            return needComponents;
        }

        private void Initialize(IEnumerable<IComponent> components)
        {
            _uniqueComponents = new();
            _repeatableComponents = new();

            foreach (IComponent component in components)
            {
                if (component == null)
                    throw new NullReferenceException("Component in installer is null");

                Type type = component.GetComponentData().GetAbstractRepresentation();

                if (_repeatableComponents.ContainsKey(type))
                {
                    _repeatableComponents[type].Add(component);
                    continue;
                }

                if (_uniqueComponents.ContainsKey(type) == false)
                {
                    _uniqueComponents.Add(type, component);
                    continue;
                }

                IComponent repeatedComponent = _uniqueComponents[type];
                _uniqueComponents.Remove(type);

                if (_repeatableComponents.ContainsKey(type) == false)
                    _repeatableComponents.Add(type, new());

                _repeatableComponents[type].Add(repeatedComponent);
                _repeatableComponents[type].Add(component);
            }
        }
    }
}
