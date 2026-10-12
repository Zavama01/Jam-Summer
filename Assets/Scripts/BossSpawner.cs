using UnityEngine;

public class RandomSpawnPointSpawner : MonoBehaviour
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

    private float timer;
    private bool isSpawning;

    private void Start()
    {
        if (startOnAwake)
            StartSpawning();
    }

    private void Update()
    {
        if (!isSpawning)
            return;

        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            timer = 0f;
            SpawnAtRandomPoint();
        }
    }

    public void StartSpawning()
    {
        isSpawning = true;
        timer = 0f;
    }

    public void StopSpawning()
    {
        isSpawning = false;
    }

    private void SpawnAtRandomPoint()
    {
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
}