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
            yield return new WaitForSecondsRealtime(0.05f);
            timer += 0.05f;
        }

        stopPlaying = true;
        gameObject.SetActive(false);
    }

    IEnumerator PlaySequence()
    {
        while (!stopPlaying)
        {
            for (int i = 0; i < Controls.Length; i++)
            {
                var control = Controls[i].transform;

                // 创建单独的 Sequence 动画链，且每个控件独立
                Sequence sequence = DOTween.Sequence();

                sequence.Append(control.DOScale(ShrinkTargetScale, 0.1f)
                    .SetEase(ShrinkEase)
                    .SetUpdate(true));

                sequence.AppendInterval(0.125f); // 延迟一部分时间

                sequence.Append(control.DOScale(EnlargeTargetScale, 0.1f)
                    .SetEase(ShowEase)
                    .SetUpdate(true));

                // 开始播放序列，避免重叠干扰
                sequence.Play().SetUpdate(true);
                yield return sequence.WaitForCompletion();
            }
        }
    }
}