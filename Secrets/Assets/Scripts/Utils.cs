using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Utils
{
    private static StringBuilder m_StringBuilder = new StringBuilder();

    public static string StringCombine(params string[] strings)
    {
        m_StringBuilder.Clear();
        foreach (var str in strings)
        {
            m_StringBuilder.Append(str);
        }

        return m_StringBuilder.ToString();
    }

    public static void LoadScene(string sceneName)
    {
        LoadingManager.SceneName = sceneName;
        SceneManager.LoadScene("LoadingScreen");
    }

    public static void RemoveAllChildren(Transform parent)
    {
        for (int i = parent.childCount - 1; i >= 0; i--)
        {
#if UNITY_EDITOR
            Debug.Log("Destroying " + parent.GetChild(i).gameObject.name);
#endif
            Object.Destroy(parent.GetChild(i).gameObject);
        }
    }

    public static void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
Application.Quit();
#endif
    }
}