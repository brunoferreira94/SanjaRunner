using UnityEngine;

public static class Collision2DExtension
{
    public static Vector2 Normal(this Collision2D coll)
    {
        return coll.contacts[0].normal;
    }

    public static Vector2 Point(this Collision2D coll)
    {
        return coll.contacts[0].point;
    }

    public static Vector2 Tangent(this Collision2D coll)
    {
        return Vector3.Cross(coll.Normal(), Vector3.forward);
    }

    public static bool MatchTag(this Collision2D coll, string tag)
    {
        return coll.collider.CompareTag(tag);
    }
}