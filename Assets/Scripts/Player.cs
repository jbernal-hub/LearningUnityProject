using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class Player : BasicUnit
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TMP_Text enemiesDeadCount;

    private int killedEnemies;
    private List<BasicUnit> enemies = new List<BasicUnit>();

    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private float speed = 5f;

    protected override void Start()
    {
        base.Start();
        SavedData data = SaveSystem.GetInstance().CurrentSavedData;
        if (data.currentHealth > 0)
        {
            TakeDamage(CurrentHealth - data.currentHealth);
        }
        healthSlider.maxValue = attributes.Health;
        healthSlider.value = CurrentHealth;
        killedEnemies = data.amountOfKills;
        enemiesDeadCount.text = $"{killedEnemies} kills";
        transform.position = data.playerPosition;
    }

    public void AddEnemy(Enemy enemy)
    {
        enemies.Add(enemy);
        enemy.AddOnDeath(EnemyKilled);
    }

    public void RemoveEnemy(Enemy enemy)
    {
        enemies.Remove(enemy);
        enemy.RemoveOnDeath(EnemyKilled);
    }

    private void EnemyKilled(Enemy enemy)
    {
        killedEnemies++;
        enemiesDeadCount.text = $"{killedEnemies} kills";

        SaveAllData();
    }

    private void SaveAllData()
    {
        SavedData data = SaveSystem.GetInstance().CurrentSavedData;
        data.currentHealth = CurrentHealth;
        data.playerPosition = transform.position;
        data.amountOfKills = killedEnemies;
        data.amountOfEnemies = enemies.Count;
        Vector3[] enemiesPositions = new Vector3[data.amountOfEnemies];
        for (int i = 0; i < data.amountOfEnemies; i++)
        {
            enemiesPositions[i] = enemies[i].transform.position;
        }
        data.enemiesPositions = enemiesPositions;

        SaveSystem.GetInstance().CurrentSavedData = data;
        SaveSystem.GetInstance().Save();
    }

    protected override void Update()
    {
        if (!Alive)
        {
            return;
        }

        float dt = Time.deltaTime;

        Move(dt);
        Attack(enemies, dt);
    }

    private void FixedUpdate()
    {
        if (!Alive)
        {
            return;
        }

        float dt = Time.fixedDeltaTime;


        //transform.position += currentVelocity * dt;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Bullet>() is Bullet b && b != null)
        {
            TakeDamage(b.Damage);
            healthSlider.value = CurrentHealth;
        }
    }
    private void OnEnable()
    {
        moveAction.action.Enable();
    }

    private void OnDisable()
    {
        moveAction.action.Disable();
    }
    private void Move(float dt)
    {
        Vector2 input = moveAction.action.ReadValue<Vector2>();

        Vector3 direction = new Vector3(input.x, 0f, input.y);

        transform.position += direction * speed * dt;
    }

}


    /*
    private void Move(float dt)
    {
        if (Input.GetKey(KeyCode.W))
        {
            currentVelocity.z += attributes.Acceleration * dt;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            currentVelocity.z -= attributes.Acceleration * dt;
        }
        else
        {
            if (currentVelocity.z > Mathf.Epsilon || currentVelocity.z < -Mathf.Epsilon)
            {
                // Deccelerate
                float currentDeceleration = Mathf.Sign(currentVelocity.z) * attributes.Deceleration * dt;
                if (currentVelocity.z < currentDeceleration)
                {
                    currentVelocity.z = 0;
                }
                else
                {
                    currentVelocity.z -= currentDeceleration;
                }
            }
        }

        if (Input.GetKey(KeyCode.A))
        {
            currentVelocity.x -= attributes.Acceleration * dt;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            currentVelocity.x += attributes.Acceleration * dt;
        }
        else
        {
            if (currentVelocity.x > Mathf.Epsilon || currentVelocity.x < -Mathf.Epsilon)
            {
                float currentDeceleration = Mathf.Sign(currentVelocity.x) * attributes.Deceleration * dt;
                if (currentVelocity.x < currentDeceleration)
                {
                    currentVelocity.x = 0;
                }
                else
                {
                    currentVelocity.x -= currentDeceleration;
                }
            }
        }

        if (currentVelocity.sqrMagnitude > attributes.MaxVelocity * attributes.MaxVelocity)
        {
            currentVelocity = currentVelocity.normalized * attributes.MaxVelocity;
        }
    }
}
    */
