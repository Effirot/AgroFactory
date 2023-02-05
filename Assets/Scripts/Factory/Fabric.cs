using System;
using System.Linq;
using UnityEngine;

[Serializable]
public struct Fabric
{
    [SerializeField] private Resource[] _inputs;
    [SerializeField] private Resource _output;
    [SerializeField] private float _time;
    [SerializeField] private bool _isSourse;

    private float _totalTime;

    public bool HasResource => _output.Current > _output.Count;

    public ResourceType Type => _output.Type;

    public Resource[] GetResorces()
    {
        return _inputs;
    }

    public void Init() {
        
    }

    public Fabric Clone() {
        var newInputs = new Resource[_inputs.Length];
        Array.Copy(_inputs, newInputs, _inputs.Length);

        return new Fabric {
            _inputs = newInputs,
            _output = _output,
            _time = _time,
            _isSourse = _isSourse,
        };
    }

    public bool CanConnect(Fabric inputFabric) {
        return _inputs
            .Select(input => input.Type)
            .Contains(inputFabric._output.Type);
    }

    public void Update(float deltaTime) {
        if (_totalTime >= _time) {
            if (_isSourse is false) {
                var canOutput = true;

                foreach (var input in _inputs) {
                    canOutput &= input.CanOutput;
                }

                if (canOutput) {
                    _totalTime = 0.0f;
                    
                    for (int i = 0; i < _inputs.Length; i++) {
                        _inputs[i].ReduceOne();
                    }

                    _output.Current++;
                }
            }

            if (_isSourse) {
                _output.Current++;
                _totalTime = 0.0f;
            }
        }

        _totalTime += deltaTime;
    }

    public void Input(Resource resource) {
        for (int i = 0; i < _inputs.Length; i++) {
            if (_inputs[i].Type == resource.Type) {
                _inputs[i].Current += resource.Current;
            }
        }
    }

    public Resource GetOne() {
        var result = _output;
        result.Current = 1;
        _output.Current--;

        return result;
    }
}

[Serializable]
public struct Resource {
    public ResourceType Type;
    public int Count;
    public int Current;
    
    public bool CanOutput => Current > Count;
    public int Output => Current % Count;

    public void ReduceOne() {
        Current -= Count;
    }

    public void Reduce(int count) {
        Current -= Count * count;
    }
}

public enum ResourceType {
    Water,
    Minerals,
    Starch,
    Sugar,
    АТР,
    Keratin,
}
