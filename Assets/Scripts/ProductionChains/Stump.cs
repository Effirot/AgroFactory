using UnityEngine;

public class Stump : MonoBehaviour
{
    [SerializeField] private ParticleSystem _highlight;
    [SerializeField] private ParticleSystem _stump;
    [SerializeField] private Resource _input;
    [SerializeField] private Vector2 _size;

    public Vector3 Center {
        get {
            var position = transform.position;
            position.y = 0.0f;
            return position;
        }
    }

    private void Start() {
        // _highlight.Stop();
        // _stump.Stop();
    }

    public bool IsCursorInside(Vector3 cursor) {
        var insideX = Center.x - _size.x <= cursor.x && cursor.x <= Center.x + _size.x;
        var insideY = Center.z - _size.y <= cursor.z && cursor.z <= Center.z + _size.y;

        return insideX && insideY;
    }

    public bool CanConnect(Fabric inputFabric) {
        return _input.Type == inputFabric.Type;
    }

    public void Input(Resource resource) {
        if (_input.Type != resource.Type) return;

        _input.Current += resource.Current;
    }

    public void HighLight() {
        if (_highlight.isPlaying) return;

        _highlight.Play();
    }

    public void Restore() {
        _highlight.Stop();
    }

    public void Connect() {
        _highlight.Stop();
        _stump.Play();
    }
}
