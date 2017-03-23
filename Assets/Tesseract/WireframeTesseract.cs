using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public partial class WireframeTesseract : MonoBehaviour
{
    public TextMesh textPrefab;

    [Header("Line Renderer")]
    public Color lineColor;

    [Header("Debugging")]
    [Range(0, 1)]
    public float pushOutAmount = 0.01f;

    //Axis4D[] rotationOrder =
    //{
    //    Axis4D.yz,
    //    Axis4D.xw,
    //    Axis4D.yw,
    //    Axis4D.zw,
    //    Axis4D.xy,
    //    Axis4D.xz
    //};


    //public Dictionary<Axis4D, float> rotation;

    List<Vector4> originalVerts;
    List<Vector4> rotatedVerts;

    public bool freezeRotation = false;
    GameObject lineRedererRoot;
    GameObject textRoot;

    public static Color[] cubeColors =
    {
        new Color32(176, 30, 0,0), // red
        new Color32(119,185,0,0), // green
        new Color32(0, 106,193,0), // blue
        new Color32(193,0,79,0), // pink
        new Color32(255,152,29,0), // yellow
        new Color32(114, 0, 172,0), // purple
        new Color32(0, 216, 204,0), // cyan
        new Color32(99,47,0, 0) // brown
    };

    public CubeParams[] cubes;
    public RotationParams[] rotation;


    void Start()
    {
        //rotation = new Dictionary<Axis4D, float>();
        //rotation.Add(Axis4D.xy, 0f);
        //rotation.Add(Axis4D.xz, 0f);
        //rotation.Add(Axis4D.xw, 0f);
        //rotation.Add(Axis4D.yz, 0f);
        //rotation.Add(Axis4D.yw, 0f);
        //rotation.Add(Axis4D.zw, 0f);

        originalVerts = new List<Vector4>(){
            new Vector4(1,1,1,1),
            new Vector4(1,1,1,-1),
            new Vector4(1,1,-1,1),
            new Vector4(1,1,-1,-1),
            new Vector4(1,-1,1,1),
            new Vector4(1,-1,1,-1),
            new Vector4(1,-1,-1,1),
            new Vector4(1,-1,-1,-1),
            new Vector4(-1,1,1,1),
            new Vector4(-1,1,1,-1),
            new Vector4(-1,1,-1,1),
            new Vector4(-1,1,-1,-1),
            new Vector4(-1,-1,1,1),
            new Vector4(-1,-1,1,-1),
            new Vector4(-1,-1,-1,1),
            new Vector4(-1,-1,-1,-1)
        };

        int i = 0;
        cubes = new CubeParams[8];
        cubes[i] = new CubeParams("red", cubeColors[i++], new int[] { 0, 1, 3, 6, 9, 15 });
        cubes[i] = new CubeParams("green", cubeColors[i++], new int[] { 2, 4, 7, 13, 19, 24 });
        cubes[i] = new CubeParams("blue", cubeColors[i++], new int[] { 28, 5, 8, 10, 20 });
        cubes[i] = new CubeParams("pink", cubeColors[i++], new int[] { 33, 34, 11, 12, 17, 22 });
        cubes[i] = new CubeParams("yellow", cubeColors[i++], new int[] { 14, 16, 37, 38, 39, 41 });
        cubes[i] = new CubeParams("purple", cubeColors[i++], new int[] { 18, 21, 43, 44, 46 });
        cubes[i] = new CubeParams("cyan", cubeColors[i++], new int[] { });
        cubes[i] = new CubeParams("brown", cubeColors[i++], new int[] { });

        i = 0;
        rotation = new RotationParams[6];
        rotation[i++] = new RotationParams(Axis4D.xy, 0f);
        rotation[i++] = new RotationParams(Axis4D.xz, 0f);
        rotation[i++] = new RotationParams(Axis4D.xw, 0f);
        rotation[i++] = new RotationParams(Axis4D.yz, 0f);
        rotation[i++] = new RotationParams(Axis4D.yw, 0f);
        rotation[i++] = new RotationParams(Axis4D.zw, 0f);

        ResetVertices();
        SetupLineRenderer();
        SetupText();
    }

    void ResetVertices()
    {
        rotatedVerts = new List<Vector4>();
        rotatedVerts.AddRange(originalVerts);
    }

    void SetupLineRenderer()
    {
        lineRedererRoot = new GameObject();
        lineRedererRoot.transform.parent = transform;
        lineRedererRoot.name = "line renderers";
        for (int i = 0; i < faces.Length; i++)
        {
            GameObject gO = new GameObject();
            gO.name = "face " + faces[i].faceGroup + " " + i;
            LineRenderer lineRenderer = gO.AddComponent<LineRenderer>();
            lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
            lineRenderer.widthMultiplier = 0.01f;
            lineRenderer.numCapVertices = 5;
            lineRenderer.numCornerVertices = 5;
            lineRenderer.numPositions = 5;
            lineRenderer.numCapVertices = 5;
            lineRenderer.numCornerVertices = 0;
            lineRenderer.useWorldSpace = false;
            gO.transform.parent = lineRedererRoot.transform;
            lineRenderer.colorGradient = CreateGradient(lineColor, lineColor);
        }
    }

    private Gradient CreateGradient(Color startColor, Color endColor)
    {
        // A simple 2 color gradient with a fixed alpha of 1.0f.
        float alpha = 1.0f;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(startColor, 0.0f), new GradientColorKey(endColor, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
        );
        return gradient;
    }

    void SetupText()
    {
        if (textPrefab != null)
        {
            textRoot = new GameObject();
            textRoot.transform.parent = transform;
            textRoot.name = "text root";
            for (int i = 0; i < rotatedVerts.Count; i++)
            {
                TextMesh t = Instantiate(textPrefab);
                t.text = "" + i;
                t.transform.parent = textRoot.transform;
                t.name = "vertex " + i;
            }
        }
    }

    void UpdateText()
    {
        for (int i = 0; i < rotatedVerts.Count; i++)
        {
            GameObject o = textRoot.transform.GetChild(i).gameObject;
            o.transform.position = rotatedVerts[i];
            //o.transform.rotation = Quaternion.LookRotation(Camera.main.transform.position - o.transform.position);
        }
    }

    void Update()
    {
        DrawTesseract();
        if (textPrefab != null)
        {
            UpdateText();
        }
    }


    void DrawTesseract()
    {
        for (int i = 0; i < faces.Length; i++)
        {

            FaceParams p = faces[i];
            DrawFace(rotatedVerts[p.a], rotatedVerts[p.b], rotatedVerts[p.c], rotatedVerts[p.d], i, p.faceColor, p.faceGroup);
        }
        for (int i = 0; i < cubes.Length; i++)
        {
            if(cubes[i].drawCube)
            {
                DrawCube(cubes[i]);
            }
        }
    }

    void DrawCube(CubeParams cube)
    {
        for (int i = 0; i < cube.faceIndices.Length; i++)
        {
            int faceIndex = cube.faceIndices[i];
            FaceParams p = faces[faceIndex];
            DrawFace(rotatedVerts[p.a], rotatedVerts[p.b], rotatedVerts[p.c], rotatedVerts[p.d], faceIndex, cube.cubeColor, cube.tag);
        }
    }

    void DrawFace(Vector3 a, Vector3 b, Vector3 c, Vector3 d, int faceIndex, Color color, string name)
    {
        Vector3 center = 0.25f * (a + b + c + d);
        LineRenderer r = lineRedererRoot.transform.GetChild(faceIndex).GetComponent<LineRenderer>();
        r.colorGradient = CreateGradient(color, color);
        r.SetPosition(0, a + center * pushOutAmount);
        r.SetPosition(1, b + center * pushOutAmount);
        r.SetPosition(2, c + center * pushOutAmount);
        r.SetPosition(3, d + center * pushOutAmount);
        r.SetPosition(4, a + center * pushOutAmount);
        r.gameObject.name = "cube " + name;

    }



    public void Rotate(Axis4D axis, float theta)
    {
        AddToRotationDictionary(axis, theta);
        ApplyRotationToVerts();
    }

    void AddToRotationDictionary(Axis4D axis, float theta)
    {
        //rotation[axis] = (rotation[axis] + theta);

    }

    public void ApplyRotationToVerts()
    {
        ResetVertices();

        foreach(RotationParams rp in rotation)
        {
            var axis = rp.axis;
            var amount = rp.amount;
            float s = Mathf.Sin(Mathf.Deg2Rad * amount);
            float c = Mathf.Cos(Mathf.Deg2Rad * amount);
            for (int i = 0; i < rotatedVerts.Count; i++)
            {
                rotatedVerts[i] = TesseractUtils.GetRotatedVertex(axis, rotatedVerts[i], s, c);
            }
        }
        //foreach (Axis4D axis in rotationOrder)
        //{
        //    float s = Mathf.Sin(Mathf.Deg2Rad * rotation[axis]);
        //    float c = Mathf.Cos(Mathf.Deg2Rad * rotation[axis]);
        //    for (int i = 0; i < rotatedVerts.Count; i++)
        //    {
        //        rotatedVerts[i] = TesseractUtils.GetRotatedVertex(axis, rotatedVerts[i], s, c);
        //    }
        //}
    }


    private FaceParams[] faces =
    {
        new FaceParams(0, 1, 5, 4),
        new FaceParams(0, 2, 6, 4),
        new FaceParams(0, 8, 12, 4),
        new FaceParams(0, 2, 3, 1),
        new FaceParams(0, 1, 9, 8),
        new FaceParams(0, 2, 10, 8),
        new FaceParams(1, 3, 7, 5),
        new FaceParams(1, 9, 13, 5),
        new FaceParams(1, 3, 11, 9),
        new FaceParams(2, 3, 7, 6),
        new FaceParams(2, 3, 11, 10),
        new FaceParams(2, 10, 14, 6),
        new FaceParams(3, 11, 15, 7),
        new FaceParams(4, 12, 13, 5),
        new FaceParams(4, 6, 14, 12),
        new FaceParams(4, 6, 7, 5),
        new FaceParams(5, 7, 15, 13),
        new FaceParams(6, 7, 15, 14),
        new FaceParams(8, 10, 14, 12),
        new FaceParams(8, 9, 13, 12),
        new FaceParams(8, 9, 11, 10),
        new FaceParams(9, 11, 15, 13),
        new FaceParams(10, 11, 15, 14),
        new FaceParams(0, 1, 3, 2),
        new FaceParams(0, 1, 5, 4),
        new FaceParams(0, 2, 6, 4),
        new FaceParams(0, 8, 12, 4),
        new FaceParams(0, 2, 3, 1),
        new FaceParams(0, 1, 9, 8),
        new FaceParams(0, 2, 10, 8),
        new FaceParams(1, 3, 7, 5),
        new FaceParams(1, 9, 13, 5),
        new FaceParams(1, 3, 11, 9),
        new FaceParams(2, 3, 7, 6),
        new FaceParams(2, 3, 11, 10),
        new FaceParams(2, 10, 14, 6),
        new FaceParams(3, 11, 15, 7),
        new FaceParams(4, 12, 13, 5),
        new FaceParams(4, 6, 14, 12),
        new FaceParams(4, 6, 7, 5),
        new FaceParams(5, 7, 15, 13),
        new FaceParams(6, 7, 15, 14),
        new FaceParams(8, 10, 14, 12),
        new FaceParams(8, 9, 13, 12),
        new FaceParams(8, 9, 11, 10),
        new FaceParams(9, 11, 15, 13),
        new FaceParams(10, 11, 15, 14),
        new FaceParams(0, 1, 3, 2)
    };
}
