using UnityEngine;

public class Cell : MonoBehaviour {
    private enum OccupationStatus {
        Free,
        Occupied,
    }

    private enum HighLightStatus {
        Normal,
        Prepare,
        Wrong,
    }

    [SerializeField, Range(0.1f, 5.0f)] private float _maxHeight;
    [SerializeField, Range(0.1f, 5.0f)] private float _maxHeightDelta;

    private MapBuild _build = null;

    private bool _isDirty = false;
    private OccupationStatus _status = OccupationStatus.Free;
    private MeshRenderer _renderer;

    private float _initHeight;
    private float _targetHeight;

    public bool IsOccupied => _status is OccupationStatus.Occupied;
    public bool IsFree => _status is OccupationStatus.Free;
    public bool HasBuild => _build != null;
    public bool HasNoBuild => HasBuild is false;

    public MapBuild Build => _build;

    public void Init() {
        _initHeight = transform.position.y;
        _renderer = GetComponentInChildren<MeshRenderer>();
    }

    public void Update() {
        if (_isDirty is false) return;

        var position = transform.position;
        position.y = Mathf.MoveTowards(position.y, _targetHeight, _maxHeightDelta * Time.deltaTime);
        transform.position = position;

        if (position.y == _targetHeight) {
            _isDirty = false;
            return;
        }
    }

    public void HighLight() {
        _renderer.material.color = Color.grey;
    }

    public void SetBuild(MapBuild build) {
        _build = build;
    }

    public void Prepare() {
        var color = _status switch {
            OccupationStatus.Free => Color.green,
            OccupationStatus.Occupied => Color.red,
            _ => Color.white,
        };

        _renderer.material.color = color;
    }

    public void Occupy() {
        _status = OccupationStatus.Occupied;
    }

    public void Restore() {
        _isDirty = true;

        var color = _status switch {
            OccupationStatus.Occupied => new Color(0.8f, 0.8f, 0.8f, 1.0f),
            _ => Color.white,
        };

        _renderer.material.color = color;

        _targetHeight = _initHeight;
    }

    public void Deform(float remoteness) {
        _isDirty = true;

        _targetHeight = remoteness * _maxHeight;
    }
}