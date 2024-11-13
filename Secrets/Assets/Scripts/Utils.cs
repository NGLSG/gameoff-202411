using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;

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

    public static T[] ConvertTextAssetArray<T>(TextAsset[] textAssets)
    {
        try
        {
            List<T> list = new List<T>();
            foreach (var textAsset in textAssets)
            {
                list.Add(JsonConvert.DeserializeObject<T>(textAsset.text));
            }

            return list.ToArray();
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
            return default;
        }
    }

    public static void SaveAsJson<T>(T data, string path)
    {
        try
        {
            string directory = Path.GetDirectoryName(path);
            if (!Directory.Exists(directory) && !string.IsNullOrEmpty(directory))
            {
                Directory.CreateDirectory(directory);
            }
#if UNITY_EDITOR
            Debug.Log("Saving to " + path);
#endif

            var json = JsonConvert.SerializeObject(data);
            System.IO.File.WriteAllText(path, json);
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
        }
    }

    public static T LoadFromJson<T>(string path)
    {
        try
        {
#if UNITY_EDITOR
            Debug.Log("Loading from " + path);
#endif
            var json = System.IO.File.ReadAllText(path);
            return JsonConvert.DeserializeObject<T>(json);
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
            return default;
        }
    }

    public static void RemoveAllChildren(Transform parent)
    {
        for (int i = parent.childCount - 1; i >= 0; i--)
        {
#if UNITY_EDITOR
            Debug.Log("Destroying OBJ: " + parent.GetChild(i).gameObject.name);
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