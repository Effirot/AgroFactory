using System.Linq;
using UnityEngine;

[System.Serializable]
public struct Fabric
{
    [SerializeField] private Resource[] _inputs;
    [SerializeField] private Resource _output;

    public bool IsSourse => _inputs.Any() is false;

    public bool CanConnect(ResourceType resource) {
        return _inputs
            .Select(input => input.Type)
            .Contains(resource);
    }
}

[System.Serializable]
public struct Resource {
    public ResourceType Type;
    public int Count;
}

public enum ResourceType {
    Water,
    Minerals,
    Starch,
    Sugar,
    АТР,
    Keratin,
}
