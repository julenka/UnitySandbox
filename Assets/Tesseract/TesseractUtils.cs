﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public enum Axis4D
{
    xy,
    xz,
    xw,
    yz,
    yw,
    zw,
}
class TesseractUtils
{
    public static Vector4 RotateAroundXY(Vector4 v, float s, float c)
    {
        float tmpX = c * v.x + s * v.y;
        float tmpY = -s * v.x + c * v.y;
        return new Vector4(tmpX, tmpY, v.z, v.w);
    }

    public static Vector4 RotateAroundXZ(Vector4 v, float s, float c)
    {
        float tmpX = c * v.x + s * v.z;
        float tmpZ = -s * v.x + c * v.z;
        return new Vector4(tmpX, v.y, tmpZ, v.w);
    }

    public static Vector4 RotateAroundXW(Vector4 v, float s, float c)
    {
        float tmpX = c * v.x + s * v.w;
        float tmpW = -s * v.x + c * v.w;
        return new Vector4(tmpX, v.y, v.z, tmpW);
    }

    public static Vector4 RotateAroundYZ(Vector4 v, float s, float c)
    {
        float tmpY = c * v.y + s * v.z;
        float tmpZ = -s * v.y + c * v.z;
        return new Vector4(v.x, tmpY, tmpZ, v.w);
    }

    public static Vector4 RotateAroundYW(Vector4 v, float s, float c)
    {
        float tmpY = c * v.y - s * v.w;
        float tmpW = s * v.y + c * v.w;
        return new Vector4(v.x, tmpY, v.z, tmpW);
    }

    public static Vector4 RotateAroundZW(Vector4 v, float s, float c)
    {
        float tmpZ = c * v.z - s * v.w;
        float tmpW = s * v.z + c * v.w;
        return new Vector4(v.x, v.y, tmpZ, tmpW);
    }

}
