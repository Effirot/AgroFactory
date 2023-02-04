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
    private InputAction _rightButtonClick;

    private Map _map;
    private PlayerMapInteraction _playerInteraction;
    private Building _building;

    private Vector2Int PointOnMap => ToMap(_playerInteraction.Cursor);

    private void Start() {
        _input = GetComponent<PlayerInput>();

        _map = new Map(
            CreateCell,
            _mapSize,
            _radiusOfShapeDeformation,
            _maxHeight
        );
        _playerInteraction = new PlayerMapInteraction(_linearDimension);
        _building = new Building();

        _edge = -_mapSize / 2;

        _map.GenerateMap();

        _leftButtonClick = _input.actions.FindAction("LeftButton");
        _rightButtonClick = _input.actions.FindAction("RightButton");
    }

    private void OnDestroy() {
    }

    private void Update() {
        _playerInteraction.Update();
        DeformMap(_playerInteraction.Cursor);

        if (_leftButtonClick.WasPressedThisFrame()) {
            _building.MoveNextBuildStage();
        }

        if (_rightButtonClick.WasPressedThisFrame()) {
            _building.CancelBuild();
        }

        var size = new Vector2Int(2, 3);

        if (_building.IsFindPlace) {
            _map.HighLightCell(PointOnMap, size);
        }

        if (_building.CanBuild) {
            if (_map.IsSectorFree(PointOnMap, size)) {
                Build(size);
                _map.OccupySector(PointOnMap, size);
                _building.ResetStages();
            } else {
                _building.MovePreviousBuildStage();
            }
        }

        _map.Update();
    }

    private void DeformMap(Vector2 cursor) {
        _map.ChangeShape(ToMap(cursor));
    }

    private void Build(Vector2Int size) {
        var position = _playerInteraction.PointOnPlane;
        var offset = new Vector3(size.x - 1, 0.0f, size.y - 1);
        var scale = new Vector3(_linearDimension, _linearDimension, _linearDimension);
        position += Vector3.Scale(offset / 2.0f, scale);

        Instantiate(_buildPattern, position, Quaternion.identity, _buildsParrent);
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
