using UnityEngine;
using System.Collections;

public class RippleGrid : MonoBehaviour
{
    public int m_gridWidth, m_gridHeight, m_cellWidth, m_cellHeight;
    // Cube prefab has:
    // Spring
    // RigidBody
    public GameObject m_cubePrefab;

    public GameObject[,] m_cubes;


    // Use this for initialization
    void Start()
    {
        // make the grid of cubes
        m_cubes = new GameObject[m_gridHeight, m_gridWidth];

        for (int i = 0; i < m_gridHeight; i++)
        {
            for (int j = 0; j < m_gridWidth; j++)
            {
                // position
                var c = Instantiate(m_cubePrefab);
                var pos = new Vector3(m_cellWidth * j, m_cellHeight * i, 0);
                c.transform.position = pos;
                c.transform.parent = transform;

                m_cubes[i, j] = c;
            }
        }

        // hook up joints
        for (int i = 0; i < m_gridHeight; i++)
        {
            for (int j = 0; j < m_gridWidth; j++)
            {
                var neighbors = GetNeighbors(i, j);
                m_cubes[i, j].GetComponent<RippleCube>().SetNeighbors(neighbors);
            }
        }
    }

    private GameObject[] GetNeighbors(int row, int col)
    {
        // loop around
        var result = new GameObject[4];
        var dxs = new int[] { 0, 0, -1, 1 };
        var dys = new int[] { -1, 1, 0, 0 };

        for (int i = 0; i < 4; i++)
        {
            var r = mod(row + dys[i], m_gridHeight);
            var c = mod(col + dxs[i], m_gridWidth);
            result[i] = m_cubes[r, c].gameObject;
        }
        return result;
    }

    int mod(int k, int n) { return ((k %= n) < 0) ? k + n : k; }
}
