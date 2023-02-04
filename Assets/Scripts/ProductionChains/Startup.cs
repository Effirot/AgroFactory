using System.Collections.Generic;
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
    [SerializeField] private Transform _buildsParrent;
    [SerializeField] private Selector _selector;

    private Vector2Int _edge;
    private PlayerInput _input;
    private InputAction _leftButtonClick;
    private InputAction _rightButtonClick;

    private Build _build;
    private List<MapBuild> _builds = new();

    private Map _map;
    private PlayerMapInteraction _playerInteraction;
    private BuildSystem _buildSystem;

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
        _buildSystem = new BuildSystem();

        _edge = -_mapSize / 2;

        _map.GenerateMap();

        _leftButtonClick = _input.actions.FindAction("LeftButton");
        _rightButtonClick = _input.actions.FindAction("RightButton");
        _selector.OnBuildSelected += SelectBuild;
    }

    private void OnDestroy() {
        _selector.OnBuildSelected -= SelectBuild;
    }

    private void Update() {
        _playerInteraction.Update();
        DeformMap(_playerInteraction.Cursor);

        if (_leftButtonClick.WasPressedThisFrame() && _buildSystem.IsNeedSelect is false) {
            _buildSystem.MoveNextBuildStage();
        }

        if (_rightButtonClick.WasPressedThisFrame()) {
            _buildSystem.CancelBuild();
            _selector.Disable();
        }

        if (_buildSystem.IsNeedSelect) {
            _selector.Enadle();
        }

        if (_build != null) {
            var point = PointOnMap - _build.Size / 2;

            if (_buildSystem.IsFindPlace) {
                _map.PrepareCell(point, _build.Size);
            }

            if (_buildSystem.CanBuild) {
                if (_map.IsSectorFree(point, _build.Size)) {
                    var build = Build();
                    _map.OccupySector(point, _build.Size, build);
                    _buildSystem.ResetStages();
                    _build = null;
                } else {
                    _buildSystem.MovePreviousBuildStage();
                }
            }
        }

        if (_buildSystem.Disable) {
            if (_map.IsOccupy(PointOnMap)) {
                var build = _map.GetBuild(PointOnMap);
                _map.HighLightCell(build.PointOnMap, build.Build.Size);
            }
        }

        _map.Update();
    }

    private void SelectBuild(Build build) {
        _build = build;
        _buildSystem.MoveNextBuildStage();
        _selector.Disable();
    }

    private void DeformMap(Vector2 cursor) {
        _map.ChangeShape(ToMap(cursor));
    }

    private MapBuild Build() {
        var offset = Vector3.zero;

        if ((_build.Size.x & 1) is 0) {
            offset.x -= _linearDimension * (_build.Size.x - 1) / 2.0f;
        }

        if ((_build.Size.y & 1) is 0) {
            offset.z -= _linearDimension * (_build.Size.y - 1) / 2.0f;
        }

        var point = _playerInteraction.PointOnPlane + offset;
        var newBuild = Instantiate(_build.Model, point, Quaternion.identity, _buildsParrent);

        var mapPoint = PointOnMap - _build.Size / 2;
        var build = new MapBuild(newBuild, _build, mapPoint);
        _builds.Add(build);

        return build;
    }

    private Cell CreateCell(Vector2Int mapCoordinate) {
        mapCoordinate += _edge;
        var coordinate = ToWorld(mapCoordinate);

        return Instantiate(_cellPattern, coordinate, Quaternion.identity, _mapParent);
    }

    private Vector3 ToWorld(Vector2Int mapCoordinate) {
        var localCoordinate = new Vector3(mapCoordinate.x, 0.0f, mapCoordinate.y);
        var size = new Vector3(_linearDimension, _linearDimension, _linearDimension);
        return Vector3.Scale(localCoordinate, size);
    }

    private Vector2Int ToMap(Vector2 cursor) {
        var mapX = Mathf.RoundToInt(cursor.x / _linearDimension);
        var mapY = Mathf.RoundToInt(cursor.y / _linearDimension);

        return new Vector2Int(mapX, mapY) - _edge;
    }
}

[System.Serializable]
public class MapBuild {
    private GameObject _model;
    private Build _build;
    private Vector2Int _pointOnMap;

    public MapBuild(GameObject model, Build build, Vector2Int pointOnMap)
    {
        _model = model;
        _build = build;
        _pointOnMap = pointOnMap;
    }

    public GameObject Model => _model;
    public Build Build => _build;
    public Vector2Int PointOnMap => _pointOnMap;
}
