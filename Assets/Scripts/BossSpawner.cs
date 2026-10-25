using UnityEngine;

public class RandomSpawnPointSpawner : MonoBehaviour, ISeflDestroy
{
    [Header("Cosa spawnare")]
    [SerializeField] private GameObject prefabToSpawn;

    [Header("Impostazioni")]
    [Tooltip("Secondi tra uno spawn e l'altro.")]
    [SerializeField] private float spawnInterval = 3f;

    [Tooltip("Posizioni da cui può nascere l'oggetto.")]
    [SerializeField] private Transform[] spawnPoints;

    [Header("Opzioni")]
    [Tooltip("Se attivo, lo spawner parte automaticamente all'avvio della scena.")]
    [SerializeField] private bool startOnAwake = true;

    [SerializeField] private int bossCounter;

    private void OnEnable()
    {
        EventBus<EmptyPayload>.Subscribe("OnSelfDestruct", OnSpawnAtRandomPoint);
    }

    private void OnDisable()
    {
        EventBus<EmptyPayload>.Unsubscribe("OnSelfDestruct", OnSpawnAtRandomPoint);
    }

    private void Start()
    {
        SpawnAtRandomPoint();
    }

    private void SpawnAtRandomPoint()
    {
        if (bossCounter <= 0)
        {
            SelfDestroy();
        }

        bossCounter--;

        if (prefabToSpawn == null)
        {
            Debug.LogWarning($"{name}: prefabToSpawn non è assegnato.");
            return;
        }

        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogWarning($"{name}: nessuno spawn point assegnato.");
            return;
        }

        Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        Instantiate(
            prefabToSpawn,
            randomSpawnPoint.position,
            randomSpawnPoint.rotation
        );
    }

    private void OnSpawnAtRandomPoint(IEventPayload _)
    {
        SpawnAtRandomPoint();
    }

    public void SelfDestroy()
    {
        gameObject.SetActive(false);
    }
}