using UnityEngine;

public class BoxAreaCollider : MonoBehaviour
{
    public Color GizmoColor = Color.red;

    public Vector2 Center;

    public Vector2 Size;

    public LayerMask LayerMask = Physics2D.AllLayers;

    private Vector2 WorldSize
    {
        get { return new Vector2(Size.x * transform.lossyScale.x, Size.y * transform.localScale.y); }
    }

    private Vector2 WorldCenter
    {
        get { return transform.TransformPoint(Center); }
    }

    private Vector2 HalfSize
    {
        get { return WorldSize * .5f; }
    }

    private Vector2 LowerLeftCorner
    {
        get { return WorldCenter - HalfSize; }
    }

    public Vector2 UpperRightCorner
    {
        get { return WorldCenter + HalfSize; }
    }

    public Collider2D Overlap()
    {
        return Physics2D.OverlapArea(LowerLeftCorner, UpperRightCorner, LayerMask);
    }

    public Collider2D[] OverlapAll()
    {
        return Physics2D.OverlapAreaAll(LowerLeftCorner, UpperRightCorner, LayerMask);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = GizmoColor;
        Gizmos.DrawWireCube(WorldCenter, WorldSize);
    }
}
