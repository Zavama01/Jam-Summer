using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class PlayerBoxCastCollision : MonoBehaviour
{
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private float skin = 0.02f;

    private BoxCollider2D boxCol;

    private void Awake()
    {
        boxCol = GetComponent<BoxCollider2D>();
    }

    public Vector3 FilterMovement(Vector3 delta)
    {
        Vector3 filtered = delta;

        filtered.x = FilterAxis(Vector2.right * Mathf.Sign(delta.x), Mathf.Abs(delta.x));
        filtered.y = FilterAxis(Vector2.up * Mathf.Sign(delta.y), Mathf.Abs(delta.y));

        return filtered;
    }

    private float FilterAxis(Vector2 direction, float distance)
    {
        if (distance <= 0f)
            return 0f;

        Vector2 origin = boxCol.bounds.center;
        Vector2 size = boxCol.bounds.size;

        RaycastHit2D hit = Physics2D.BoxCast(
            origin,
            size,
            0f,
            direction,
            distance + skin,
            obstacleMask
        );

        if (hit.collider == null)
            return distance;

        float allowedDistance = hit.distance - skin;
        return Mathf.Max(allowedDistance, 0f);
    }
}