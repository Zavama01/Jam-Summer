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
            if(hit.collider.CompareTag("Player"))
{
                Debug.Log("Colpito il player");
                EventBus<ProjectilePayload>.Raise("OnPlayerHit", new ProjectilePayload { projectileDamage = damage });

                //crea un ProjecftilePayload al posto di questo empty e mettici il danno dentro
                //poi modifica la classe playerlife e lascia solo la roba relativa ell'evento

            }
            gameObject.SetActive(false);
            return;
        }

        transform.position += (Vector3)movement;
    }


    public void Deflect(Vector2 newDirection)
    {
        _direction = newDirection.normalized;
        gameObject.layer = LayerMask.NameToLayer("Deflect");
    }

    public int GetDamage()
    {
        return damage;
    }




}