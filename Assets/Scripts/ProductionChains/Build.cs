using UnityEngine;

[CreateAssetMenu(fileName = "Новая постройка", menuName = "Постройка", order = 1)]
public class Build : ScriptableObject
{
    [SerializeField, TextArea] private string _name;
    [SerializeField] private Vector2Int _size;
    [SerializeField] private GameObject _model;
    [SerializeField] private Fabric _factory;
    [SerializeField] private Sprite _icon;

    public string Description => _name;
    public Vector2Int Size => _size;
    public GameObject Model => _model;
    public Fabric Fabric => _factory;
    public Sprite Icon => _icon;
}
