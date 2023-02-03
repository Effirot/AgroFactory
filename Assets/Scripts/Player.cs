
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
        rigidbody.AddForce(vector3 * _speed);
    }
    public void SetDirection(Vector3 vector)
    {
      // rigidbody.velocity = vector * _speed;
      vector3 = vector;
        Debug.Log(vector);
    }

}
