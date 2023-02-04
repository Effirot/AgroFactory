using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _speedRotate = 5f;
    [SerializeField] private Transform rotateObject;
    [SerializeField] private Animator _animator;
    private static readonly int _isRunningKey = Animator.StringToHash("is-running");
    private static readonly int _isInteractKey = Animator.StringToHash("is-interact");
    private Rigidbody _rigidbody;
    private Vector3 vector3;
    private Vector3 vector3Rotate = new();
    private bool _isInteract = false;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        _rigidbody.AddForce(vector3 * _speed);
        _animator.SetBool(_isRunningKey, vector3 != Vector3.zero);
        _animator.SetBool(_isInteractKey, _isInteract);
        if (vector3Rotate != Vector3.zero)        
            rotateObject.rotation = Quaternion.Lerp(rotateObject.rotation,Quaternion.LookRotation(vector3Rotate),Time.deltaTime * _speedRotate);
    }
    public void SetDirection(Vector3 vector)
    {
        vector3 = vector;
        if(vector != Vector3.zero)
        {
            vector3Rotate = vector;
        }
    }
    public void SetInteract(bool bl)
    {
        _isInteract = bl;
    }
}
