using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static ResourceItem;

public class FactoryItem : MonoBehaviour
{
    [SerializeField] public List<ResourceParameters> resourcesToCreateNew;
    public ResourceItem newResource;
    public static FactoryItem Instance;
    private void Start()
    {
        Instance = this;
    }
    public void CreateNewResource()
    {
        bool GoCreate = false;
        foreach(var resource in resourcesToCreateNew)
        {
            GoCreate = resource.isComplete;
        }
        if (!GoCreate)
        {
            Debug.Log("Недостаточно ресурсов");
            return;
        }

        Debug.Log("Ебашу новый ресурс");
        Instantiate(newResource.gameObject);
        foreach (var resource in resourcesToCreateNew)
        {
            resource.Reset();
        }
    }
    public void AddResource(ResourceParameters resource)
    {
        ResourceParameters resourceParameters = resourcesToCreateNew.Where(x => x.type == resource.type).FirstOrDefault();
        if(resourceParameters == null)
        {
            Debug.Log("Нет такого ресурса");
            return;
        }

        resourceParameters.count++;
    }

}


