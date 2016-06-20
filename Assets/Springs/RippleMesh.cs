using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(RippleGrid))]
public class RippleMesh : MonoBehaviour
{
    private Mesh mesh;
    private Vector3[] vertices;
    private RippleGrid grid;

    // Use this for initialization
    void Start()
    {
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "Ripple Mesh";
        grid = GetComponent<RippleGrid>();
        CreateVertices();
        CreateTriangles();

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.matrix = transform.localToWorldMatrix;
        if (vertices == null)
        {
            return;
        }
        for (int i = 0; i < vertices.Length; i++)
        {
            Gizmos.DrawSphere(vertices[i], 0.1f);
        }
    }

    private void CreateTriangles()
    {
        int[] triangles = new int[vertices.Length * 6];
        int tIndex = 0;
        for (int row = 0; row < grid.m_gridHeight - 1; row++)
        {
            for (int col = 0; col < grid.m_gridWidth - 1; col++)
            {
                var o00 = RowColToIndex(row, col);
                var o10 = RowColToIndex(row + 1, col);
                var o01 = RowColToIndex(row, col + 1);
                var o11 = RowColToIndex(row + 1, col + 1);
                tIndex = SetQuad(triangles, tIndex, o00, o10, o01, o11);
            }
        }
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }

    private int RowColToIndex(int row, int col)
    {
        return row * grid.m_gridWidth + col;
    }

    private void CreateVertices()
    {
        vertices = new Vector3[grid.m_cubes.Length];
        UpdateVertices();
        mesh.vertices = vertices;
    }

    private void UpdateVertices()
    {
        for (int i = 0; i < grid.m_cubes.Length; i++)
        {
            vertices[i] = grid.m_cubes[i / grid.m_gridWidth, i % grid.m_gridWidth].transform.localPosition;
        }
        mesh.vertices = vertices;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateVertices();
        mesh.RecalculateNormals();
        if (Input.GetMouseButton(0))
        {
            HandleInput();
        }
    }

    private static int
SetQuad(int[] triangles, int i, int v00, int v10, int v01, int v11)
    {
        triangles[i] = v00;
        triangles[i + 1] = triangles[i + 4] = v01;
        triangles[i + 2] = triangles[i + 3] = v10;
        triangles[i + 5] = v11;
        return i + 6;
    }

    public float m_force = 5000;
    public float m_size = 5;
    void HandleInput()
    {
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        Ray[] specles = new Ray[16];
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                var rot = Quaternion.Euler(m_size * i - 2 * m_size, m_size * j - 2 * m_size, 0);
                specles[i * 4 + j] = new Ray(inputRay.origin, rot * inputRay.direction);
            }
        }

        for (int i = 0; i < specles.Length; i++)
        {
            inputRay = specles[i];
            if (Physics.Raycast(inputRay, out hit))
            {
                Debug.DrawLine(inputRay.origin, hit.point, Color.red);

                Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
                if (rb)
                {
                    rb.AddForce(Vector3.down * m_force);
                }
            }
        }

    }


}
