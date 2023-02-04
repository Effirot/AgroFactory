using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootGrow : MonoBehaviour
{
    [SerializeField] private List<MeshRenderer> _growRootMeshes;
    [SerializeField] private float _timeToGrow = 5.0f;
    [SerializeField] private float _refreshRate = 0.05f;
    [SerializeField, Range(0.0f, 1.0f)] private float _minGrow = 0.2f;
    [SerializeField, Range(0.0f, 1.0f)] private float _maxGrow = 0.97f;

    private List<Material> _growRootMaterials = new();
    private bool _isFullyGrown;
    
    private void Start() {
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
        if (Input.GetKeyDown(KeyCode.Space)) {
            foreach (var material in _growRootMaterials) {
                StartCoroutine(GrowRoot(material));
            }
        }
    }

    private IEnumerator GrowRoot(Material material) {
        var growValue = material.GetFloat("_Grow");

        if (_isFullyGrown is false) {
            while (growValue < _maxGrow) {
                growValue += 1.0f / (_timeToGrow / _refreshRate);
                material.SetFloat("_Grow", growValue);

                yield return new WaitForSeconds(_refreshRate);
            }
        } else {
            while (growValue > _minGrow) {
                growValue -= 1.0f / (_timeToGrow / _refreshRate);
                material.SetFloat("_Grow", growValue);

                yield return new WaitForSeconds(_refreshRate);
            }
        }

        _isFullyGrown = growValue >= _maxGrow;
    }
}
