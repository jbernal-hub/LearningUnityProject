using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private bool newGame;
    [SerializeField] private Enemy enemyPrefab;
    [SerializeField] private EnemyAttributes enemyAttributes;
    [SerializeField] private List<Weapon> enemyWeapons;
    [SerializeField] private int enemyPoolSize = 30;
    [SerializeField] private float enemyYPos = 0.5f;
    [SerializeField] private float enemyMinZPos = -24;
    [SerializeField] private float enemyMaxZPos = 24;
    [SerializeField] private float enemyMinXPos = -21;
    [SerializeField] private float enemyMaxXPos = 21;
    [SerializeField] private float enemyTimeSpawn = 5f;
    private float currentEnemyTimeSpawn;
    private MonoBehaviourPool<Enemy> enemyPool;

    void Awake()
    {
        if (newGame || SaveSystem.GetInstance().CurrentSavedData.currentHealth == 0)
        {
            SaveSystem.GetInstance().CurrentSavedData = new SavedData();
            SaveSystem.GetInstance().Save();
        }

        enemyPool = new MonoBehaviourPool<Enemy>(enemyPrefab, enemyPoolSize);
        currentEnemyTimeSpawn = enemyTimeSpawn;
    }

    private void Start()
    {
        SavedData data = SaveSystem.GetInstance().CurrentSavedData;
        for (int i = 0; i < data.amountOfEnemies; i++)
        {
            SpawnEnemy(data.enemiesPositions[i]);
        }
    }

    public void Update()
    {
        currentEnemyTimeSpawn += Time.deltaTime;

        if(currentEnemyTimeSpawn >= enemyTimeSpawn)
        {
            SpawnEnemy();
            currentEnemyTimeSpawn = 0f;
        }
        // aquí quiero spawnear un enemigo cada vez que pase un intervalo de tiempo "enemyTimeSpawn"
        // el tiempo es con Time.time o con delta.time?
    }

    public void SpawnEnemy(Vector3 position = default)
    {
        if (position == default)
        {
            float x, y = enemyYPos, z;

            if (Random.value < 0.5f)
            {
                z = Random.value < 0.5f ? enemyMinZPos : enemyMaxZPos;
                x = Random.Range(enemyMinXPos, enemyMaxXPos);
            }
            else
            {
                x = Random.value < 0.5f ? enemyMinXPos : enemyMaxXPos;
                z = Random.Range(enemyMinZPos, enemyMaxZPos);
            }

            position = new Vector3(x, y, z);
        }

        Enemy enemy = enemyPool.GetObject();
        enemy.Initialize(position, enemyAttributes, enemyWeapons, OnEnemyDeath);
    }

    private void OnEnemyDeath(Enemy enemy)
    {
        enemyPool.ReturnObject(enemy);
        enemy.RemoveOnDeath(OnEnemyDeath);
    }
}
