namespace ComponentSystem
{
    public interface ISignalRegistrator
    {
        void RegisterDataSignal<T>() where T : IDataSignal;
    }
}
