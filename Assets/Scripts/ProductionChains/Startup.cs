using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class Startup : MonoBehaviour {
    [Header("Карта")]
    [SerializeField, Range(0.1f, 5.0f)] private float _linearDimension;
    [SerializeField, Min(0.0f)] private Vector2Int _mapSize;
    [SerializeField, Range(1f, 10.0f)] private int _radiusOfShapeDeformation;
    [SerializeField, Range(0.1f, 1.0f)] private float _maxHeight;
    [SerializeField] private Cell _cellPattern;
    [SerializeField] private Transform _mapParent;

    [Header("Строительство")]
    [SerializeField] private GameObject _buildPattern;
    [SerializeField] private Transform _buildsParrent;

    private Vector2Int _edge;
    private PlayerInput _input;
    private InputAction _leftButtonClick;

    private Transform _build;

    private Map _map;
    private PlayerMapInteraction _playerInteraction;

    private void Start() {
        _input = GetComponent<PlayerInput>();

        _map = new Map(
            CreateCell,
            _mapSize,
            _radiusOfShapeDeformation,
            _maxHeight
        );
        _playerInteraction = new PlayerMapInteraction(_linearDimension);

        _edge = -_mapSize / 2;

        _map.GenerateMap();

        _leftButtonClick = _input.actions.FindAction("LeftButton");
    }

    private void OnDestroy() {
    }

    private void Update() {
        _playerInteraction.Update();
        DeformMap(_playerInteraction.Cursor);

        if (_leftButtonClick.WasPressedThisFrame()) {
            Build();
        }

        if (_build != null) {
            _build.gameObject.SetActive(_map.IsFree(ToMap(_playerInteraction.Cursor)));
        }

        if (_build != null && _map.IsOccupy(ToMap(_playerInteraction.Cursor)) is false) {
            _build.position = _playerInteraction.PointOnPlane;
        }
    }

    private void DeformMap(Vector2 cursor) {
        _map.ChangeShape(ToMap(cursor));
    }

    private void Build() {
        if (_build != null) {
            if (_map.IsOccupy(ToMap(_playerInteraction.Cursor))) return;

            _map.OccupyCell(ToMap(_playerInteraction.Cursor));
            _build = null;
        }

        if (_build != null) return;

        _build = Instantiate(_buildPattern, _buildsParrent).transform;
    }

    private Cell CreateCell(Vector2 mapCoordinate) {
        mapCoordinate += _edge;

        var localCoordinate = new Vector3(mapCoordinate.x, 0.0f, mapCoordinate.y);
        var size = new Vector3(_linearDimension, _linearDimension, _linearDimension);
        var coordinate = Vector3.Scale(localCoordinate, size);

        return Instantiate(_cellPattern, coordinate, Quaternion.identity, _mapParent);
    }

    private Vector2Int ToMap(Vector2 cursor) {
        var mapX = Mathf.RoundToInt(cursor.x / _linearDimension);
        var mapY = Mathf.RoundToInt(cursor.y / _linearDimension);

        return new Vector2Int(mapX, mapY) - _edge;
    }
}
