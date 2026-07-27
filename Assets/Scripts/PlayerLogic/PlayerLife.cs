using UnityEngine;

public class PlayerLife : MonoBehaviour
{

    [SerializeField] private int maxHealth = 3;
    private int currentHealth;
    
    void Awake()
    {
        currentHealth = maxHealth;
    }

    private void OnTriggerEnter(Collider other)
    {
        // se vieni colpito chiama TakeDamage e Lancia evento per UI
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }

    private void TakeDamage(int damage)
    {
        currentHealth -= damage;

        // evento per aggiornare la UI

        if (currentHealth < 0) {

            // evento Gameover

        }
    }
}
