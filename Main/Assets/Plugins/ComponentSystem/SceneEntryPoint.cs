using UnityEngine;

namespace ComponentSystem
{
    [RequireComponent(typeof(ComponentContainer))]
    public class SceneEntryPoint : MonoBehaviour
    {
        private void Awake()
        {
            IComponent[] components = GetComponent<ComponentContainer>().InstallOutside();

            ComponentSet componentSet = new(components);
            componentSet.InitializeComponents(new SignalsBranch());
        }
    }
}
