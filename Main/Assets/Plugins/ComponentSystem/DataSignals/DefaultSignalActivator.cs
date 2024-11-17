namespace ComponentSystem
{
    public class DefaultSignalActivator<T> : ISignalActivator<T> where T : class, IDataSignal
    {
        private IInvokableSignalContainer _signalsContainer;

        public DefaultSignalActivator(IInvokableSignalContainer signalsContainer)
        {
            _signalsContainer = signalsContainer;
        }

        public void Activate(T dataSignal)
        {
            _signalsContainer.Invoke(dataSignal);
        }
    }
}