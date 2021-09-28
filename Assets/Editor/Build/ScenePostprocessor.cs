#if UNITY_EDITOR
using System.Reflection;
using System.Linq;
using UnityEngine;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using TVB.Core.Attributes;

namespace TVB.Editor
{
    public class ScenePostprocessor : IProcessSceneWithReport
    {
        int IOrderedCallback.callbackOrder { get {  return 1; } }

        void IProcessSceneWithReport.OnProcessScene(UnityEngine.SceneManagement.Scene scene, BuildReport report)
        {
            var rootGameObjects = scene.GetRootGameObjects();

            for (int i = 0; i < rootGameObjects.Length; i++)
            {
                var gameObject = rootGameObjects[i];

                var sceneComponents = gameObject.GetComponents<Component>();
                ProcessComponents(sceneComponents);

                var childComponents = gameObject.GetComponentsInChildren<Component>(true);
                ProcessComponents(childComponents);
            }
        }

        // PRIVATE METHODS

        private void ProcessComponents(Component[] components)
        {
            for (int i = 0; i < components.Length; i++)
            {
                var component = components[i];

                if (component == null)
                    continue;

                var fields = component.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)
                    .Concat(component.GetType().BaseType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public));
                
                foreach (var field in fields)
                {
                    if (System.Attribute.IsDefined(field, typeof(GetComponentInChildrenAttribute)) == true)
                    {
                        if (System.Attribute.IsDefined(field, typeof(SerializeField)) == false)
                        {
                            Debug.LogError($"Attribute SerializeField is not defined on {field.Name}");
                        }

                        var attribute = field.GetCustomAttribute<GetComponentInChildrenAttribute>(false);

                        var obj = default(Component);
                        if (attribute.Path == "")
                        {
                            obj = component.GetComponentInChildren(field.FieldType, true);
                        }
                        else
                        {
                            obj = FindObject(component.gameObject, attribute.Path)?.GetComponent(field.FieldType);

                            if (obj == null)
                            {
                                obj = GameObject.Find(attribute.Path)?.GetComponent(field.FieldType);
                            }

                            if (obj == null && attribute.Verbose == true)
                            {
                                Debug.LogErrorFormat("Can't find reference on object {0} of type {1} on path {2}", field.Name, field.FieldType, attribute.Path);
                                continue;
                            }
                        }

                        field.SetValue(component, obj);
                    }


                    if (System.Attribute.IsDefined(field, typeof(GetComponentAttribute)) == true)
                    {
                        var attribute = field.GetCustomAttribute<GetComponentAttribute>(false);

                        if (System.Attribute.IsDefined(field, typeof(SerializeField)) == false )
                        {
                            Debug.LogError($"Attribute SerializeField is not defined on {field.Name}");
                        }

                        var obj = component.GetComponent(field.FieldType);

                        if (obj == null && attribute.Verbose == true)
                        {
                            Debug.LogErrorFormat("Can't find reference on object {0} of type {1} on component {2}", field.Name, field.FieldType, component.name);
                            continue;
                        }

                        field.SetValue(component, obj);
                    }
                }
            }
        }

        private GameObject FindObject(GameObject parent, string name)
        {
            Transform[] transforms = parent.GetComponentsInChildren<Transform>(true);

            for (int i = 0; i < transforms.Length; i++)
            {
                var transform = transforms[i];

                if (transform.name == name)
                {
                    return transform.gameObject;
                }
            }

            return null;
        }
    }
}
#endif