using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject m_PauseMenu;
    [SerializeField] private Image m_Image;
    [SerializeField] private bool isPause = false;

    private void Start()
    {
        Time.timeScale = 1;
    }
    public void ToggleMenu()
    {
        isPause = !isPause;
        if(!isPause) Time.timeScale = 1;
        StartCoroutine(SetAlphaMax(isPause));

    }
    IEnumerator SetAlphaMax(bool toMax)
    {
        m_PauseMenu.SetActive(isPause);
        float curTime = 0;
        float time = 0.5f;
        float shag = 0.05f;
        float delta = 0.75f / (time / shag);
        Color color = m_Image.color;
        while(curTime < time)
        {
            curTime += shag;
            if(toMax)
                color.a += delta;
            else
                color.a -= delta;
            m_Image.color = color;
            yield return new WaitForSeconds(shag);
        }
        if (isPause) Time.timeScale = 0;
    }

}
