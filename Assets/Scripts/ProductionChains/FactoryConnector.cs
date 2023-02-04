using UnityEngine;

public class FactoryConnector
{
    private enum ConnetionStatus {
        Wait,
        Start,
        Finish,
    }

    private readonly RootGrow _root;

    public FactoryConnector(RootGrow root)
    {
        _root = root;
    }

    private Vector3 _start;
    private Vector3 _finish;

    private ConnetionStatus _status;

    public void StartConnection(Vector3 startPoint) {
        _status = ConnetionStatus.Start;
        _start = startPoint;
    }

    public void LeadConnection(Vector3 point) {
        _finish = point;
    }

    public void Update() {
        var rotation = _finish - _start;
        _root.transform.position = _start;
        _root.transform.localRotation = Quaternion.Euler(rotation);
    }
}
