using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuHandler : MonoBehaviour
{
    public int sceneNumber;

    public void Play()
    {
        SceneManager.LoadScene(sceneNumber);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
