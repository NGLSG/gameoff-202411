using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISetting : Singleton<UISetting>
{
    [SerializeField] private Checkbox BGMCheckbox;
    [SerializeField] private Checkbox SoundCheckbox;

    [SerializeField] private AudioSource[] BGMs;
    [SerializeField] private AudioSource[] Sounds;

    [SerializeField] private GameObject BGMParent;
    [SerializeField] private GameObject SoundParent;

    void Awake()
    {
        BGMs = BGMParent.GetComponentsInChildren<AudioSource>();
        Sounds = SoundParent.GetComponentsInChildren<AudioSource>();
        BGMCheckbox.isOn = GameSetting.Setting.BGMOn;
        SoundCheckbox.isOn = GameSetting.Setting.GameSoundOn;
        BGMCheckbox.OnChange.Add(OnBGMToggle);
        SoundCheckbox.OnChange.Add(OnSoundToggle);
        foreach (var bgm in BGMs)
        {
            bgm.volume = BGMCheckbox.isOn ? 1 : 0;
        }

        foreach (var sound in Sounds)
        {
            sound.volume = SoundCheckbox.isOn ? 1 : 0;
        }
    }

    void OnBGMToggle(bool isOn)
    {
        foreach (var bgm in BGMs)
        {
            bgm.volume = isOn ? 1 : 0;
        }
    }

    void OnSoundToggle(bool isOn)
    {
        foreach (var sd in Sounds)
        {
            sd.volume = isOn ? 1 : 0;
        }
    }

    public void ContinueGame()
    {
        GameUIManager.Instance.TogglePause(false);
    }

    public void ExitGame()
    {
        GameManager.Instance.stateMechine.SetState("GameInitState");
        Utils.Exit();
    }
}