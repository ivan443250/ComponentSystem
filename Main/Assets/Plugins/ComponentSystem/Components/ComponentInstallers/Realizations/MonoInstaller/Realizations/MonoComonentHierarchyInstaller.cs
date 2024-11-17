using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ComponentSystem
{
    public class MonoComonentHierarchyInstaller : BaseMonoInstaller
    {
        [SerializeField] private MonoBehaviourComponent _main;

        public override IComponent[] InstallOutside()
        {
            return new IComponent[] { _main };
        }

        public override IComponent[] InstallInside(IComponent sender)
        {
            if (sender != _main as IComponent)
                return new IComponent[0];

            List<IComponent> componetsOnMainGameObject = GetComponents<IComponent>().ToList();
            componetsOnMainGameObject.Remove(_main);

            List<IComponent> allComponents = GetChildComponets();
            allComponents.AddRange(componetsOnMainGameObject);

            return allComponents.ToArray();
        }
    }
}
