using System;
using UnityEngine;
using UnityEngine.UI;

public class Selector : MonoBehaviour
{
    public event Action<Build> OnBuildSelected;

    [SerializeField, Range(0.1f, 10.0f)] private float _speedOfOpen;
    [SerializeField] private AnimationCurve _speedCurve;
    [SerializeField] private RectTransform _itemPattern;
    [SerializeField] private RectTransform _itemsParent;
    [SerializeField] private BuildContainer _buildContainer;
    [SerializeField] private float _radius;

    private Item[] _items;
    private float _progress = 0.0f;
    
    private void Start() {
        var angleOffet = 360.0f / _buildContainer.Builds.Length;
        angleOffet = angleOffet * Mathf.Deg2Rad;
        var i = 0;
        
        _items = new Item[_buildContainer.Builds.Length];

        foreach (var bulid in _buildContainer.Builds) {
            var position = new Vector2();
            position.y = Mathf.Cos(angleOffet * i) * _radius;
            position.x = Mathf.Sin(angleOffet * i) * _radius;

            var item = Instantiate(_itemPattern, Vector3.zero, Quaternion.identity, _itemsParent);
            item.localPosition = position;
            item.GetComponent<Image>().sprite = bulid.Icon;
            item.GetComponent<Button>().onClick.AddListener(() => OnBuildSelected?.Invoke(bulid));

            _items[i] = new Item {
                Transform = item,
                Destination = position,
                Scale = item.localScale,
            };

            i++;
        }

        Disable();
    }

    public void Update() {
        _progress += _speedOfOpen * Time.deltaTime;
        _progress = Mathf.Clamp01(_progress);

        var delta = _speedCurve.Evaluate(_progress);

        foreach (var item in _items) {
            item.Transform.localPosition = Vector3.Lerp(Vector3.zero, item.Destination, delta);
            item.Transform.localScale = Vector3.Lerp(Vector3.zero, item.Scale, delta);
        }
    }

    public void Enadle() {
        if (_itemsParent.gameObject.activeSelf) return;

        foreach (var item in _items) {
            item.Transform.localPosition = Vector3.zero;
            item.Transform.localScale = Vector3.zero;
        }

        _itemsParent.gameObject.SetActive(true);
        _progress = 0.0f;
    }

    public void Disable() {
        _itemsParent.gameObject.SetActive(false);
    }
}

internal struct Item {
    public RectTransform Transform;
    public Vector3 Destination;
    public Vector3 Scale;
}
