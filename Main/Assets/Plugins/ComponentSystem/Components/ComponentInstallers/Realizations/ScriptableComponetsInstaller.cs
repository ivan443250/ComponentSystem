using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace ComponentSystem
{
    public class ScriptableComponetsInstaller : MonoBehaviour, IOutsideComponentInstaller
    {
        [SerializeField] private ScriptableComponentContainer[] _containers;

        public IComponent[] InstallOutside()
        {
            return _containers.Select(container => container.ComponentShell).ToArray();
        }
    }

    [Serializable]
    public class ScriptableComponentContainer
    {
        public ScriptableComponent ComponentShell => new(_script.GetClass());

        [SerializeField] private MonoScript _script;
    }
}
