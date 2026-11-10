using UnityEngine;

public class Shooter : MonoBehaviour, IDamageable, ISeflDestroy
{
    [SerializeField] private Projectile projectilePrefab;

    [Header("Shooting")]
    [SerializeField] private Vector2 shootDirection = Vector2.right;
    [SerializeField] private float shootInterval = 2f;
    [SerializeField] private float projectileSpeed = 8f;

    [Header("Damage Detection")]
    [SerializeField] private Vector2 hitboxOffset = Vector2.zero;
    [SerializeField] private Vector2 hitboxSize = Vector2.one;
    [SerializeField] private LayerMask projectileMask;

    [Header("Health")]
    [SerializeField] private int maxLife;

    private int currentHealth;
    private float timer;

    private void Start()
    {
        currentHealth = maxLife;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= shootInterval)
        {
            timer = 0f;
            Shoot();
        }

        CheckProjectileHit();
    }

    private void Shoot()
    {
        Vector3 spawnOffset = transform.right * 1f;

        Projectile projectile = Instantiate(
            projectilePrefab,
            transform.position + spawnOffset,
            Quaternion.identity
        );

        projectile.Initialize(shootDirection.normalized, projectileSpeed);
    }

    private void CheckProjectileHit()
    {
        Vector2 center = (Vector2)transform.position + hitboxOffset;

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

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        Debug.Log($"Danno ricevuto: {damage}. Vita rimasta: {currentHealth}");

        if (currentHealth <= 0)
        {
            EventBus<EmptyPayload>.Raise(
                "OnSelfDestruct",
                new EmptyPayload()
            );


            SelfDestroy();
        }
    }

    public void SelfDestroy()
    {
        gameObject.SetActive(false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Vector3 center = transform.position + (Vector3)hitboxOffset;
        Gizmos.DrawWireCube(center, hitboxSize);
    }
}