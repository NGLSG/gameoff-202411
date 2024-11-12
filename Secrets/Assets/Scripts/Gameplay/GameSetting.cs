﻿using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class GameSetting : Singleton<GameSetting>
{
    public static Setting Setting = new Setting();
    [SerializeField] private Checkbox BGMCheckbox;
    [SerializeField] private Checkbox SoundCheckbox;
    [SerializeField] private TMP_Dropdown LanguageDropdown;

    void Awake()
    {
        BGMCheckbox.isOn = Setting.BGMOn;
        SoundCheckbox.isOn = Setting.GameSoundOn;
        BGMCheckbox.OnChange.Add(OnBGMToggle);
        SoundCheckbox.OnChange.Add(OnSoundToggle);
        LanguageDropdown.onValueChanged.AddListener(OnLanguageChange);
    }

    void OnLanguageChange(int index)
    {
        if (LanguageDropdown.options[index].text == "English")
        {
            Setting.Language = "en";
        }
        else if (LanguageDropdown.options[index].text == "简体中文")
        {
            Setting.Language = "zh-cn";
        }

        if (Setting.Language == "en")
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[1];
        }
        else if (Setting.Language == "zh-cn")
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[0];
        }
    }

    void OnBGMToggle(bool isOn)
    {
        Setting.BGMOn = isOn;
    }

    void OnSoundToggle(bool isOn)
    {
        Setting.GameSoundOn = isOn;
    }
}

public class Setting
{
    public bool GameSoundOn = true;
    public bool BGMOn = true;
    public string Language = "en"; //en,zh-cn
}