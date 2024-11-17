using System;
using System.Collections.Generic;

namespace ComponentSystem
{
    public class SignalsBranch : IUnderritableSignalsContainer, IInvokableSignalContainer
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

        public void Subscribe<T>(Action<IDataSignal> onGetSignalAction) where T : class, IDataSignal
        {
            RegisterDataSignal<T>();

            Type currentType = typeof(T);

            _allLayers[_LayerIndexByType[currentType]][currentType] += onGetSignalAction;
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
            RegisterDataSignal<T>();

            return new DefaultSignalActivator<T>(this);
        }

        private void RegisterDataSignal<T>() where T : IDataSignal
        {
            Type signalType = typeof(T);

            if (_LayerIndexByType.ContainsKey(signalType))
                return;

            _allLayers[_LayerIndex].Add(signalType, (d) => { });
            _LayerIndexByType.Add(signalType, _LayerIndex);
        }
    }
}