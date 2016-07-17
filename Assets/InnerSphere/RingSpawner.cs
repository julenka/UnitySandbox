using UnityEngine;
using System.Collections;

public class RingSpawner : MonoBehaviour
{
    public int ringCount = 10;
    public Vector3 initialPosition = Vector3.down * 4.5f, rotationVelocity;
    public float translateMagnitude = 5f, translateFrequency = 3.14f, spawnInterval = 0.1f;

    public InnerSphereMovement ringPrefab;


    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < ringCount; i++)
        {
            var ring = MakeRing();
            ring.timeDelay = spawnInterval * i;
        }
    }


    private void Update()
    {
        var rings = GetComponentsInChildren<InnerSphereMovement>();
        foreach (var ring in rings)
        {
            UpdateRing(ring);
        }
    }
    private InnerSphereMovement MakeRing()
    {
        var ring = Instantiate<InnerSphereMovement>(ringPrefab);
        UpdateRing(ring);
        return ring;
    }

    private void UpdateRing(InnerSphereMovement ring)
    {
        ring.transform.parent = transform;
        ring.transform.localPosition = initialPosition;
        ring.translateMagnitude = translateMagnitude;
        ring.translateFrequency = translateFrequency;
        ring.rotation = rotationVelocity;
    }
}
