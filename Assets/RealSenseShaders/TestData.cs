using UnityEngine;
using System.Collections;
using System.IO;

namespace UnitySandbox.RealSenseShaders
{
    /// <summary>
    /// Generates data for testing our texture
    /// In this case it will be a byte array that 
    /// represents 16-bit pixels, pixel image dimensions
    /// are 640 x 480. Vertical stripes ranging from 0 to 2048.
    /// </summary>
    public class TestData : MonoBehaviour
    {

        public static byte[] TestImage;

        public const int DATA_WIDTH = 640;
        public const int DATA_HEIGHT = 480;
        public const int BYTES_PER_PIXEL = 2;

        void Awake()
        {
            TestImage = new byte[DATA_WIDTH * DATA_HEIGHT * BYTES_PER_PIXEL];
            BinaryWriter bw = new BinaryWriter(new MemoryStream(TestImage, true));

            for (int y = 0; y < DATA_HEIGHT; y++)
            {
                for (int x = 0; x < DATA_WIDTH; x++)
                {
                    ushort value = (ushort)((float)x / DATA_WIDTH * 2048.0f);
                    bw.Write(value);
                }
            }
            bw.Flush();
            bw.Close();
        }
    }
}

