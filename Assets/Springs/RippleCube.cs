using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class RippleCube : MonoBehaviour
{
    public GameObject[] m_neighbors;
    public bool m_isKinematic = false;

    public bool m_animate = false;

    public void Start()
    {
        GetComponent<Rigidbody>().isKinematic = m_animate || m_isKinematic;
    }

    public void Update()
    {
        if (m_animate)
        {
            var t = Time.timeSinceLevelLoad;
            var z = RippleGrid.Instance.Amplitude * Mathf.Sin(t * RippleGrid.Instance.Period);
            transform.localPosition =
                new Vector3
                (transform.localPosition.x,
                transform.localPosition.y,
                z
            );
        }
    }
    public float m_spring = 1000;
    public void SetNeighbors(GameObject[] neighbors)
    {
        for (int k = 0; k < neighbors.Length; k++)
        {
            var neighbor = neighbors[k];
            if (neighbor == null || neighbor.Equals(gameObject))
            {
                neighbors[k] = null;
                continue;
            }
            var spring = gameObject.AddComponent<SpringJoint>();
            spring.connectedBody = neighbor.GetComponent<Rigidbody>();
            spring.spring = m_spring;
            spring.damper = 100f;
            //spring.minDistance = 0f;
            //spring.maxDistance = 1.5f;

            if (m_animate)
            {
                GetComponent<Rigidbody>().isKinematic = true;
            }
        }
        m_neighbors = neighbors;
    }

    public void OnDrawGizmosSelected()
    {
        foreach (var item in m_neighbors)
        {
            if (item == null)
            {
                continue;
            }
            Gizmos.color = Color.red;
            Gizmos.DrawCube(item.transform.position, Vector3.one * 0.3f);
        }
    }
}
