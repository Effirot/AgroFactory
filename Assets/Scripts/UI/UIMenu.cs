using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMenu : MonoBehaviour
{
    private void Start()
    {
        Time.timeScale = 1;
    }
    public void ReloadActiveScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void StartSceneByNum(int scene)
    {
        SceneManager.LoadScene(scene);
    }
}
