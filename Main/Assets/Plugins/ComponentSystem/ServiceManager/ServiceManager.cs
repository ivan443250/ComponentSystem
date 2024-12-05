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

        public static T GetService<T>()
        {
            if (_instance.ComponentGetter.TryGetComponent(out object service, typeof(T), AccessModifier.Everything) == false)
                throw new System.Exception($"Service {typeof(T)} was not found");

            return (T)service;
        }
    }
}
