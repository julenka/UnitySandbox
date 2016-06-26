using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class RippleCube : MonoBehaviour
{
    public GameObject[] m_neighbors;
    public bool m_isKinematic = false;
    public bool m_animate = false;
    public float m_spring = 1000;
    public float m_damper = 1000;
    public enum JointType
    {
        Spring,
        Fixed,
        Hinge
    };
    public JointType JointToUse;


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
                z,
                transform.localPosition.z
            );
        }
        UpdateJoints();
    }

    private void UpdateJoints()
    {
        foreach (var neighbor in m_neighbors)
        {
            if (neighbor == null)
            {
                continue;
            }
            var spring = neighbor.GetComponent<SpringJoint>();
            if (spring != null)
            {
                spring.spring = m_spring;
                spring.damper = m_damper;
            }
        }
    }

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
            var spring = GetSpringObject();
            spring.connectedBody = neighbor.GetComponent<Rigidbody>();

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

    private Joint GetSpringObject()
    {
        if (JointToUse == JointType.Fixed)
        {
            var result = gameObject.AddComponent<FixedJoint>();
            return result;
        }
        else if (JointToUse == JointType.Hinge)
        {
            var result = gameObject.AddComponent<HingeJoint>();
            return result;
        }
        else
        {
            var result = gameObject.AddComponent<SpringJoint>();
            result.spring = m_spring;
            result.damper = m_damper;
            result.maxDistance = 2;
            return result;
        }
    }

}
