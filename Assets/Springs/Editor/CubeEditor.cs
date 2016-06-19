using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(RippleGrid))]
public class CubeEditor : Editor
{
    private RippleGrid grid;
    private void OnSceneGUI()
    {
        grid = target as RippleGrid;

    }
}
