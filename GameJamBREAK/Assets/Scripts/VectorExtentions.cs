using UnityEngine;

public static class VectorExtentions
{
    public static Vector3 ToHorizontal(this Vector3 v)
    {
        return Vector3.ProjectOnPlane(v, Vector3.up);
    }

    public static float VerticalComponent(this Vector3 v)
    {
        return Vector3.Dot(v, Vector3.up);
    }


    public static Vector3 TransformDirectionHorizontal(this Transform t, Vector3 v)
    {
        return t.TransformDirection(v).ToHorizontal().normalized;
    }
}
