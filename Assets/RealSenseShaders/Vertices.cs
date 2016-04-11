using UnityEngine;
using System.Collections;


using System.Runtime.InteropServices;
using System;

namespace UnitySandbox.RealSenseShaders
{
    public class Vertices : MonoBehaviour
    {

        private Texture2D m_texture;

        // Use this for initialization
        void Start()
        {
            m_texture = new Texture2D(TestData.DATA_WIDTH, TestData.DATA_HEIGHT, TextureFormat.R16, false);
            m_texture.name = "TestData Grayscale";
            GetComponent<MeshRenderer>().material.mainTexture = m_texture;

            GetComponent<MeshFilter>().mesh = MakeMesh(25);
        }

        void Update()
        {
            // fill the texture
            m_texture.LoadRawTextureData(TestData.TestImage2);
            m_texture.Apply();
        }

        Mesh MakeMesh(int size)
        {

            Vector3[] vertices = new Vector3[size * size];
            Vector2[] texcoords = new Vector2[size * size];
            int[] indices = new int[size * size * 6];

            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    Vector2 uv = new Vector2(x / (size - 1.0f), y / (size - 1.0f));
                    Vector3 pos = new Vector3(uv.x - 0.5f, 0, uv.y - 0.5f);

                    texcoords[x + y * size] = uv;
                    vertices[x + y * size] = pos;
                }
            }

            int num = 0;
            for (int x = 0; x < size - 1; x++)
            {
                for (int y = 0; y < size - 1; y++)
                {
                    indices[num++] = x + y * size;
                    indices[num++] = x + (y + 1) * size;
                    indices[num++] = (x + 1) + y * size;

                    indices[num++] = x + (y + 1) * size;
                    indices[num++] = (x + 1) + (y + 1) * size;
                    indices[num++] = (x + 1) + y * size;
                }
            }

            Mesh mesh = new Mesh();

            mesh.vertices = vertices;
            mesh.uv = texcoords;
            mesh.triangles = indices;

            return mesh;
        }
    }
}

