using UnityEngine;

public class PlayerLife : MonoBehaviour, IDamageable
{
    [SerializeField] private LayerMask projectileMask;
    [SerializeField] private Vector2 hitboxSize = Vector2.one;
    [SerializeField] private int maxHealth = 3;
    private int currentHealth;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    private void OnEnable()
    {
        
        EventBus<EmptyPayload>.Subscribe("OnPlayerHit", OnPlayerHit);

    }

    private void OnDisable()
    {
        
        EventBus<EmptyPayload>.Unsubscribe("OnPlayerHit", OnPlayerHit);
    }

    private void Update()
    {

        CheckProjectileHit();
    }

    private void OnPlayerHit(IEventPayload _)
    {
        TakeDamage();
    }

    private void CheckProjectileHit()
    {
        Vector2 center = (Vector2)transform.position;

        Collider2D hit = Physics2D.OverlapBox(
            center,
            hitboxSize,
            0f,
            projectileMask
        );

        if (hit == null)
            return;

        if (hit.TryGetComponent(out Projectile projectile))
        {
            TakeDamage(projectile.GetDamage());

        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Vector3 center = transform.position ;
        Gizmos.DrawWireCube(center, hitboxSize);
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("danno al player");

        currentHealth -= damage;

        // evento per aggiornare la UI

        if (currentHealth < 0)
        {

            // evento Gameover

        }

        EventBus<HeartsPayload>.Raise("OnHeartUpdate", new HeartsPayload { hearts_number = currentHealth });
    }

}
