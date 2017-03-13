using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Analog.Platform.ShellPrototyping.Common;
public class TesseractVoiceControl : MonoBehaviour
{
    public WireframeTesseract tesseract;

    public void Start()
    {
        VoiceController.Instance.AddKeyword("pause", KeyCode.Keypad0, 
            () => tesseract.freezeRotation = true);
        VoiceController.Instance.AddKeyword("play", KeyCode.Keypad1,
            () => tesseract.freezeRotation = false);
        VoiceController.Instance.AddKeyword("bigger", KeyCode.Keypad2,
            () => tesseract.transform.localScale *= 1.25f);
        VoiceController.Instance.AddKeyword("smaller", KeyCode.Keypad3,
            () => tesseract.transform.localScale *= 0.75f);
    }

    void Update()
    {
        if (!tesseract.freezeRotation)
        {
            tesseract.Rotate(Axis4D.xy, 0.1f);
            tesseract.Rotate(Axis4D.xz, 0.15f);
            tesseract.Rotate(Axis4D.xw, 0.6f);
            tesseract.Rotate(Axis4D.yw, 0.3f);
            tesseract.Rotate(Axis4D.yz, 0.45f);
            tesseract.Rotate(Axis4D.zw, 0.5f);
        }

    }
}
