using System;

namespace ComponentSystem
{
    public interface IUnderritableSignalsContainer
    {
        void Subscribe<T>(Action<IDataSignal> onGetSignalAction) where T : class, IDataSignal;
        void Unsubscribe<T>(Action<IDataSignal> onGetSignalAction) where T : class, IDataSignal;
    }
}
