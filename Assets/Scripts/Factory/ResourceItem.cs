using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class ResourceItem : MonoBehaviour
{
    public ResourceParameters recource;
    public void AddToFactory()
    {
        FactoryItem.Instance.AddResource(recource);
    }
    public enum ResourceType
    {
        A, B, C
    }


    [Serializable]
    public class ResourceParameters
    {        
        [SerializeField] public ResourceType type;
        public int count { get => _count; set { _count = value; if (_count == _countNeed) isComplete = true; } }
        [SerializeField] private int _count;
        public int countNeed { get => _countNeed; set { _countNeed = value; } }
        [SerializeField] private int _countNeed = 1;
        public bool isComplete { get => _isComplete; set { _isComplete = value; FactoryItem.Instance.CreateNewResource(); } }
        private bool _isComplete;

        public void Reset()
        {
            count = 0;
            _isComplete = false;
        }
    }
}
