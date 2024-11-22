using UnityEngine;

namespace ComponentSystem
{
    [RequireComponent(typeof(ComponentContainer))]
    public class SceneEntryPoint : MonoBehaviour
    {
        [SerializeField] private ServiceManager _serviceManager;

        private void Awake()
        {
            _serviceManager.Initialize(new ComponentGetter(new ComponentDictionary()), new SignalsBranch());

            IComponent[] components = GetComponent<ComponentContainer>().InstallOutside();

            ComponentSet componentSet = new(components);
            componentSet.InitializeComponents(new SignalsBranch());
        }
    }
}
