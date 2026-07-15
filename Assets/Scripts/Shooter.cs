using UnityEngine;

public class Shooter : MonoBehaviour
{
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private Vector2 shootDirection = Vector2.right;
    [SerializeField] private float shootInterval = 2f;
    [SerializeField] private float projectileSpeed = 8f;

    private float timer;

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
            firePoint.position,
            Quaternion.identity
        );

        projectile.Initialize(shootDirection.normalized, projectileSpeed);
    }
}