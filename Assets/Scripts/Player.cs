
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed = 10f;
    [SerializeField] private Transform rotateObject;
    private Rigidbody rigidbody;
    private Vector3 vector3;
    private Vector3 vector3Rotate;
    public float speedRotate = 10f;
    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        rigidbody.AddForce(vector3 * _speed);
        rotateObject.rotation = Quaternion.Lerp(rotateObject.rotation,Quaternion.LookRotation(vector3Rotate),Time.deltaTime * speedRotate);
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
