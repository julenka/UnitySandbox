using UnityEngine;
using System;

public partial class WireframeTesseract : MonoBehaviour
{
    [Serializable]
    public class FaceParams
    {
        public FaceParams(int a, int b, int c, int d)
        {
            this.a = a;
            this.b = b;
            this.c = c;
            this.d = d;
        }
        public int a, b, c, d;
        public Color faceColor;
        public string faceGroup;
    }

    [Serializable]
    public class CubeParams
    {
        public CubeParams(String tag, Color cubeColor, int[] faceIndices)
        {
            this.faceIndices = faceIndices;
            this.cubeColor = cubeColor;
            this.tag = tag;
        }
        public int[] faceIndices;
        public Color cubeColor;
        public String tag;
        public bool drawCube;
    }

    [Serializable]
    public class RotationParams
    {
        public Axis4D axis;
        public float amount;
        public RotationParams(Axis4D axis, float amount)
        {
            this.axis = axis;
            this.amount = amount;
        }
    }
}
