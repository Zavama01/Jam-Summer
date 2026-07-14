using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class PlayerCollisionCheck : MonoBehaviour
{
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private float skinWidth = 0.02f;

    private BoxCollider2D boxCol;

    private void Awake()
    {
        boxCol = GetComponent<BoxCollider2D>();
    }

    public Vector2 GetAllowedMovement(Vector2 desiredMovement)
    {
        Vector2 allowed = desiredMovement;

        allowed.x = GetAllowedDistance(Vector2.right * Mathf.Sign(desiredMovement.x), Mathf.Abs(desiredMovement.x));
        transform.position += new Vector3(allowed.x, 0f, 0f);

        allowed.y = GetAllowedDistance(Vector2.up * Mathf.Sign(desiredMovement.y), Mathf.Abs(desiredMovement.y));
        transform.position += new Vector3(0f, allowed.y, 0f);

        transform.position -= new Vector3(allowed.x, allowed.y, 0f);

        return allowed;
    }

    private float GetAllowedDistance(Vector2 direction, float distance)
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
            distance + skinWidth,
            obstacleMask
        );

        if (hit.collider == null)
            return distance;

        float allowedDistance = hit.distance - skinWidth;
        return Mathf.Max(allowedDistance, 0f);
    }
}