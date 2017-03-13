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
        public Color faceColor = Color.green;
        public String faceGroup;
    }

}
