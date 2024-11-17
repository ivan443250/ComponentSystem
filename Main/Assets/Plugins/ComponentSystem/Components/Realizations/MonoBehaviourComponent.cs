using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace ComponentSystem
{
    public class MonoBehaviourComponent : MonoBehaviour, IComponent
    {
        protected ComponentGetter @ComponentGetter => _componentSet.Getter;

        protected virtual Type _abstractRepresentation => GetType();
        [SerializeField] private AccessModifier _accessModifier;

        private FieldInfo[] _fieldsWithDependentComponents;

        private ComponentSet _componentSet;

        private IUnderritableSignalsContainer _underritableSignalsContainer;

        private event Action _enable;
        private event Action _disable;

        public Type[] GetDependentComponentTypes()
        {
            if (_fieldsWithDependentComponents == null)
                _fieldsWithDependentComponents = 
                    GetType()
                    .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                    .Where((field) => field.GetCustomAttribute<GetComponentAttribute>() != null)
                    .ToArray();

            return _fieldsWithDependentComponents
                .Select((field) =>
                {
                    Type typeToReturn = field.FieldType;

                    if (typeToReturn.IsArray)
                        typeToReturn = typeToReturn.GetElementType();

                    return typeToReturn;
                })
                .ToArray();
        }

        public void Initialize(IInsideModifierCollection _dependentComponentsSet, SignalsBranch signalsBranch)
        {
            if (_fieldsWithDependentComponents != null)
                foreach (FieldInfo field in _fieldsWithDependentComponents)
                    field.SetValue(this, _dependentComponentsSet.GetComponent(field.FieldType));

            _underritableSignalsContainer = signalsBranch;

            RegisterSignals(signalsBranch);

            SetSignalActivator(signalsBranch);

            Initialize();

            InitializeChildren(signalsBranch);
        }

        public ComponentData GetComponentData()
        {
            return new(_accessModifier, _abstractRepresentation);
        }

        protected virtual void RegisterSignals(ISignalRegistrator signalRegistrator) { }

        protected virtual void Initialize() { }

        protected void Subscribe<T>(Action<T> onGetSignalAction) where T : class, IDataSignal 
        {
            Action<IDataSignal> dataSignalAction = (ds) => onGetSignalAction.Invoke(ds as T);

            _underritableSignalsContainer.Subscribe<T>(dataSignalAction);

            _enable += () => _underritableSignalsContainer.Subscribe<T>(dataSignalAction);
            _disable += () => _underritableSignalsContainer.Unsubscribe<T>(dataSignalAction);
        }

        private IComponent[] GetChildrenComponents()
        {
            if (TryGetComponent(out IInsideComponentInstaller insideComponentInstaller) == false)
                return new IComponent[0];

            return insideComponentInstaller.InstallInside(this);
        }

        private void InitializeChildren(SignalsBranch signalsPool)
        {
            _componentSet = new(GetChildrenComponents());
            _componentSet.InitializeComponents(signalsPool);
        }

        private void SetSignalActivator(SignalsBranch signalsPool)
        {
            IEnumerable<FieldInfo> signalActivatorFields = 
                GetType()
                .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where((field) => field.GetCustomAttribute<GetSignalActivatorAttribute>() != null);

            foreach (FieldInfo field in signalActivatorFields)
            {
                Type dataSignalType = field.FieldType.GetGenericArguments()[0];

                MethodInfo getActivator = 
                    typeof(SignalsBranch)
                    .GetMethod(nameof(SignalsBranch.GetSignalActivator))
                    .MakeGenericMethod(dataSignalType);

                object value = getActivator.Invoke(signalsPool, new object[0]);

                field.SetValue(this, value);
            }
        }

        private void OnEnable()
        {
            _enable?.Invoke();
        }

        private void OnDisable()
        {
            _disable?.Invoke();
        }
    }
}