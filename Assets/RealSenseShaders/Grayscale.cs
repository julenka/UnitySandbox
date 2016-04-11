using UnityEngine;
using System.Collections;

namespace UnitySandbox.RealSenseShaders
{
    public class Grayscale : MonoBehaviour
    {
        private Texture2D texture;

        // Use this for initialization
        void Start()
        {
            texture = new Texture2D(TestData.DATA_WIDTH, TestData.DATA_HEIGHT, TextureFormat.R16, false);
            texture.name = "TestData Grayscale";
            GetComponent<MeshRenderer>().material.mainTexture = texture;

        }

        void Update()
        {
            // fill the texture
            texture.LoadRawTextureData(TestData.TestImage2);
            texture.Apply();
        }

    }
}
