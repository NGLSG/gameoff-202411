public class GameSetting : Singleton<GameSetting>
{
    public static Setting Setting = new Setting();
}

public class Setting
{
    public bool GameSoundOn = true;
    public bool BGMOn = true;
}