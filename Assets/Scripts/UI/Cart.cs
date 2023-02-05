using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Cart : MonoBehaviour
{
    [SerializeField] private Image img;
    [SerializeField] private TMP_Text title;
    [SerializeField] private GameObject Player;
    [SerializeField] private float angle = 180;
    [SerializeField] private float speed = 100;
    [SerializeField] private Outline _outline;
    [SerializeField] private Color _color;

    [SerializeField] private bool _toggle = true;
    public void Toggle()
    {
        if (_toggle)
        {
            StartCoroutine(c_RotateAroundPerfect(transform , transform.position, Vector3.up, angle, speed));
            _toggle =false;
        }

    }
    public void SetColorGreen()
    {
        _outline.effectColor = _color;
    }
    public void SetColorWhite()
    {
        _outline.effectColor = Color.white;
    }

    IEnumerator c_RotateAroundPerfect(Transform me, Vector3 point, Vector3 axis, float angle, float speed)
    {
        if (angle < 0.0f)
        {
            axis = -axis;
            angle = -angle;
        }

        float t = 0.0f;

        while (true)
        {
            float value = speed * Time.deltaTime;
  
            me.RotateAround(point, axis, value);
            if ((t += value) > angle)
            {
                me.RotateAround(point, axis, angle - t);


                break;
            }

            yield return null;
        }
        title.gameObject.SetActive(false);
        Player.SetActive(true);
    }
}
