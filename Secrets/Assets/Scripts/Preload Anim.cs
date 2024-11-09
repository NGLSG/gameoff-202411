using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PreloadAnim : MonoBehaviour
{
    [SerializeField] GameObject[] Controls;

    [SerializeField] Ease ShrinkEase = Ease.InBack;
    [SerializeField] Ease ShowEase = Ease.OutBack;
    [SerializeField] Vector3 ShrinkTargetScale = new Vector3(0.5f, 0.5f, 0.5f);
    [SerializeField] Vector3 EnlargeTargetScale = new Vector3(1.5f, 1.5f, 1.5f);
    Coroutine handle;
    Coroutine handle2;
    [SerializeField] float timer = 0;
    public bool stopPlaying;

    private void OnEnable()
    {
        timer = 0;
        stopPlaying = false;
    }

    public void Play(float time)
    {
        if (handle != null)
        {
            StopCoroutine(handle);
        }

        handle = StartCoroutine(PlayBackend(time));
    }

    IEnumerator PlayBackend(float time)
    {
        while (timer < time)
        {
            if (handle2 == null)
                handle2 = StartCoroutine(PlaySequence());
            yield return new WaitForSeconds(0.05f);
            timer += 0.05f;
        }

        stopPlaying = true;
        gameObject.SetActive(false);
    }

    IEnumerator PlaySequence()
    {
        while (!stopPlaying)
        {
            // 创建一个新的 Sequence 动画链
            Sequence sequence = DOTween.Sequence();

            for (int i = 0; i < Controls.Length; i++)
            {
                var control = Controls[i].transform;

                sequence.Append(control.DOScale(ShrinkTargetScale, 0.1f)
                    .SetEase(ShrinkEase).SetUpdate(true));


                if (i < Controls.Length - 1) // 避免在最后一个控件后添加延迟
                {
                    sequence.AppendInterval(0.25f / 2);
                }


                sequence.Append(control.DOScale(EnlargeTargetScale, 0.1f)
                    .SetEase(ShowEase).SetUpdate(true));
            }

            // 等待当前 sequence 动画播放完毕
            yield return sequence.WaitForCompletion();
        }
    }



    IEnumerator ScaleRecursively(int BeginIdx)
    {
        if (stopPlaying)
        {
            yield break;
        }

        try
        {
            var control = Controls[BeginIdx].transform;
            control.DOScale(ShrinkTargetScale, 0.05f).SetEase(ShrinkEase).SetUpdate(true).OnComplete(() =>
            {
                StartCoroutine(ScaleTransform(control, EnlargeTargetScale, 0.05f, ShowEase,
                    () => { StartCoroutine(ScaleTransform(control, Vector3.one, 0.05f, ShowEase)); }));
                StartCoroutine(ScaleRecursively(++BeginIdx));
            });
        }
        catch
        {
            yield break;
        }

        yield return null;
    }

    IEnumerator ScaleTransform(Transform control, Vector3 targetScale, float duration, Ease ease,
        TweenCallback action = null)
    {
        control.DOScale(targetScale, duration).SetEase(ease).SetUpdate(true).OnComplete(() => { action?.Invoke(); });
        yield return null;
    }
}