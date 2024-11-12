using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    public static String SceneName;

    void Start()
    {
        StartCoroutine(LoadSceneWithProgress());
    }

    IEnumerator LoadSceneWithProgress()
    {
        yield return new WaitForSeconds(1.0f);
        // 异步加载指定的场景
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneName);

        // 循环直到加载完成
        while (!asyncLoad.isDone)
        {
            // 可以在这里更新加载进度
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f); // 防止进度条超过1

            // 等待下一帧
            yield return null;
        }

        // 检查加载是否成功完成
        if (asyncLoad.allowSceneActivation && !asyncLoad.isDone)
        {
            // 激活场景
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(SceneName));
        }

        // 可选：卸载当前场景
        // SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
    }
}