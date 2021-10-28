using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void loadMainScene() {
        SceneManager.LoadScene(1);
    }
    public void loadStartScene()
    {
        SceneManager.LoadScene(0);
    }
    public void loadMoneyScene()
    {
        SceneManager.LoadScene(2);
    }

    public void quitGame() {
        Application.Quit();
    }

}
