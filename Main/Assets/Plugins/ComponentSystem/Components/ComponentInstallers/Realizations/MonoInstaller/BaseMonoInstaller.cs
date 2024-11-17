using System.Collections.Generic;
using UnityEngine;

namespace ComponentSystem
{
    public abstract class BaseMonoInstaller : MonoBehaviour, IOutsideComponentInstaller, IInsideComponentInstaller
    {
        public abstract IComponent[] InstallOutside();
        public abstract IComponent[] InstallInside(IComponent sender);

        protected List<IComponent> GetChildComponets()
        {
            List<IComponent> allComponents = new();

            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).TryGetComponent(out IOutsideComponentInstaller outsideComponentInstaller) == false)
                    continue;

                allComponents.AddRange(outsideComponentInstaller.InstallOutside());
            }

            return allComponents;
        }
    }
}
