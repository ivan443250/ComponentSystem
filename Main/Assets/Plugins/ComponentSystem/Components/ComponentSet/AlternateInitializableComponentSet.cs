using System;
using System.Collections.Generic;

namespace ComponentSystem
{
    public class AlternateInitializableComponentSet : IAlternateInitializableCollection
    {
        private IComponent[] _components;

        private List<int> _uninitializedIndexes;

        private Dictionary<Type, List<int>> _typeIndexPairs;

        private ComponentGetter _componentGetter;

        private SignalsBranch _dataSignalsBranch;

        public AlternateInitializableComponentSet(ComponentDictionary componentDictionary, ComponentGetter componentGetter, SignalsBranch dataSignalsBranch)
        {
            _componentGetter = componentGetter;

            _typeIndexPairs = new();
            _uninitializedIndexes = new();

            _dataSignalsBranch = dataSignalsBranch;

            List<IComponent> componetnsList = new();
            foreach (IComponent component in componentDictionary)
            {
                Type componentType = component.GetComponentData().GetAbstractRepresentation();
                int componentIndex = componetnsList.Count;

                _uninitializedIndexes.Add(componentIndex);

                if (_typeIndexPairs.ContainsKey(componentType) == false)
                    _typeIndexPairs.Add(componentType, new());

                _typeIndexPairs[componentType].Add(componentIndex);

                componetnsList.Add(component);
            }
            _components = componetnsList.ToArray();
        }

        public int GetNextUninitializedIndex()
        {
            return _uninitializedIndexes[0];
        }

        public void Initialize(int index)
        {
            _components[index].Initialize(_componentGetter, _dataSignalsBranch.GetNext());
            _uninitializedIndexes.Remove(index);
        }

        public bool IsCollectionInitialized()
        {
            return _uninitializedIndexes.Count == 0;
        }

        public bool IsInitialized(int index)
        {
            return !_uninitializedIndexes.Contains(index);
        }

        public int[] MustInitializeBedfore(int index)
        {
            Type[] dependentTypes = _components[index].GetDependentComponentTypes();

            List<int> answer = new();

            foreach (Type dependentType in dependentTypes) 
            {
                Type needType = dependentType.IsArray ? dependentType.GetElementType() : dependentType;

                if (_typeIndexPairs.ContainsKey(needType) == false)
                    throw new Exception($"{_components[index].GetType()} needs in {needType}, but " +
                        $"component with this type not found");

                answer.AddRange(_typeIndexPairs[needType]);
            }

            return answer.ToArray();
        }
    }
}
