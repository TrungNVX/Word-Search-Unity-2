using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUtility : MonoBehaviour
{
    /// <summary>
    /// This method use to switch between the different scenes
    /// </summary>
    /// <param name="sceneName"> Name of the scene are called to switch </param>
    public void LoadScenee(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void ExitApplication()
    {
        Application.Quit();
    }
    public void MuteToggleBgMusic()
    {
        SoundManager.Instance.ToggleBgMusic();
    }
    public void MuteToggleSoundFx()
    {
        SoundManager.Instance.ToggleSoundFxMusic();
    }
}
