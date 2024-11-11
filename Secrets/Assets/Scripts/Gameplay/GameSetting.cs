using TMPro;
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
        Setting.Language = LanguageDropdown.options[index].text;
        if (Setting.Language == "English")
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[1];
        }
        else if (Setting.Language == "Chinese")
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
    public string Language = "English";
}