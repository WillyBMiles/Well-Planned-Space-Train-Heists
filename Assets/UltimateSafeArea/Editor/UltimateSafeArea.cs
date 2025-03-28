using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

namespace Frosttale_Studios_Assets
{
    /// <summary>
    /// This component is responsible for applying the SafeArea script to the scene or whole project accordingly.
    /// </summary>

    public class UltimateSafeArea : EditorWindow
    {
        public bool finished;

        [MenuItem("Tools/UltimateSafeArea/Apply SafeArea in Scene")]
        static bool ApplySafeArea_InScene()
        {
            //Look for canvases
            Canvas[] allCanvases = FindObjectsByType<Canvas>(FindObjectsSortMode.None);

            if (allCanvases.Length == 0) return true;
            if (allCanvases[0].GetComponentInChildren<SafeArea>()) return true;

            //Create SafeArea in all canvases in scene
            for (int i = 0; i < allCanvases.Length; i++)
            {
                var safeArea = new GameObject();
                safeArea.transform.parent = allCanvases[i].transform;
                safeArea.name = "SafeArea";
                safeArea.AddComponent<RectTransform>();
                safeArea.GetComponent<RectTransform>().anchorMin = Vector2.zero;
                safeArea.GetComponent<RectTransform>().anchorMax = Vector2.one;
                safeArea.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
                safeArea.transform.eulerAngles = Vector3.zero;
                safeArea.transform.localScale = Vector3.one;
                safeArea.AddComponent<SafeArea>();

                //Put all children inside SafeArea
                allCanvases[i].GetComponentInChildren<CanvasRenderer>().transform.parent = safeArea.transform;
            }

            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            return true;
        }

        [MenuItem("Tools/UltimateSafeArea/Apply SafeArea in Project")]
        static void ApplySafeArea_InProject()
        {
            //Get all existing scenes
            int sceneCount = SceneManager.sceneCountInBuildSettings;
            string[] scenes = new string[sceneCount];

            for (int i = 0; i < sceneCount; i++)
            {
                scenes[i] = SceneUtility.GetScenePathByBuildIndex(i);
                Debug.Log(scenes[i]);
            }

            //Go through one by one and apply safe area
            for (int i = 0; i < sceneCount; i++)
            {
                EditorSceneManager.OpenScene(scenes[i], OpenSceneMode.Single);
                ApplySafeArea_InScene();
                EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
            }
        }
    }
}
