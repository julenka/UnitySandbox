using UnityEngine;
using System.Collections;

public class Axes : MonoBehaviour
{
    public float size;
    
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                for (int z = 0; z < size; z++)
                {
                    Gizmos.DrawWireCube(new Vector3(x, y, z) + Vector3.one * 0.5f, Vector3.one);
                }
            }
        }
    }
}
