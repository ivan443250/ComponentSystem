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

        private SignalsBranch _currentBranch;

        private event Action _enable;
        private event Action _disable;

        private bool _isEnabled = false;

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

            _currentBranch = signalsBranch;

            RegisterSignals(signalsBranch);

            SetSignalActivator(signalsBranch);

            InitializeChildren(signalsBranch);

            Initialize();
        }

        public ComponentData GetComponentData()
        {
            return new(_accessModifier, _abstractRepresentation);
        }

        protected virtual void RegisterSignals(ISignalRegistrator signalRegistrator) { }

        protected virtual void Initialize() { }

        protected IDisposable Subscribe<T>(Action<T> onGetSignalAction) where T : class, IDataSignal 
        {
            Action<IDataSignal> dataSignalAction = (ds) => onGetSignalAction.Invoke(ds as T);

            Action subscribeAction = () => _currentBranch.Subscribe<T>(dataSignalAction);
            Action unSubscribeAction = () => _currentBranch.Unsubscribe<T>(dataSignalAction);

            if (_isEnabled)
                subscribeAction.Invoke();

            _enable += subscribeAction;
            _disable += unSubscribeAction;

            return new AbstractUnsubscriber(() => 
            {
                _enable -= subscribeAction;
                _disable -= unSubscribeAction;
            });
        }

        protected T InstanceComponentPrefab<T>(T componentPrefab) where T : MonoBehaviourComponent
        {
            T instance = Instantiate(componentPrefab, transform);
            instance.Initialize(@ComponentGetter, _currentBranch.GetNext());
            instance.OnEnable();
            return instance;
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
            _isEnabled = true;
            _enable?.Invoke();
        }

        private void OnDisable()
        {
            _disable?.Invoke();
        }

        private class AbstractUnsubscriber : IDisposable
        {
            private Action _unsubdcribe;

            public AbstractUnsubscriber(Action unsubdcribe)
            {
                _unsubdcribe = unsubdcribe;
            }

            public void Dispose()
            {
                _unsubdcribe.Invoke();
            }
        }
    }
}