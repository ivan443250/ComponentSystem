using UnityEngine;
using System;

namespace ComponentSystem
{
    public class MonoComponentInstaller : BaseMonoInstaller
    {
        public override IComponent[] InstallOutside()
        {
            IComponent[] components = GetComponents<IComponent>();

            if (components.Length > 1)
                throw new Exception("MonoComponentInstaller do not support installing more than one IComponent on GameObject");

            return components;
        }

        public override IComponent[] InstallInside(IComponent sender)
        {
            return GetChildComponets().ToArray();
        }
    }
}