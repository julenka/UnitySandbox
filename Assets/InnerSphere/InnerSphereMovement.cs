using UnityEngine;
using System.Collections;

public class InnerSphereMovement : MonoBehaviour
{
    public Vector3 rotation;
    public float translateMagnitude, translateOffset, translateFrequency, timeDelay;

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(rotation * Time.deltaTime) * transform.rotation;
        var tx = translateMagnitude * Mathf.Sin((Time.timeSinceLevelLoad - timeDelay) * translateFrequency) + translateOffset;
        transform.localPosition = Vector3.up * tx;
    }
}
