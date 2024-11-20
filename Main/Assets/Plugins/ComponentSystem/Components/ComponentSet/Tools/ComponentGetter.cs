using System;
using System.Collections.Generic;
using System.Linq;

namespace ComponentSystem
{
    public class ComponentGetter : IInsideModifierCollection, IOutsideModifierCollection
    {
        private ComponentDictionary _componentDictionary;

        public ComponentGetter(ComponentDictionary componentDictionary)
        {
            _componentDictionary = componentDictionary;
        }

        public bool TryGetComponent<T>(out T component) where T : class, IComponent
        {
            component = null;

            if (TryGetComponent(out object componentObj, typeof(T), AccessModifier.Everything))
            {
                component = componentObj as T;
                return true;
            }

            return false;
        }

        public bool TryGetComponent(out object component, Type type, AccessModifier accessModifier)
        {
            component = null;

            if (accessModifier == AccessModifier.None)
                return false;

            List<IComponent> components = _componentDictionary.FindComponent(type);

            if (components == null)
                return false;

            List<object> handledComponents = components
                .Select(c => GetHandledComponent(c, accessModifier))
                .Where(obj => obj != null)
                .ToList();

            if (handledComponents.Count == 0)
                return false;

            if (type.IsArray)
            {
                Array arr = Array.CreateInstance(type.GetElementType(), handledComponents.Count);

                for (int i = 0; i < handledComponents.Count; i++)
                    arr.SetValue(handledComponents[i], i);

                component = arr;

                return true;
            }

            component = handledComponents[0];
            return true;
        }

        private object GetHandledComponent(IComponent component, AccessModifier accessModifier)
        {
            if (component == null)
                return null;

            if ((accessModifier & component.GetComponentData().AccessModifier) == 0)
                return null;

            object handledComponent = component is IComponentShell shell ? shell.GetValue() : component;

            return handledComponent;
        }

        private object GetComponent(Type type, AccessModifier accessModifier)
        {
            if (TryGetComponent(out object component, type, accessModifier))
                return component;

            return null;
        }

        object IInsideModifierCollection.GetComponent(Type type)
        {
            return GetComponent(type, AccessModifier.Inside);
        }

        object IOutsideModifierCollection.GetComponent(Type type)
        {
            return GetComponent(type, AccessModifier.Outside);
        }
    }
}
