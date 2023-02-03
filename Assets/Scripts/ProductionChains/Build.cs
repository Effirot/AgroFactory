using UnityEngine;

[CreateAssetMenu(fileName = "Новая постройка", menuName = "Постройка", order = 1)]
public class Build : ScriptableObject
{
    [SerializeField] private Vector2Int _size;
    [SerializeField] private GameObject _model;
}
