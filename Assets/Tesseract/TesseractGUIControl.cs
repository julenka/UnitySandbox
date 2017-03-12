using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TesseractGUIControl : MonoBehaviour
{
    public WireframeTesseract tesseract;


    void OnGUI()
    {

        tesseract.freezeRotation = GUI.Toggle(new Rect(25, 15, 200, 30), tesseract.freezeRotation, "Freeze Rotation");

        GUI.Label(new Rect(25, 25, 100, 30), "XY");
        tesseract.rotation[Axis4D.xy] = GUI.HorizontalSlider(new Rect(25, 50, 100, 30), Mathf.Repeat(tesseract.rotation[Axis4D.xy], 360f), 0.0F, 360.0F);
        GUI.Label(new Rect(25, 75, 100, 30), "XZ");
        tesseract.rotation[Axis4D.xz] = GUI.HorizontalSlider(new Rect(25, 100, 100, 30), Mathf.Repeat(tesseract.rotation[Axis4D.xz], 360f), 0.0F, 360.0F);
        GUI.Label(new Rect(25, 125, 100, 30), "XW");
        tesseract.rotation[Axis4D.xw] = GUI.HorizontalSlider(new Rect(25, 150, 100, 30), Mathf.Repeat(tesseract.rotation[Axis4D.xw], 360f), 0.0F, 360.0F);
        GUI.Label(new Rect(25, 175, 100, 30), "YZ");
        tesseract.rotation[Axis4D.yz] = GUI.HorizontalSlider(new Rect(25, 200, 100, 30), Mathf.Repeat(tesseract.rotation[Axis4D.yz], 360f), 0.0F, 360.0F);
        GUI.Label(new Rect(25, 225, 100, 30), "YW");
        tesseract.rotation[Axis4D.yw] = GUI.HorizontalSlider(new Rect(25, 250, 100, 30), Mathf.Repeat(tesseract.rotation[Axis4D.yw], 360f), 0.0F, 360.0F);
        GUI.Label(new Rect(25, 275, 100, 30), "ZW");
        tesseract.rotation[Axis4D.zw] = GUI.HorizontalSlider(new Rect(25, 300, 100, 30), Mathf.Repeat(tesseract.rotation[Axis4D.zw], 360f), 0.0F, 360.0F);

        GUI.Label(new Rect(25, 325, 200, 30), "Zoom with the Mouse Wheel");
        Camera.main.orthographic = GUI.Toggle(new Rect(25, 375, 200, 30), Camera.main.orthographic, "Orth Camera");

        if (!tesseract.freezeRotation)
        {
            tesseract.Rotate(Axis4D.xy, 0.1f);
            tesseract.Rotate(Axis4D.xz, 0.15f);
            tesseract.Rotate(Axis4D.xw, 0.6f);
            tesseract.Rotate(Axis4D.yw, 0.3f);
            tesseract.Rotate(Axis4D.yz, 0.45f);
            tesseract.Rotate(Axis4D.zw, 0.5f);
        }

        tesseract.ApplyRotationToVerts();
    }

}
