using UnityEngine;

public class Shooter : MonoBehaviour, IDamageable, ISeflDestroy
{
    [SerializeField] private Projectile projectilePrefab;

    [SerializeField] Vector3 spawnOffset = new Vector3(0.5f, 0f, 0f);
    //[SerializeField] private Transform firePoint;
    [SerializeField] private Vector2 shootDirection = Vector2.right;
    [SerializeField] private float shootInterval = 2f;
    [SerializeField] private float projectileSpeed = 8f;

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

    }

    private void Shoot()
    {
        Projectile projectile = Instantiate(
            projectilePrefab,
            transform.position + spawnOffset,
            Quaternion.identity
        );

        projectile.Initialize(shootDirection.normalized, projectileSpeed);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth < 0)
        {
            //evento per farne spawnare uno nuovo
            EventBus<EmptyPayload>.Raise("OnSelfDestruct", new EmptyPayload{});
            SelfDestroy();

        }


    }

    public void SelfDestroy()
    {

        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Projectile projectile = other.GetComponent<Projectile>();

        if (projectile != null)
        {
            TakeDamage(projectile.GetDamage());

        }
    }

}