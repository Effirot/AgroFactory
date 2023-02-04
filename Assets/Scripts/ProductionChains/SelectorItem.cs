using System;
using UnityEngine;

public class SelectorItem : MonoBehaviour
{
    public event Action OnHover;
    public event Action OnExit;

    public void Hover() => OnHover?.Invoke();
    public void Exit() => OnExit?.Invoke();
}
