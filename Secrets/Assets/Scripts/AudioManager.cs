using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    public AudioSource[] soundEffects;
    public AudioSource bgm;
    public AudioSource[] enddingBGM;
    private void Start()
    {
        bgm.Play();
    }

    public void PlaySoundEffect(int soundToPlay)
    {
        Debug.Log($"here in audio manager: {soundToPlay}");
        // 停止当前播放的音效
        soundEffects[soundToPlay].Stop();

        // 播放音效
        soundEffects[soundToPlay].Play();

    }

    public void PlaySoundEffectWithoutShutDown(int soundToPlay)
    {
        Debug.Log($"here in audio manager: {soundToPlay}");

        // 检查音效是否正在播放
        if (! soundEffects[soundToPlay].isPlaying)
        {
            soundEffects[soundToPlay].Play();
        }
    }
}