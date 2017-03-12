using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(ParticleSystem))]
public class ParticlesTesseract : MonoBehaviour
{
    ParticleSystem.Particle[] particles;
    ParticleSystem particleSystem;

    public List<Vector4> originalVerts;
    public List<Vector4> rotatedVerts;

    public List<Vector3> originalTris;

    public List<Vector3> verts;
    public List<int> tris;
    public List<Vector2> uvs;

    public List<Axis4D> rotationOrder;
    public Dictionary<Axis4D, float> rotation;

    void InitializeTesseract()
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
    }

    void InitializeParticles()
    {
        particleSystem = GetComponent<ParticleSystem>();
        if(verts == null)
        {
            InitializeTesseract();
        }
        particles = new ParticleSystem.Particle[rotatedVerts.Count];
        for (int i = 0; i < rotatedVerts.Count; i++)
        {
            particles[i] = new ParticleSystem.Particle();
        }
    }

    void UpdateParticles()
    {
        if (rotatedVerts == null)
        {
            InitializeTesseract();
            InitializeParticles();
        }
        for (int i = 0; i < rotatedVerts.Count; i++)
        {
            //particles[i].position = rotatedVerts[i];
            //particles[i].color = Color.white;
            //particles[i].size = 1f;
            ParticleSystem.EmitParams p = new ParticleSystem.EmitParams();
            p.position = rotatedVerts[i];
            p.startSize = 1f;
            p.startLifetime = 0.5f;
            particleSystem.Emit(p, 1);

        }
        //GetComponent<ParticleSystem>().SetParticles(particles, particles.Length);
    }

    void Awake()
    {
        if (particles == null)
        {
            InitializeTesseract();
            InitializeParticles();
        }
    }

    void Update()
    {
        DrawTesseract();
    }

    bool freezeRotation = false;
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
        UpdateParticles();
    }

    Vector4 GetRotatedVertex(Axis4D axis, Vector4 v, float s, float c)
    {
        switch (axis)
        {
            case Axis4D.xy:
                return TesseractUtils.RotateAroundXY(v, s, c);
            case Axis4D.xz:
                return TesseractUtils.RotateAroundXZ(v, s, c);
            case Axis4D.xw:
                return TesseractUtils.RotateAroundXW(v, s, c);
            case Axis4D.yz:
                return TesseractUtils.RotateAroundYZ(v, s, c);
            case Axis4D.yw:
                return TesseractUtils.RotateAroundYW(v, s, c);
            case Axis4D.zw:
                return TesseractUtils.RotateAroundZW(v, s, c);
        }

        return new Vector4(0, 0, 0, 0);
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
                rotatedVerts[i] = GetRotatedVertex(axis, rotatedVerts[i], s, c);
            }
        }
    }

    void ResetVertices()
    {
        rotatedVerts = new List<Vector4>();
        rotatedVerts.AddRange(originalVerts);
    }

}

