using System.Collections.Generic;
using UnityEngine;

public class RootGrow : MonoBehaviour
{
    [SerializeField] private List<MeshRenderer> _growRootMeshes;
    [SerializeField, Range(1.0f, 5.0f)] private float _timeToGrow = 5.0f;
    [SerializeField, Range(0.0f, 1.0f)] private float _minGrow = 0.2f;
    [SerializeField, Range(0.0f, 1.0f)] private float _maxGrow = 0.97f;
    [SerializeField, Range(1.0f, 15.0f)] private float _maxLength = 1.0f;

    private List<Material> _growRootMaterials = new();
    private bool _isFullyGrown;

    private float _length;
    private float _growValue;

    public float MaxLength => _maxLength;

    private void Start() {
        _growValue = 0.0f;

        foreach (var mesh in _growRootMeshes) {
            foreach (var material in mesh.materials) {
                if (material.HasProperty("_Grow")) {
                    material.SetFloat("_Grow", _minGrow);
                    _growRootMaterials.Add(material);
                }
            }
        }
    }

    private void Update() {
        _growValue = Mathf.MoveTowards(_growValue, _length, _timeToGrow * Time.deltaTime);

        foreach (var mesh in _growRootMeshes) {
            foreach (var material in mesh.materials) {
                material.SetFloat("_Grow", _growValue);
            }
        }
    }

    public void UpdateGrow(float length) {
        var growValue = length / _maxLength;
        growValue = Mathf.Clamp01(growValue);

        _length = _minGrow + growValue * (_maxGrow - _minGrow);
    }
}
