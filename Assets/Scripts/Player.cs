using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _speedRotate = 5f;
    [SerializeField] private Transform rotateObject;
    private Rigidbody rigidbody;
    private Vector3 vector3;
    private Vector3 vector3Rotate = new();

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        rigidbody.AddForce(vector3 * _speed);
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
}
