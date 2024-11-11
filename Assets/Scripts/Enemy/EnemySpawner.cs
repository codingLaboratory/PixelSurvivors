using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class Wave {
        public GameObject enemyPrefab;
        public float spawnTimer;
        public float spawnInterval;
        public int enemiesPerWave;
        public int spawnedEnemyCount;
    }
    public List<Wave> waves;
    public int waveNumber;
    public Transform minPos;
    public Transform maxPos;

    void Update()
    {
        if (PlayerController.Instance.gameObject.activeSelf){
            waves[waveNumber].spawnTimer += Time.deltaTime;
            if (waves[waveNumber].spawnTimer >= waves[waveNumber].spawnInterval){
                waves[waveNumber].spawnTimer = 0;
                SpawnEnemy();
            }
            if (waves[waveNumber].spawnedEnemyCount >= waves[waveNumber].enemiesPerWave){
                waves[waveNumber].spawnedEnemyCount = 0;
                if (waves[waveNumber].spawnInterval > 0.15f){
                    waves[waveNumber].spawnInterval *= 0.8f;
                }
                waveNumber++;
            }
            if (waveNumber >= waves.Count){
                waveNumber = 0;
            }
        }
    }

    private void SpawnEnemy(){
        Instantiate(waves[waveNumber].enemyPrefab, RandomSpawnPoint(), transform.rotation);
        waves[waveNumber].spawnedEnemyCount++;
    }

    private Vector2 RandomSpawnPoint(){
        Vector2 spawnPoint;
        if (Random.Range(0f, 1f) > 0.5){
            spawnPoint.x = Random.Range(minPos.position.x, maxPos.position.x);
            if (Random.Range(0f, 1f) > 0.5){
                spawnPoint.y = minPos.position.y;
            } else {
                spawnPoint.y = maxPos.position.y;
            }
        } else {
            spawnPoint.y = Random.Range(minPos.position.y, maxPos.position.y);
            if (Random.Range(0f, 1f) > 0.5){
                spawnPoint.x = minPos.position.x;
            } else {
                spawnPoint.x = maxPos.position.x;
            }
        }
        return spawnPoint;
    }
}
