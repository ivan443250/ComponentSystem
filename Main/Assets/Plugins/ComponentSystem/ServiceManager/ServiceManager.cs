namespace ComponentSystem
{
    public class ServiceManager : MonoBehaviourComponent
    {
        private static ServiceManager _instance;

        protected override void Initialize()
        {
            if (_instance != null)
                throw new System.Exception("There are more than one ServiceManagers on scene");

            _instance = this;
        }

        public static T GetService<T>() where T : MonoBehaviourComponent
        {
            if (_instance.ComponentGetter.TryGetComponent(out T service) == false)
                throw new System.Exception($"Service {typeof(T)} was not found");

            return service;
        }
    }
}
