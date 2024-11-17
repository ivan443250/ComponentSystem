namespace ComponentSystem
{
    public interface IInvokableSignalContainer
    {
        void Invoke<T>(T dataSignal) where T : class, IDataSignal;
    }
}
