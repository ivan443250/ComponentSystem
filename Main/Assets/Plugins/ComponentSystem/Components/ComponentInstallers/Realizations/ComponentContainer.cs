using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ComponentSystem
{
    public class ComponentContainer : MonoBehaviour, IOutsideComponentInstaller
    {
        [SerializeField] private bool _installOnThisGameObject;
        [SerializeField] private bool _installOnChildGameObjects;

        public IComponent[] InstallOutside()
        {
            List<IComponent> components = new();

            if (_installOnThisGameObject)
                components.AddRange(InstallOnThisGameObject());

            if (_installOnChildGameObjects)
                components.AddRange(InstallOnChildGameObjects());

            return components.ToArray();
        }

        private List<IComponent> InstallOnChildGameObjects()
        {
            List<IComponent> components = new();

            for (int i = 0; i < transform.childCount; i++)
                if (transform.GetChild(i).TryGetComponent(out IOutsideComponentInstaller componentInstaller))
                    components.AddRange(componentInstaller.InstallOutside());

            return components;
        }

        private List<IComponent> InstallOnThisGameObject()
        {
            return GetComponents<IComponent>().ToList(); 
        }
    }
}
