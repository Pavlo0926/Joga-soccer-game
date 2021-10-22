using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using System;
using System.Collections.Generic;
using Object = UnityEngine.Object;

namespace FastSkillTeam
{

    public class FindMissingScriptsEditor : EditorWindow
    {
        [MenuItem("FST Tools/Utilities/Find Missing Scripts")]
        public static void FindMissingScripts()
        {
            EditorWindow.GetWindow(typeof(FindMissingScriptsEditor));
        }

        [MenuItem("FST Tools/Utilities/Clear Progressbar")]
        public static void ClearProgressbar()
        {
            EditorUtility.ClearProgressBar();
        }

        static int missingCount = -1;
        void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("Missing Scripts:");
                EditorGUILayout.LabelField("" + (missingCount == -1 ? "---" : missingCount.ToString()));
            }
            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("Find missing scripts"))
            {
                missingCount = 0;
                EditorUtility.DisplayProgressBar("Searching Prefabs", "", 0.0f);

                string[] files = System.IO.Directory.GetFiles(Application.dataPath, "*.prefab", System.IO.SearchOption.AllDirectories);
                EditorUtility.DisplayCancelableProgressBar("Searching Prefabs", "Found " + files.Length + " prefabs", 0.0f);

                Scene currentScene = EditorSceneManager.GetActiveScene();
                string scenePath = currentScene.path;
                EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);

                for (int i = 0; i < files.Length; i++)
                {
                    string prefabPath = files[i].Replace(Application.dataPath, "Assets");
                    if (EditorUtility.DisplayCancelableProgressBar("Processing Prefabs " + i + "/" + files.Length, prefabPath, (float)i / (float)files.Length))
                        break;

                    GameObject go = UnityEditor.AssetDatabase.LoadAssetAtPath(prefabPath, typeof(GameObject)) as GameObject;

                    if (go != null)
                    {
                        FindInGO(go);
                        go = null;
                        EditorUtility.UnloadUnusedAssetsImmediate(true);
                    }
                }


                EditorUtility.DisplayProgressBar("Cleanup", "Cleaning up", 1.0f);
                EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);

                EditorUtility.UnloadUnusedAssetsImmediate(true);
                GC.Collect();

                EditorUtility.ClearProgressBar();
            }


            if (GUILayout.Button("Obliterate missing script components"))
            {
                missingCount = 0;
                EditorUtility.DisplayProgressBar("Searching Prefabs", "", 0.0f);

                string[] files = System.IO.Directory.GetFiles(Application.dataPath, "*.prefab", System.IO.SearchOption.AllDirectories);
                EditorUtility.DisplayCancelableProgressBar("Searching Prefabs", "Found " + files.Length + " prefabs", 0.0f);

                Scene currentScene = EditorSceneManager.GetActiveScene();
                string scenePath = currentScene.path;
                EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);

                for (int i = 0; i < files.Length; i++)
                {
                    string prefabPath = files[i].Replace(Application.dataPath, "Assets");
                    if (EditorUtility.DisplayCancelableProgressBar("Processing Prefabs " + i + "/" + files.Length, prefabPath, (float)i / (float)files.Length))
                        break;

                    GameObject go = UnityEditor.AssetDatabase.LoadAssetAtPath(prefabPath, typeof(GameObject)) as GameObject;

                    if (go != null)
                    {
                         GameObject instance = PrefabUtility.InstantiatePrefab(go.transform.root) as GameObject; //PrefabUtility.GetOutermostPrefabInstanceRoot(go);
                                                                                                                 //   GameObject instance =   PrefabUtility.InstantiatePrefab(root) as GameObject;

                        if (instance)
                        {
                            Component[] comps = instance.GetComponents<Component>();
                            for (int v = 0; v < comps.Length; v++)
                            {
                                if (comps[i] == null)
                                {
                                    PrefabUtility.ApplyRemovedComponent(instance, comps[i], InteractionMode.AutomatedAction);
                                    // PrefabUtility.ApplyPrefabInstance(instance.transform.root.gameObject, InteractionMode.AutomatedAction);
                                }
                            }


                            DestroyImmediate(instance);
                        }

                        FindInGO(go);
                        EditorUtility.UnloadUnusedAssetsImmediate(true);
                    }
                }


                EditorUtility.DisplayProgressBar("Cleanup", "Cleaning up", 1.0f);
                EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);

                EditorUtility.UnloadUnusedAssetsImmediate(true);
                GC.Collect();

                EditorUtility.ClearProgressBar();
            }
        }

        private static void FindInGO(GameObject go, string prefabName = "")
        {
            Component[] components = go.GetComponents<Component>();
            for (int i = 0; i < components.Length; i++)
            {
                if (components[i] == null)
                {
                    missingCount++;
                    Transform t = go.transform;

                    string componentPath = go.name;
                    while (t.parent != null)
                    {
                        componentPath = t.parent.name + "/" + componentPath;
                        t = t.parent;
                    }
                    Debug.LogWarning("Prefab " + prefabName + " has an empty script attached:\n" + componentPath, go);
                }
            }

            foreach (Transform child in go.transform)
            {
                FindInGO(child.gameObject, prefabName);
            }
        }
    }

    public class FindMissingSceneScripts : Editor
    {
        [MenuItem("FST Tools/Utilities/Select Scene GameObjects With Missing Scripts")]
        static void SelectGameObjects()
        {
            //Get the current scene and all top-level GameObjects in the scene hierarchy
            Scene currentScene = SceneManager.GetActiveScene();
            GameObject[] rootObjects = currentScene.GetRootGameObjects();

            List<Object> objectsWithDeadLinks = new List<Object>();


            for (int i = 0; i < rootObjects.Length; i++)
            {

                List<Transform> ts = GetChildRecursive(rootObjects[i].transform);

                for (int v = 0; v < ts.Count; v++)
                {
                 


                    Transform child = ts[v];

                    Debug.Log("Searching: " + child);

                    //Get all components on the GameObject, then loop through them 
                    Component[] components = child.GetComponents<Component>();
                    for (int u = 0; u < components.Length; u++)
                    {
                        Component currentComponent = components[u];

                        //If the component is null, that means it's a missing script!
                        if (currentComponent == null)
                        {
                            //Add the sinner to our naughty-list
                            objectsWithDeadLinks.Add(child);
                            Selection.activeGameObject = child.gameObject;
                            Debug.Log(child.gameObject + " has a missing script!");
                            break;
                        }
                    }
                }
            }

           
            if (objectsWithDeadLinks.Count > 0)
            {
                //Set the selection in the editor
                Selection.objects = objectsWithDeadLinks.ToArray();
            }
            else
            {
                Debug.Log("No GameObjects in '" + currentScene.name + "' have missing scripts! Yay!");
            }
        }
        public static List<Transform> GetChildRecursive(Transform t)
        {
            if (t != null)
            {
                List<Transform> children = new List<Transform>();

                for (int i = 0; i < t.childCount; i++)
                {
                    Transform f = t.GetChild(i);
                    if (f != null)
                    {
                        children.Add(f);
                        GetChildRecursive(f);
                    }
                }


                return children;
            }
            else return null;
        }
    }
}