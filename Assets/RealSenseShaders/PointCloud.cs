#if UNITY_STANDALONE
#define IMPORT_GLENABLE
#endif


using UnityEngine;
using System.Collections;
using System;
using System.Runtime.InteropServices;

namespace UnitySandbox.RealSenseShaders
{
    public class PointCloud : MonoBehaviour
    {
        public int MeshSize = 250;
        private Texture2D m_texture;
        private bool _mIsOpenGl;
        // Use this for initialization
        private void Start()
        {
            m_texture = new Texture2D(TestData.DATA_WIDTH, TestData.DATA_HEIGHT, TextureFormat.R16, false);
            m_texture.name = "TestData Grayscale";
            m_texture.wrapMode = TextureWrapMode.Clamp;
            GetComponent<MeshRenderer>().material.mainTexture = m_texture;

            GetComponent<MeshFilter>().mesh = MakeMesh(MeshSize);

            _mIsOpenGl = SystemInfo.graphicsDeviceVersion.Contains("OpenGL");
        }

        private void Update()
        {
            // fill the texture
            m_texture.LoadRawTextureData(TestData.TestImage2);
            m_texture.Apply();
        }

        Mesh MakeMesh(int size)
        {

            Vector3[] vertices = new Vector3[size * size];
            int[] indices = new int[size * size];
            Color[] colors = new Color[size * size];
            Vector2[] texcoords = new Vector2[size * size];

            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    Vector2 uv = new Vector2(x / (size - 1.0f), y / (size - 1.0f));
                    Vector3 pos = new Vector3(uv.x - 0.5f, 0, uv.y - 0.5f);

                    vertices[x + y * size] = pos;
                    indices[x + y * size] = x + y * size;
                    colors[x + y * size] = Color.green;
                    texcoords[x + y * size] = uv;
                }
            }

            Mesh mesh = new Mesh();

            mesh.vertices = vertices;
            mesh.colors = colors;
            mesh.uv = texcoords;
            mesh.SetIndices(indices, MeshTopology.Points, 0);

            return mesh;
        }

        // http://www.kamend.com/2014/05/rendering-a-point-cloud-inside-unity/
        const uint GL_VERTEX_PROGRAM_POINT_SIZE = 0x8642;
        const uint GL_POINT_SMOOTH = 0x0B10;

        const string LibGLPath = "opengl32.dll";

#if IMPORT_GLENABLE
        [DllImport(LibGLPath)]
        public static extern void glEnable(UInt32 cap);

        private bool mIsOpenGL;

        void OnPreRender()
        {
            glEnable(GL_VERTEX_PROGRAM_POINT_SIZE);
            glEnable(GL_POINT_SMOOTH);
        }
#endif
    }

}