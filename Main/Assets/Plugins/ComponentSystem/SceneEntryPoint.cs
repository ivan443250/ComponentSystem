using UnityEngine;

namespace ComponentSystem
{
    [RequireComponent(typeof(ComponentContainer))]
    public class SceneEntryPoint : MonoBehaviour
    {
        [SerializeField] private ServiceManager _serviceManager;

        [SerializeField] private BaseSceneSignalRegistrator _sceneSignalRegistrator;

        private void Awake()
        {
            SignalsBranch main = new SignalsBranch();
            _sceneSignalRegistrator?.RegisterSignals(main);

            _serviceManager?.Initialize(new ComponentGetter(new ComponentDictionary()), main);

            IComponent[] components = GetComponent<ComponentContainer>().InstallOutside();

            ComponentSet componentSet = new(components);
            componentSet.InitializeComponents(main);
        }
    }
}
