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

    [Header("Создание цепочек")]
    [SerializeField] private float _deltaTime;
    [SerializeField] private RootGrow _rootPattern;
    [SerializeField] private Stump _stump;
    [SerializeField] private FactoryInformator _factoryInformation;

    private Vector2Int _edge;
    private PlayerInput _input;
    private InputAction _leftButtonClick;
    private InputAction _rightButtonClick;

    private Build _build;
    private List<MapBuild> _builds = new();
    private RootGrow _root;
    private float _time;

    private Map _map;
    private PlayerMapInteraction _playerInteraction;
    private BuildSystem _buildSystem;
    private FactoryConnector _connector;

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
        _connector = new FactoryConnector();

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

        if (_map.IsOccupy(PointOnMap) && _map.IsInsideBound(PointOnMap) && _connector.Disable && _buildSystem.Disable) {
            var build = _map.GetBuild(PointOnMap);

            if (build is not null) {
                _map.HighLightCell(build.PointOnMap, build.Build.Size);
                _factoryInformation.Enable();
                _factoryInformation.SetInfo(build.Fabric.GetResorces(), build.Fabric.Output);

                if (_leftButtonClick.WasPressedThisFrame()) {
                    if (_root == null) {
                        _root = Instantiate(_rootPattern);
                    }

                    _root.gameObject.SetActive(true);
                    _connector.StartConnection(build, _root);
                }
            }
        } else if (_connector.Enable && _map.IsInsideBound(PointOnMap)) {
            _map.HighLightCell(_connector.Build.PointOnMap, _connector.Build.Build.Size);
            var build = _map.GetBuild(PointOnMap);

            if (build is not null) {
                if (_map.IsOccupy(PointOnMap) && build != _connector.Build) {
                    _connector.LeadConnection(build.Model.transform.position);

                    if (_connector.IsInRadius && build.Fabric.CanConnect(inputFabric: _connector.Build.Fabric)) {
                        _map.HighLightCell(build.PointOnMap, build.Build.Size);
                        _factoryInformation.Enable();
                        _factoryInformation.SetInfo(build.Fabric.GetResorces(), build.Fabric.Output);

                        if (_leftButtonClick.WasPressedThisFrame()) {
                            _connector.Cancel();
                            _connector.Build.Next.Add(build);

                            _root = null;
                        }
                    }
                }
            } else {
                _factoryInformation.Disable();
                if (_stump.IsCursorInside(_playerInteraction.PointOnPlane)) {
                    _connector.LeadConnection(_stump.Center);

                    if (_connector.IsInRadius && _stump.CanConnect(inputFabric: _connector.Build.Fabric)) {
                        _stump.HighLight();

                        if (_leftButtonClick.WasPressedThisFrame()) {
                            _stump.Connect();
                            _connector.Cancel();
                            _connector.Build.Stump = _stump;
                            _root = null;
                        }
                    }
                } else {
                    _stump.Restore();
                    _connector.LeadConnection(_playerInteraction.PointOnPlane);
                }
            }

            if (_rightButtonClick.WasPressedThisFrame()) {
                _connector.Cancel();
                _root.gameObject.SetActive(false);
            }
        } else if (_connector.Disable) {
            _factoryInformation.Disable();

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
        }
        
        foreach (var build in _builds) { 
            build.Fabric.Update(Time.deltaTime);
        }

        if (_time >= _deltaTime) {
            foreach (var build in _builds) {
                if (build.Stump != null) {
                    build.Stump.Input(build.Fabric.GetOne());
                }

                foreach (var next in build.Next) {
                    if (build.Fabric.HasResource) {
                        next.Fabric.Input(build.Fabric.GetOne());
                    }
                }
            }

            _time = 0.0f;
        }

        _connector.Update();
        _map.Update();

        _time += Time.deltaTime;
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
    private Fabric _fabric;

    public List<MapBuild> Next = new();
    public Stump Stump = null;

    public MapBuild(GameObject model, Build build, Vector2Int pointOnMap)
    {
        _model = model;
        _build = build;
        _pointOnMap = pointOnMap;
        _fabric = build.Fabric.Clone();
        
        _fabric.Init();
    }

    public GameObject Model => _model;
    public Build Build => _build;
    public Vector2Int PointOnMap => _pointOnMap;
    public ref Fabric Fabric => ref _fabric;
}
