using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame(string sceneName)
    {
        Utils.LoadScene(sceneName);
    }

    public void ExitGame()
    {
        Utils.Exit();
    }
}