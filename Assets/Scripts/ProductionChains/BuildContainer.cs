using UnityEngine;

[CreateAssetMenu(fileName = "Новое хранилище построек", menuName = "Хранилище построек", order = 1)]
public class BuildContainer : ScriptableObject
{
    [SerializeField] private Build[] _builds;

    public Build[] Builds => _builds;
}
