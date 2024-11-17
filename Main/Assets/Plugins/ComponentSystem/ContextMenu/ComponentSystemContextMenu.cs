using UnityEditor;
using UnityEngine;

namespace ComponentSystem
{
    public class ComponentSystemContextMenu
    {
        [MenuItem("GameObject/ComponentSystem/MonobehaviourComponent")]
        private static void CreateMonobehaviourComponent()
        {
            GameObject instance = new GameObject("UntitledComponent");

            instance.AddComponent<MonoComponentInstaller>();
            instance.AddComponent<MonoBehaviourComponent>();
        }

        [MenuItem("GameObject/ComponentSystem/ScriptableComponentsInstaller")]
        private static void CreateScriptableComponentInstaller()
        {
            GameObject instance = new GameObject("ScriptableComponentsInstaller");

            instance.AddComponent<ScriptableComponetsInstaller>();
        }

        [MenuItem("GameObject/ComponentSystem/SceneEntryPoint")]
        private static void CreateSceneEntryPoint()
        {
            GameObject instance = new GameObject("SceneEntryPoint");

            instance.AddComponent<SceneEntryPoint>();
        }

        [MenuItem("GameObject/ComponentSystem/ComponentContainer")]
        private static void CreateComponentContainer()
        {
            GameObject instance = new GameObject("ComponentContainer");

            instance.AddComponent<ComponentContainer>();
        }
    }
}
