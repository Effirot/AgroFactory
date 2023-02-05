using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class FactoryInformator : MonoBehaviour
{
    [SerializeField] private float _offset;
    [SerializeField] private float _min;
    [SerializeField] private float _max;
    [SerializeField] private float _speed;

    [SerializeField] private ResourcePresenter[] _presenter;

    private InfoItem[] _items;

    private void Awake() {
        _items = new InfoItem[transform.childCount];

        for (int i = 0; i < _items.Length; i++) {
            var item = transform.GetChild(i) as RectTransform;
            var offset = Vector3.left * _offset * i;
            
            _items[i] = new InfoItem {
                Transform = item,
                Min = new Vector3(_min, item.localPosition.y, item.localPosition.z) + offset,
                Max = new Vector3(_max, item.localPosition.y, item.localPosition.z),
                Speed = _speed,
            };
        }

        Disable();
    }

    public void Enable() {
        if (GetComponent<Image>().enabled) return;

        for (int i = 0; i < _items.Length; i++) {
            _items[i].Reset();
            _items[i].Transform.gameObject.SetActive(true);
        }

        GetComponent<Image>().enabled = true;
    }

    public void Disable() {
        if (GetComponent<Image>().enabled is false) return;

        for (int i = 0; i < _items.Length; i++) {
            _items[i].Transform.gameObject.SetActive(false);
        }

        GetComponent<Image>().enabled = false;
    }

    public void SetInfo(Resource[] resources, Resource output) {
        _items[0].Transform.gameObject.SetActive(true);
        _items[0].Transform.GetChild(0).GetComponent<Image>().sprite = _presenter.Single(presenter => presenter.Type == output.Type).Image;
        _items[0].Transform.GetChild(1).GetComponent<TMPro.TMP_Text>().text = output.Current.ToString();

        for (int i = 1; i < _items.Length; i++) {
           if (i - 1 >= resources.Length) {
                _items[i].Transform.gameObject.SetActive(false);

                continue;
           }

           _items[i].Transform.gameObject.SetActive(true);

           _items[i].Transform.GetChild(0).GetComponent<Image>().sprite = _presenter.Single(presenter => presenter.Type == resources[i - 1].Type).Image;
           _items[i].Transform.GetChild(1).GetComponent<TMPro.TMP_Text>().text = resources[i - 1].Current.ToString();
        }
    }

    private void Update() {
        for (int i = 0; i < _items.Length; i++) {
            _items[i].Update();
        }
    }
}

[System.Serializable]
public struct InfoItem {
    public RectTransform Transform;
    public Vector3 Min;
    public Vector3 Max;
    public float Speed;

    public void Update() {
        Transform.localPosition = Vector3.MoveTowards(Transform.localPosition, Max, Speed * Time.deltaTime);
    }

    public void Reset() {
        Transform.localPosition = Min;
    }
}

[System.Serializable]
public struct ResourcePresenter {
    public ResourceType Type;
    public Sprite Image;
}
