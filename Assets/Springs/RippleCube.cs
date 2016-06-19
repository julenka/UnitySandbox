using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class RippleCube : MonoBehaviour
{
    public GameObject[] m_neighbors;

    public void SetNeighbors(GameObject[] neighbors)
    {
        for (int k = 0; k < neighbors.Length; k++)
        {
            var neighbor = neighbors[k];
            if (neighbor == null || neighbor == gameObject)
            {
                continue;
            }
            var spring = gameObject.AddComponent<SpringJoint>();
            spring.connectedBody = neighbor.GetComponent<Rigidbody>();
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
