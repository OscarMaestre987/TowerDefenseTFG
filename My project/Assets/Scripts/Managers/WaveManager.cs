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
        waitingNextWave = true;
        skipWave = false;

        float timer = timeBetweenWaves;

        while (timer > 0)
        {
            if (skipWave)
            {
                Debug.Log("Oleada saltada");
                break;
            }

            if (waveTextUI != null)
            {
                waveTextUI.text =
                    "Comienzo de la siguiente oleada en: " + Mathf.Ceil(timer) +
                    "\n(P para saltar)";
            }

            timer -= Time.deltaTime;
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
        foreach (var enemyEntry in waveEnemies)
        {
            Transform baseSpawn = spawnPoints[Random.Range(0, spawnPoints.Length)];
            Vector2 offset2D = Random.insideUnitCircle * spawnOffsetRadius;
            Vector3 spawnPos = baseSpawn.position + new Vector3(offset2D.x, 0, offset2D.y);

            GameObject enemy = Instantiate(enemyEntry.prefab, spawnPos, baseSpawn.rotation * Quaternion.Euler(0, 180, 0));
            activeEnemies.Add(enemy);

            yield return new WaitForSeconds(delay);
        }

        isSpawning = false;
    }

    List<EnemyEntry> BuildWaveEnemyList(int totalPoints)
    {
        List<EnemyEntry> result = new List<EnemyEntry>();
        int remaining = totalPoints;

        int maxTries = 1000;
        int tries = 0;

        while (remaining > 0 && tries < maxTries)
        {
            List<EnemyEntry> valid = enemyTypes.FindAll(e => e.cost <= remaining);
            if (valid.Count == 0) break;

            EnemyEntry pick = valid[Random.Range(0, valid.Count)];
            result.Add(pick);
            remaining -= pick.cost;
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
