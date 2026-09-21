using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(BoxCollider2D))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private LayerMask hitMask;
    [SerializeField] private float maxLifetime = 5f;
    [SerializeField] private float skin = 0.01f;
    [SerializeField] private int damage;

    private Vector2 _direction;
    private float speed;
    private BoxCollider2D boxCol;
    private float lifeTimer;

   

    private void OnEnable()
    {
        EventBus<DirectionPayload>.Subscribe("OnDeflect", OnDeflect);
    }

    private void OnDisable()
    {
        EventBus<DirectionPayload>.Unsubscribe("OnDeflect", OnDeflect);
    }

    public void Initialize(Vector2 newDirection, float newSpeed)
    {
        _direction = newDirection.normalized;
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

        Vector2 movement = _direction * speed * Time.deltaTime;
        float distance = movement.magnitude;

        RaycastHit2D hit = Physics2D.BoxCast(
            boxCol.bounds.center,
            boxCol.bounds.size,
            0f,
            _direction,
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


    private void OnDeflect(IEventPayload payload)
    {
        _direction = ((DirectionPayload)payload).direction.normalized;
    }

    public int GetDamage()
    {
        return damage;
    }
}