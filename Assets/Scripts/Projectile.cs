using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(BoxCollider2D))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private LayerMask hitMask;
    [SerializeField] private float maxLifetime = 5f;
    [SerializeField] private float skin = 0.01f;

    private Vector2 direction;
    private float speed;
    private BoxCollider2D boxCol;
    private float lifeTimer;

    private void OnEnable()
    {
        EventBus<ProjectilePayload>.Subscribe("OnDeflect", OnDeflect);
    }

    private void OnDisable()
    {
        EventBus<ProjectilePayload>.Unsubscribe("OnDeflect", OnDeflect);
    }

    public void Initialize(Vector2 newDirection, float newSpeed)
    {
        direction = newDirection.normalized;
        speed = newSpeed;
        lifeTimer = 0f;
    }

    private void Awake()
    {
        boxCol = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        lifeTimer += Time.deltaTime;

        if (lifeTimer >= maxLifetime)
        {
            Destroy(gameObject);
            return;
        }

        Vector2 movement = direction * speed * Time.deltaTime;
        float distance = movement.magnitude;

        RaycastHit2D hit = Physics2D.BoxCast(
            boxCol.bounds.center,
            boxCol.bounds.size,
            0f,
            direction,
            distance + skin,
            hitMask
        );

        if (hit.collider != null)
        {
            Destroy(gameObject);
            return;
        }

        transform.position += (Vector3)movement;
    }


    private void OnDeflect(Vector2 newDirection)
    {
        direction = newDirection.normalized;
    }
}