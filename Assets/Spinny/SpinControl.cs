using UnityEngine;

/// <summary>
/// Provides ability to control the spin of an object via angular velocity.
/// Spin can be dampened
/// </summary>
class SpinControl : MonoBehaviour
{
    Rigidbody rigidBody;
    public float TorqueMagnitude;
    public float TorqueDampen = 0.7f;

    // Degrees per second in X, Y, Z axes
    private Vector3 angularVelocity;

    private Quaternion initialRotation;

    public void IncreaseSpin(Vector3 amount)
    {
        angularVelocity += amount;
    }

    public void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        initialRotation = transform.localRotation;
    }

    public void Update()
    {
        // update velocity
        if (Input.GetKey(KeyCode.Space))
        {
            IncreaseSpin(Vector3.up * TorqueMagnitude * Time.deltaTime);
        }
        angularVelocity *= (1f - TorqueDampen * Time.deltaTime);

        // Update rotation
        var rotationDelta = Quaternion.EulerAngles(angularVelocity * Time.deltaTime);
        transform.localRotation = rotationDelta * transform.localRotation;

        // Check reset condition (TODO: refactor?)
        if (angularVelocity.magnitude < 0.05f)
        {
            transform.localRotation = initialRotation;
        }
    }
}
