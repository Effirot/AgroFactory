using System;
using UnityEngine;

public class PlayerMapInteraction {
    private readonly float _linearDimension;
    private readonly Plane _plane;

    public PlayerMapInteraction(float linearDimension)
    {
        _linearDimension = linearDimension;
        _plane = new Plane(Vector3.up, Vector3.zero);
    }

    public Vector2 Cursor { get; private set; }
    public Vector3 PointOnPlane { get; private set; }

    public void Update() {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (_plane.Raycast(ray, out float enter)) {
            var hitPoint = ray.GetPoint(enter);

            Cursor = GetMapCoordinate(hitPoint);
            PointOnPlane = new Vector3(Cursor.x, 0.0f, Cursor.y);
        }
    }

    private Vector2 GetMapCoordinate(Vector3 rawCoordinate) {
        var x = GetCoordinate(rawCoordinate.x);
        var z = GetCoordinate(rawCoordinate.z);

        return new Vector2(x, z);
    }

    private float GetCoordinate(float rawCoordinate) {
        return Mathf.Round(rawCoordinate / _linearDimension) * _linearDimension;
    }
}
