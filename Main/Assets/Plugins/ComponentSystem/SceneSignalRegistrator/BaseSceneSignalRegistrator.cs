using UnityEngine;

namespace ComponentSystem
{
    public abstract class BaseSceneSignalRegistrator : MonoBehaviour
    {
        public abstract void RegisterSignals(ISignalRegistrator signalRegistrator);
    }
}
