using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MyMathUtils
{
    public static float Remap(float dataValue, float from0, float to0, float from1, float to1)
    {
        return from1 + (dataValue - from0) * (to1 - from1) / (to0 - from0);
    }

    public static float Percent(float value, float min, float max)
    {
        return (value - min) / (max - min);
    }

    public static float ValueFromPercent(float percent, float min, float max)
    {
        return percent * (max - min) + min;
    }

    public static bool IsWholeNumber(float value, float tolerance = 0.001f)
    {
        return (value % 1) < tolerance;
    }

    // https://math.stackexchange.com/questions/1905533/find-perpendicular-distance-from-point-to-line-in-3d
    public static float DistanceFromPointToLine(Vector3 point, Vector3 lineStart, Vector3 lineEnd)
    {
        Vector3 lineStartToEnd = lineEnd - lineStart;
        return Vector3.Cross(point - lineStart, lineStartToEnd).magnitude / lineStartToEnd.magnitude;
    }
    public static float DistanceSquaredFromPointToLine(Vector3 point, Vector3 lineStart, Vector3 lineEnd)
    {
        Vector3 lineStartToEnd = lineEnd - lineStart;
        return Vector3.Cross(point - lineStart, lineStartToEnd).sqrMagnitude / lineStartToEnd.sqrMagnitude;
    }

    // https://math.stackexchange.com/questions/2213165/find-shortest-distance-between-lines-in-3d
    public static float DistanceSquaredFromLineToLine(Vector3 lineAPoint, Vector3 lineADirection, Vector3 lineBPoint, Vector3 lineBDirection)
    {
        Vector3 n = Vector3.Cross(lineADirection, lineBDirection);

        // if n.magnitude = 0, then the lines are parallel

        float dotProd = Vector3.Dot(n, lineAPoint - lineBPoint);
        return (dotProd * dotProd) / n.sqrMagnitude;
    }

}
