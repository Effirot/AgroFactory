using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed;
    private Rigidbody rigidbody;
    private Vector3 vector3;
    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        
    }
    public void SetDirection(Vector3 vector)
    {
       // rigidbody.velocity = vector3 * _speed;

       rigidbody.AddForce(vector3*_speed);
        Debug.Log(vector3);
    }
}
