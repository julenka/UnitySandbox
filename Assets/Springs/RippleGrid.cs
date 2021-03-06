﻿using UnityEngine;
using System.Collections;

public class RippleGrid : MonoBehaviour
{
    public static RippleGrid Instance;
    public void Awake()
    {
        Instance = this;
        Generate();
    }

    public int m_gridWidth, m_gridHeight;
    public float m_cellWidth, m_cellHeight;
    // Cube prefab has:
    // Spring
    // RigidBody
    public GameObject m_cubePrefab;

    public GameObject[,] m_cubes;

    public bool m_wrap;
    public Vector2[] m_fixedPoints;
    public uint[] m_fixedRows, m_fixedCols;

    public float Period = 5;
    public float Amplitude = 5;
    public bool m_animate;

    public int OffsetKinematic = 0;

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(transform.position, new Vector3(
            m_cellWidth * (m_gridWidth - 1), m_cellHeight * (m_gridHeight - 1), m_cellHeight));
    }

    public GameObject GetCubeByIndex(int i)
    {
        return m_cubes[i / m_gridWidth, i % m_gridWidth];
    }

    private void SetIsKinematic(RippleCube rippleCube, int row, int col)
    {
        // check coordinates
        foreach (var item in m_fixedPoints)
        {
            if (item.x == col && item.y == row)
            {
                rippleCube.m_isKinematic = true;
            }
        }
        // check row
        foreach (var r in m_fixedRows)
        {
            if (r == row)
            {
                rippleCube.m_isKinematic = true;
            }
        }
        foreach (var c in m_fixedCols)
        {
            if (c == col)
            {
                rippleCube.m_isKinematic = true;
            }
        }
        // check column
    }

    void Generate()
    {
        // make the grid of cubes
        m_cubes = new GameObject[m_gridHeight, m_gridWidth];

        var offset = new Vector3(-(m_cellWidth * m_gridWidth) / 2, -(m_cellHeight * m_gridHeight) / 2, 0);
        for (int i = 0; i < m_gridHeight; i++)
        {
            for (int j = 0; j < m_gridWidth; j++)
            {
                // position
                var c = Instantiate(m_cubePrefab);
                var pos = new Vector3(m_cellWidth * j, m_cellHeight * i, 0);
                pos += offset;
                c.transform.parent = transform;
                c.transform.localPosition = pos;

                SetIsKinematic(c.GetComponent<RippleCube>(), i, j);


                //if (m_animate && i == m_gridHeight / 2 && j == m_gridWidth / 2)
                if (m_animate && i == 0 / 2 && j == 0 / 2)
                {
                    c.GetComponent<RippleCube>().m_animate = true;
                }

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
            if (!m_wrap)
            {
                r = Mathf.Clamp(row + dys[i], 0, m_gridHeight - 1);
                c = Mathf.Clamp(col + dxs[i], 0, m_gridWidth - 1);
            }
            if (r < row || c < col)
            {
                r = row;
                c = col;
            }
            result[i] = m_cubes[r, c].gameObject;
        }
        return result;
    }

    int mod(int k, int n) { return ((k %= n) < 0) ? k + n : k; }
}
