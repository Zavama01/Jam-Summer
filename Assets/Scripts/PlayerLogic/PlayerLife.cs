using UnityEngine;

public class PlayerLife : MonoBehaviour, IDamageable
{

    [SerializeField] private int maxHealth = 3;
    private int currentHealth;

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        // evento per aggiornare la UI

        if (currentHealth < 0)
        {

            // evento Gameover

        }

        EventBus<HeartsPayload>.Raise("OnHeartUpdate", new HeartsPayload { hearts_number = currentHealth });
    }

    void Awake()
    {
        currentHealth = maxHealth;
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
