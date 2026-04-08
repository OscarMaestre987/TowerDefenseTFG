using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [System.Serializable]
    public class EnemyEntry
    {
        public GameObject prefab;
        public int cost;
    }

    public int baseWavePoints = 600;            
    public int pointsPerWaveIncrease = 300;   
    public float timeBetweenWaves = 20f;
    public float spawnOffsetRadius = 20f;
    public Transform[] spawnPoints;
    public List<EnemyEntry> enemyTypes = new List<EnemyEntry>();

    private int currentWave = 1;
    private int currentWavePoints;
    private int num;
    private List<GameObject> activeEnemies = new List<GameObject>();
    private bool isSpawning = false;
    private bool waitingNextWave = false;
    private bool skipWave = false;

    public TextMeshProUGUI waveTextUI;
    public float waveTextDisplayTime = 2f;
    void Start()
    {
        currentWavePoints = baseWavePoints - pointsPerWaveIncrease;
        StartCoroutine(StartNextWave());
    }

    void Update()
    {
        activeEnemies.RemoveAll(e => e == null);

        if (!isSpawning && activeEnemies.Count == 0)
        {
            StartCoroutine(StartNextWave());
        }
        if (waitingNextWave && Input.GetKeyDown(KeyCode.P))
        {
            skipWave = true;
        }
    }

    IEnumerator StartNextWave()
    {
        if (waveTextUI != null)
        {
            waveTextUI.text = $"Oleada {currentWave} comenzando...";
            StartCoroutine(ClearWaveTextAfterDelay(waveTextDisplayTime));
        }

        isSpawning = true;

        yield return StartNextWaveDelay();


        num = currentWave / 10;
        currentWavePoints = currentWavePoints + (int)(pointsPerWaveIncrease * Mathf.Pow(2, num));


        Debug.Log($"Oleada {currentWave} iniciando con {currentWavePoints} puntos {(int)(pointsPerWaveIncrease)}  llll{(int)Mathf.Pow(2, currentWave / 10f)}");
        currentWave++;

        List<EnemyEntry> waveEnemies = BuildWaveEnemyList(currentWavePoints);

        StartCoroutine(SpawnEnemiesGradually(waveEnemies, 0.5f));
    }

    IEnumerator StartNextWaveDelay()
    {
        // Activa el estado de espera entre oleadas
        waitingNextWave = true;
        skipWave = false;

        // Inicializa el temporizador con el tiempo configurado entre oleadas
        float timer = timeBetweenWaves;

        // Bucle que cuenta atrás hasta que empiece la siguiente oleada
        while (timer > 0)
        {
            // Permite al jugador saltar la espera manualmente
            if (skipWave)
            {
                Debug.Log("Oleada saltada");
                break;
            }
            // Actualiza el texto en pantalla mostrando la cuenta atrás
            if (waveTextUI != null)
            {
                waveTextUI.text =
                    $"Oleada {currentWave} en: " + Mathf.Ceil(timer) +
                    "\n(P para saltar)";
            }
            // Reduce el tiempo usando el deltaTime (tiempo real entre frames)
            timer -= Time.deltaTime;

            // Espera al siguiente frame (comportamiento típico en coroutines)
            yield return null;
        }
        // limpiar texto
        if (waveTextUI != null)
            waveTextUI.text = "";

        StartNextWave();
        waitingNextWave = false;
    }

    IEnumerator ClearWaveTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (waveTextUI != null)
        {
            waveTextUI.text = "";
        }
    }


    IEnumerator SpawnEnemiesGradually(List<EnemyEntry> waveEnemies, float delay = 0.5f)
    {
        // Recorre la lista de enemigos generada previamente para la oleada
        foreach (var enemyEntry in waveEnemies)
        {
            // Selecciona un punto de spawn aleatorio
            Transform baseSpawn = spawnPoints[Random.Range(0, spawnPoints.Length)];

            // Aplica un pequeño desplazamiento aleatorio alrededor del punto base
            // para evitar que todos los enemigos aparezcan exactamente en el mismo lugar
            Vector2 offset2D = Random.insideUnitCircle * spawnOffsetRadius;
            Vector3 spawnPos = baseSpawn.position + new Vector3(offset2D.x, 0, offset2D.y);

            // Instancia el enemigo con una rotación ajustada (mirando hacia el jugador/base)
            // Se añade a la lista de enemigos activos para su gestión posterior
            GameObject enemy = Instantiate(
                enemyEntry.prefab,
                spawnPos,
                baseSpawn.rotation * Quaternion.Euler(0, 180, 0));
            activeEnemies.Add(enemy);

            // Espera antes de generar el siguiente enemigo (spawn progresivo)
            yield return new WaitForSeconds(delay);
        }
        // Indica que la oleada ha terminado de generarse
        isSpawning = false;
    }

    List<EnemyEntry> BuildWaveEnemyList(int totalPoints)
    {
        List<EnemyEntry> result = new List<EnemyEntry>();
        int remaining = totalPoints;

        // Límite de intentos para evitar bucles infinitos
        int maxTries = 1000;
        int tries = 0;

        // Mientras queden puntos por asignar y no se supere el límite de intentos
        while (remaining > 0 && tries < maxTries)
        {
            // Filtrar enemigos cuyo coste sea menor o igual a los puntos restantes
            List<EnemyEntry> valid = enemyTypes.FindAll(e => e.cost <= remaining);

            // Si no hay enemigos válidos, se termina el bucle
            if (valid.Count == 0) break;

            // Seleccionar un enemigo aleatorio de los válidos y añadirlo a la lista de la oleada
            EnemyEntry pick = valid[Random.Range(0, valid.Count)];
            result.Add(pick);

            // Restar el coste del enemigo a los puntos restantes
            remaining -= pick.cost;

            // Incrementar contador de intentos
            tries++;
        }

        int finalTotal = 0;
        foreach (var e in result) finalTotal += e.cost;

        if (finalTotal != totalPoints)
        {
            Debug.LogWarning($"[WaveManager] Error: no se pudo crear una combinación exacta de {totalPoints} puntos. Resultado: {finalTotal}");
        }
        return result;
    }
    int GetMinimumEnemyCost()
    {
        int min = int.MaxValue;
        foreach (var e in enemyTypes)
        {
            if (e.cost < min) min = e.cost;
        }
        return min;
    }
}
