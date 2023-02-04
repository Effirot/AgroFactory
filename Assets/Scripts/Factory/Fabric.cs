using System.Linq;
using UnityEngine;

[System.Serializable]
public struct Fabric
{
    [SerializeField] private Resource[] _inputs;
    [SerializeField] private Resources _output;

    public bool CanConnect(Resources resource) {
        return _inputs
            .Select(input => input.Type)
            .Contains(resource);
    }
}

[System.Serializable]
public struct Resource {
    public Resources Type;
    public int Count;
}

public enum Resources {
    Water,
    Minerals,
    Starch,
    Sugar,
    АТР,
}
