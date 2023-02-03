using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMenu : MonoBehaviour
{
    [SerializeField] private GameObject m_PauseMenu;
    [SerializeField] private bool isPause = false;

    private void Start()
    {
        Time.timeScale = 1;
    }
    public void ToggleMenu()
    {
        isPause = !isPause;
        m_PauseMenu.SetActive(isPause);
        Time.timeScale = isPause ? 0 : 1;
    }
    public void ReloadActiveScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
