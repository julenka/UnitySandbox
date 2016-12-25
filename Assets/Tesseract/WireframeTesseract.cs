using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class WireframeTesseract : MonoBehaviour
{
    public List<Vector4> originalVerts;
    public List<Vector4> rotatedVerts;

    public List<Axis4D> rotationOrder;
    public Dictionary<Axis4D, float> rotation;

	GameObject textRoot;
	public TextMesh textPrefab;

	// Line Renderer
	public Color startColor, endColor;
	float widthMultiplier = 0.02f;
	GameObject lineRedererRoot;

	// Drawing faces
	int planeIndex = 0;
	int numFaces = 23;

	bool freezeRotation = false;

    void Start()
    {
        rotationOrder = new List<Axis4D>();
        rotationOrder.Add(Axis4D.yz);
        rotationOrder.Add(Axis4D.xw);
        rotationOrder.Add(Axis4D.yw);
        rotationOrder.Add(Axis4D.zw);
        rotationOrder.Add(Axis4D.xy);
        rotationOrder.Add(Axis4D.xz);

        rotation = new Dictionary<Axis4D, float>();
        rotation.Add(Axis4D.xy, 0f);
        rotation.Add(Axis4D.xz, 0f);
        rotation.Add(Axis4D.xw, 0f);
        rotation.Add(Axis4D.yz, 0f);
        rotation.Add(Axis4D.yw, 0f);
        rotation.Add(Axis4D.zw, 0f);

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

        ResetVertices();
		SetupLineRenderer ();
		SetupText ();
    }

	void ResetVertices()
	{
		rotatedVerts = new List<Vector4>();
		rotatedVerts.AddRange(originalVerts);
	}

	void SetupLineRenderer() {
		lineRedererRoot = new GameObject ();
		lineRedererRoot.transform.parent = transform;
		lineRedererRoot.name = "line renderers";
		for (int i = 0; i < numFaces; i++) {
			GameObject gO = new GameObject();
			LineRenderer lineRenderer = gO.AddComponent<LineRenderer> ();
			lineRenderer.material = new Material (Shader.Find("Particles/Additive"));
			lineRenderer.widthMultiplier = widthMultiplier;
			lineRenderer.numPositions = 5;
			lineRenderer.numCapVertices = 0;
			lineRenderer.numCornerVertices = 0;
			gO.transform.parent = lineRedererRoot.transform;
			
			// A simple 2 color gradient with a fixed alpha of 1.0f.
			float alpha = 1.0f;
			Gradient gradient = new Gradient();
			gradient.SetKeys(
				new GradientColorKey[] { new GradientColorKey(startColor, 0.0f), new GradientColorKey(endColor, 1.0f) },
				new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
			);
			lineRenderer.colorGradient = gradient;
		}
	}

	void SetupText() 
	{
		textRoot = new GameObject ();
		textRoot.transform.parent = transform;
		textRoot.name = "text root";
		for (int i = 0; i < rotatedVerts.Count; i++) {
			TextMesh t = Instantiate (textPrefab);
			t.text = "" + i;
			t.transform.parent = textRoot.transform;
			t.name = "vertex " + i;
		}
	}

    void Update()
    {
        DrawTesseract();
    }


    void OnGUI()
    {

        freezeRotation = GUI.Toggle(new Rect(25, 15, 200, 30), freezeRotation, "Freeze Rotation");

        GUI.Label(new Rect(25, 25, 100, 30), "XY");
        rotation[Axis4D.xy] = GUI.HorizontalSlider(new Rect(25, 50, 100, 30), Mathf.Repeat(rotation[Axis4D.xy], 360f), 0.0F, 360.0F);
        GUI.Label(new Rect(25, 75, 100, 30), "XZ");
        rotation[Axis4D.xz] = GUI.HorizontalSlider(new Rect(25, 100, 100, 30), Mathf.Repeat(rotation[Axis4D.xz], 360f), 0.0F, 360.0F);
        GUI.Label(new Rect(25, 125, 100, 30), "XW");
        rotation[Axis4D.xw] = GUI.HorizontalSlider(new Rect(25, 150, 100, 30), Mathf.Repeat(rotation[Axis4D.xw], 360f), 0.0F, 360.0F);
        GUI.Label(new Rect(25, 175, 100, 30), "YZ");
        rotation[Axis4D.yz] = GUI.HorizontalSlider(new Rect(25, 200, 100, 30), Mathf.Repeat(rotation[Axis4D.yz], 360f), 0.0F, 360.0F);
        GUI.Label(new Rect(25, 225, 100, 30), "YW");
        rotation[Axis4D.yw] = GUI.HorizontalSlider(new Rect(25, 250, 100, 30), Mathf.Repeat(rotation[Axis4D.yw], 360f), 0.0F, 360.0F);
        GUI.Label(new Rect(25, 275, 100, 30), "ZW");
        rotation[Axis4D.zw] = GUI.HorizontalSlider(new Rect(25, 300, 100, 30), Mathf.Repeat(rotation[Axis4D.zw], 360f), 0.0F, 360.0F);

        GUI.Label(new Rect(25, 325, 200, 30), "Zoom with the Mouse Wheel");
        Camera.main.orthographic = GUI.Toggle(new Rect(25, 375, 200, 30), Camera.main.orthographic, "Orth Camera");

        if (!freezeRotation)
        {
            Rotate(Axis4D.xy, 0.1f);
            Rotate(Axis4D.xz, 0.15f);
            Rotate(Axis4D.xw, 0.6f);
            Rotate(Axis4D.yw, 0.3f);
            Rotate(Axis4D.yz, 0.45f);
            Rotate(Axis4D.zw, 0.5f);
        }

        ApplyRotationToVerts();
    }

    void DrawTesseract()
    {
		planeIndex = 0;
		for (int i = 0; i < rotatedVerts.Count; i++) 
		{
			GameObject o = textRoot.transform.GetChild (i).gameObject;
			o.transform.position = rotatedVerts [i];
			o.transform.rotation = Quaternion.LookRotation (Camera.main.transform.position - o.transform.position);
		}

		CreatePlane(rotatedVerts[0], rotatedVerts[1], rotatedVerts[5], rotatedVerts[4]);
		CreatePlane(rotatedVerts[0], rotatedVerts[2], rotatedVerts[6], rotatedVerts[4]);
		CreatePlane(rotatedVerts[0], rotatedVerts[8], rotatedVerts[12], rotatedVerts[4]);
		CreatePlane(rotatedVerts[0], rotatedVerts[2], rotatedVerts[3], rotatedVerts[1]);
		CreatePlane(rotatedVerts[0], rotatedVerts[1], rotatedVerts[9], rotatedVerts[8]);
		CreatePlane(rotatedVerts[0], rotatedVerts[2], rotatedVerts[10], rotatedVerts[8]);

		CreatePlane(rotatedVerts[1], rotatedVerts[3], rotatedVerts[7], rotatedVerts[5]);
		CreatePlane(rotatedVerts[1], rotatedVerts[9], rotatedVerts[13], rotatedVerts[5]);
		CreatePlane(rotatedVerts[1], rotatedVerts[3], rotatedVerts[11], rotatedVerts[9]);

		CreatePlane(rotatedVerts[2], rotatedVerts[3], rotatedVerts[7], rotatedVerts[6]);
		CreatePlane(rotatedVerts[2], rotatedVerts[3], rotatedVerts[11], rotatedVerts[10]);
		CreatePlane(rotatedVerts[2], rotatedVerts[10], rotatedVerts[14], rotatedVerts[6]);

		CreatePlane(rotatedVerts[3], rotatedVerts[11], rotatedVerts[15], rotatedVerts[7]);

		CreatePlane(rotatedVerts[4], rotatedVerts[12], rotatedVerts[13], rotatedVerts[5]);
		CreatePlane(rotatedVerts[4], rotatedVerts[6], rotatedVerts[14], rotatedVerts[12]);
		CreatePlane(rotatedVerts[4], rotatedVerts[6], rotatedVerts[7], rotatedVerts[5]);

		CreatePlane(rotatedVerts[5], rotatedVerts[7], rotatedVerts[15], rotatedVerts[13]);

		CreatePlane(rotatedVerts[6], rotatedVerts[7], rotatedVerts[15], rotatedVerts[14]);

		CreatePlane(rotatedVerts[8], rotatedVerts[10], rotatedVerts[14], rotatedVerts[12]);
		CreatePlane(rotatedVerts[8], rotatedVerts[9], rotatedVerts[13], rotatedVerts[12]);
		CreatePlane(rotatedVerts[8], rotatedVerts[9], rotatedVerts[11], rotatedVerts[10]);

		CreatePlane(rotatedVerts[9], rotatedVerts[11], rotatedVerts[15], rotatedVerts[13]);

		CreatePlane(rotatedVerts[10], rotatedVerts[11], rotatedVerts[15], rotatedVerts[14]);
    }

	void CreatePlane(Vector3 a, Vector3 b, Vector3 c, Vector3 d ) 
	{
		LineRenderer r = lineRedererRoot.transform.GetChild (planeIndex++).GetComponent<LineRenderer> ();
		r.SetPosition (0, a);
		r.SetPosition (1, b);
		r.SetPosition (2, c);
		r.SetPosition (3, d);
		r.SetPosition (4, a);
	}



    void Rotate(Axis4D axis, float theta)
    {
        AddToRotationDictionary(axis, theta);
        ApplyRotationToVerts();
    }

    void AddToRotationDictionary(Axis4D axis, float theta)
    {
        rotation[axis] = (rotation[axis] + theta);

    }

    void ApplyRotationToVerts()
    {
        ResetVertices();

        foreach (Axis4D axis in rotationOrder)
        {
            float s = Mathf.Sin(Mathf.Deg2Rad * rotation[axis]);
            float c = Mathf.Cos(Mathf.Deg2Rad * rotation[axis]);
            for (int i = 0; i < rotatedVerts.Count; i++)
            {
                rotatedVerts[i] = TesseractUtils.GetRotatedVertex(axis, rotatedVerts[i], s, c);
            }
        }
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        for (int i = 0; i < rotatedVerts.Count; i++)
        {
            Gizmos.DrawSphere(rotatedVerts[i], 0.1f);
        }
    }
}
