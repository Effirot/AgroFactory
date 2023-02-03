using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMapInteraction : MonoBehaviour {
    [SerializeField] private Marker _marker;
    [SerializeField, Range(0.1f, 5.0f)] private float _linearDimension;

    private Plane _plane;

    private void Start() {
        _plane = new Plane(Vector3.up, Vector3.zero);
    }

    private void Update() {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (_plane.Raycast(ray, out float enter)) {
            var hitPoint = ray.GetPoint(enter);

            _marker.transform.position = GetMapCoordinate(hitPoint);
        }
    }

    private Vector3 GetMapCoordinate(Vector3 rawCoordinate) {
        var x = GetCoordinate(rawCoordinate.x);
        var z = GetCoordinate(rawCoordinate.z);

        return new Vector3(x, 0.0f, z);
    }

    private float GetCoordinate(float rawCoordinate) {
        return Mathf.Round(rawCoordinate / _linearDimension) * _linearDimension;
    }
}
