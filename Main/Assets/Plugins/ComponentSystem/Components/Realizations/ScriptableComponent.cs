using System;
using System.Linq;
using System.Reflection;
using UnityEditor;

namespace ComponentSystem
{
    public sealed class ScriptableComponent : IComponentShell
    {
        private object _instance;

        private Type _targetComponentType;

        private ComponentData _componentData;

        private ConstructorInfo _targetTypeConstructor;

        private Type[] _parameterTypes;

        public ScriptableComponent(Type targetComponentType)
        {
            _targetComponentType = targetComponentType;
            InitializeComponentData(targetComponentType);
        }

        public Type[] GetDependentComponentTypes()
        {
            ConstructorInfo[] constructors = _targetComponentType.GetConstructors();

            if (constructors.Length > 1)
                throw new InvalidOperationException($"Constructors count in type {_targetComponentType} must be no more than 1");

            _targetTypeConstructor = constructors[0];

            _parameterTypes = _targetTypeConstructor
                .GetParameters()
                .Select(parameter => parameter.ParameterType)
                .ToArray();

            return _parameterTypes;
        }

        public void Initialize(IInsideModifierCollection _dependentComponentsSet, SignalsBranch signalsPool)
        {
            object[] parameterValues = new object[_parameterTypes.Length];

            for (int i = 0; i < _parameterTypes.Length; i++)
            {
                parameterValues[i] = _dependentComponentsSet.GetComponent(_parameterTypes[i]);
                if (parameterValues[i] == null)
                    throw new InvalidOperationException($"Component of type {_parameterTypes[i]} " +
                        $"is requred by constructor of type {_targetComponentType}, but this component was not found");
            }

            _instance = _targetTypeConstructor.Invoke(parameterValues);
        }

        public ComponentData GetComponentData() => _componentData;

        public object GetValue()
        {
            return _instance;
        }

        private void InitializeComponentData(Type targetComponentType)
        {
            var accessModAttribute = targetComponentType.GetCustomAttribute<AccessModofierAttribute>();
            AccessModifier currentAccessModofier = accessModAttribute?.Value ?? ComponentData.DefaultAccessModifier;

            var abstractRepresentationAttribute = targetComponentType.GetCustomAttribute<AbstractRepresentationAttribute>();
            Type currentAbstractRepresentation = abstractRepresentationAttribute?.Value ?? targetComponentType;

            _componentData = new ComponentData(currentAccessModofier, currentAbstractRepresentation);
        }
    }
}
