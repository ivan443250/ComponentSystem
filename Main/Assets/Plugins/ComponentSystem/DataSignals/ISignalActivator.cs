namespace ComponentSystem
{
    public interface ISignalActivator<T> where T : class, IDataSignal
    {
        void Activate(T dataSignal);
    }
}
