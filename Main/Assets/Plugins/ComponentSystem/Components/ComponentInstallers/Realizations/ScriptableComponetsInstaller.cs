using System;
using UnityEngine;

namespace ComponentSystem
{
    //todo
    public class ScriptableComponetsInstaller : MonoBehaviour, IOutsideComponentInstaller
    {
        //[SerializeField] private ScriptableComponentContainer[] _containers;

        public IComponent[] InstallOutside()
        {
            //return _containers.Select(container => container.ComponentShell).ToArray();
            throw new NotImplementedException();
        }
    }

    //[Serializable]
    //public class ScriptableComponentContainer
    //{
    //    public ScriptableComponent ComponentShell => new(_script.GetClass());

    //    [SerializeField] private MonoScript _script;
    //}
}
