using UnityEngine;

public class FactoryConnector
{
    private enum ConnetionStatus {
        Disable,
        Start,
        Finish,
    }

    private RootGrow _root;
    
    private Vector3 _start;
    private Vector3 _finish;

    private MapBuild _build;
    private ConnetionStatus _status;

    public bool Enable => _status is not ConnetionStatus.Disable;
    public bool Disable => _status is ConnetionStatus.Disable;

    public MapBuild Build => _build;
    public bool IsInRadius => (_finish - _start).magnitude <= _root.MaxLength;

    public void StartConnection(MapBuild build, RootGrow root) {
        _root = root;

        _status = ConnetionStatus.Start;
        _start = build.Model.transform.position;
        _build = build;
    }

    public void LeadConnection(Vector3 point) {
        _finish = point;
    }

    public void Update() {
        if (Disable) return;

        var rotation = _finish - _start;
        _root.transform.position = _start;
        _root.transform.localRotation = Quaternion.LookRotation(rotation, Vector3.up);
        _root.UpdateGrow(rotation.magnitude);
    }

    public void Cancel() {
        _status = ConnetionStatus.Disable;
    }
}
