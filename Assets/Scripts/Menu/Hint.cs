using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Hint : MonoBehaviour
{
    [SerializeField] private Image m_bgImage;
    [SerializeField] private float m_Width;
    [SerializeField] private GameObject m_Hint;
    public TMP_Text m_tmpText;
    public string m_text;
    [Space]
    [Header("Animation parameters")]
    [SerializeField] private float time = 0.5f;
    [SerializeField] private float shag = 0.01f;
    public bool isActive { get => _isActive; set { _isActive = value; SetHint(); } }
    private bool _isActive = false;
    public void ToggleHint()
    {
        isActive = !isActive;
    }
    public void SetHintParameters(string str = "")
    {
        m_text = str;
        isActive = !string.IsNullOrEmpty(m_text);
    }
    private void SetHint()
    {
        m_tmpText.text = "";
        if (isActive)
        {
            StartCoroutine(TextAnim(true));
            m_Hint.SetActive(false);
        }
        else
        {
            StartCoroutine(TextAnim(false));
        }
    }
    IEnumerator TextAnim(bool toMax)
    {
        float curtime = 0;
        float delta = m_Width / (time / shag);
        Vector2 vector2 = new(m_bgImage.rectTransform.sizeDelta.x, m_bgImage.rectTransform.sizeDelta.y);
        float curWidth = vector2.x;
        if (!toMax)
        {
            m_Hint.SetActive(toMax);
            yield return new WaitForSeconds(0.01f);
            m_tmpText.gameObject.SetActive(toMax);
        }


        m_bgImage.gameObject.SetActive(true);

        while (curtime < time)
        {
            curtime += shag;
            if (toMax)
                curWidth += delta;
            else
                curWidth -= delta;
            if ((toMax && curWidth > m_Width) || (!toMax && curWidth < 0)) break;
            m_bgImage.rectTransform.sizeDelta = new Vector2(curWidth, vector2.y);
            yield return new WaitForSeconds(shag);
        }
        m_tmpText.text = toMax ? m_text : "";
        if (toMax)
        {
            m_Hint.SetActive(toMax);
            yield return new WaitForSeconds(0.01f);
            m_tmpText.gameObject.SetActive(toMax);

        }
        yield return new WaitForSeconds(0.1f);
        m_bgImage.gameObject.SetActive(false);
    }
}
