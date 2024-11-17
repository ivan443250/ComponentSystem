using MyTestGame;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ComponentSystem
{
    public class SignalsBranch : IUnderritableSignalsContainer, IInvokableSignalContainer, ISignalRegistrator
    {
        private Dictionary<Type, int> _LayerIndexByType;
        private List<Dictionary<Type, Action<IDataSignal>>> _allLayers;

        private readonly int _LayerIndex;

        public SignalsBranch()
        {
            _LayerIndexByType = new();

            _allLayers = new();
            _allLayers.Add(new());

            _LayerIndex = 0;
        }

        private SignalsBranch(SignalsBranch signalsBranch)
        {
            _LayerIndexByType = new();
            foreach (var pair in signalsBranch._LayerIndexByType)
                _LayerIndexByType.Add(pair.Key, pair.Value);

            _allLayers = new();
            foreach (var layer in signalsBranch._allLayers)
                _allLayers.Add(layer);

            _allLayers.Add(new());

            _LayerIndex = signalsBranch._LayerIndex + 1;
        }

        public SignalsBranch GetNext()
        {
            return new SignalsBranch(this);
        }

        public void RegisterDataSignal<T>() where T : IDataSignal
        {
            Type signalType = typeof(T);

            if (IsSignalRegistrated(signalType))
                throw new Exception($"Signal with type {signalType} has already been registered");

            _allLayers[_LayerIndex].Add(signalType, (d) => { });
            _LayerIndexByType.Add(signalType, _LayerIndex);
        }

        public void Subscribe<T>(Action<IDataSignal> onGetSignalAction) where T : class, IDataSignal
        {
            Type signalType = typeof(T);

            if (IsSignalRegistrated(signalType) == false)
                throw new Exception($"It is impossible to subscribe to an signal {signalType} in " +
                    $"the current branch with layer index {_LayerIndex}" +
                    $" because this signal has not been registered");

            _allLayers[_LayerIndexByType[signalType]][signalType] += onGetSignalAction;
        }

        public void Unsubscribe<T>(Action<IDataSignal> onGetSignalAction) where T : class, IDataSignal
        {
            Type currentType = typeof(T);

            if (_LayerIndexByType.ContainsKey(currentType) == false)
                return;

            _allLayers[_LayerIndexByType[currentType]][currentType] -= onGetSignalAction;
        }

        public void Invoke<T>(T dataSignal) where T : class, IDataSignal
        {
            Type currentType = typeof(T);

            if (_LayerIndexByType.ContainsKey(currentType) == false)
                return;

            _allLayers[_LayerIndexByType[currentType]][currentType].Invoke(dataSignal);
        }

        public ISignalActivator<T> GetSignalActivator<T>() where T : class, IDataSignal
        {
            if (IsSignalRegistrated<T>() == false)
                throw new Exception($"It is impossible to get activator of signal {typeof(T)} in " +
                    $"the current branch with layer index {_LayerIndex}" +
                    $" because this signal has not been registered");

            return new DefaultSignalActivator<T>(this);
        }

        private bool IsSignalRegistrated<T>() where T : class, IDataSignal
        {
            return IsSignalRegistrated(typeof(T));
        }

        private bool IsSignalRegistrated(Type type)
        {
            return _LayerIndexByType.ContainsKey(type);
        }
    }
}