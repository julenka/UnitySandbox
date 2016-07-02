using UnityEngine;
using System.Collections;

public class SpinOnCollision : MonoBehaviour
{

    private Vector3 previousPosition;
    public LayerMask layerMask;
    public float TorqueMultiplier = 1000;
    public float Exponent = 2;

    // Use this for initialization
    void Start()
    {
        previousPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        var direction = transform.position - previousPosition;
        var distance = direction.magnitude;
        Debug.DrawLine(transform.position, previousPosition);
        var allHit = Physics.RaycastAll(previousPosition, direction, distance, layerMask);

        foreach (var hitInfo in allHit)
        {
            var torqueTest = hitInfo.collider.gameObject.GetComponent<SpinControl>();
            if (torqueTest != null)
            {
                torqueTest.IncreaseSpin(Vector3.up * -1f * Mathf.Sign(Vector3.Dot(direction, transform.right)) * Mathf.Pow(distance, Exponent) * TorqueMultiplier);
                Debug.DrawRay(hitInfo.point, Vector3.up, Color.red);
            }
        }

        previousPosition = transform.position;
    }
}
