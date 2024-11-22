using System;

namespace ComponentSystem
{
    //todo
    public class SignalRegistrtor : MonoBehaviourComponent
    {
        //[SerializeField] private MonoScript[] _signalScripts;

        protected override void RegisterSignals(ISignalRegistrator signalRegistrator)
        {
            //foreach (MonoScript script in _signalScripts)
            //{
            //    Type signalType = script.GetClass();

            //    if (typeof(IDataSignal).IsAssignableFrom(signalType) == false)
            //        throw new Exception($"Script {script.name} is not a representation of the IDataSignal interface");

            //    MethodInfo methodInfo =
            //        typeof(ISignalRegistrator)
            //        .GetMethod(nameof(ISignalRegistrator.RegisterDataSignal))
            //        .MakeGenericMethod(signalType);

            //    methodInfo.Invoke(signalRegistrator, new object[0]);
            //}
            throw new NotImplementedException();
        }
    }
}
