using System;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : BasicUnit
{
    private float currentAlertTime;
    private EnemyAttributes enemyAttributes;
    private EnemyState enemyState;
    private Player player;
    private List<BasicUnit> playerList;
    private Action<Enemy> onDeath;

    private void Awake()
    {
        playerList = new List<BasicUnit>();
        player = FindFirstObjectByType<Player>();
        playerList.Add(player);
    }

    protected override void Start()
    {
        base.Start();

        enemyState = EnemyState.IDLE;

        enemyAttributes = (EnemyAttributes)attributes;
        if (enemyAttributes == null)
        {
            Debug.LogError("Please add enemy attributes instead of basic unit attributes to " + gameObject.name);
            return;
        }
    }

    private void OnEnable()
    {
        player.AddEnemy(this);
    }

    private void OnDisable()
    {
        player.RemoveEnemy(this);
    }

    protected override void Update()
    {
        if (!Alive)
        {
            Debug.Log("Ded");
            return;
        }

        float dt = Time.deltaTime;

        Vector3 positionDiff = player.transform.position - this.transform.position;
        float sqrDistancewithPlayer = positionDiff.sqrMagnitude; // TODO

        switch (enemyState) 
        {
            case EnemyState.IDLE:
                if (sqrDistancewithPlayer < enemyAttributes.VisionRange * enemyAttributes.VisionRange)
                {
                    currentAlertTime = enemyAttributes.AlertTime;
                    enemyState = EnemyState.ALERTED;
                }
                break;
            case EnemyState.ALERTED:
                currentAlertTime -= dt;

                if (currentAlertTime <= 0)
                { 
                    enemyState = EnemyState.CHASING;
                }
                break;
            case EnemyState.CHASING:
                if (sqrDistancewithPlayer > enemyAttributes.VisionRange * enemyAttributes.VisionRange)
                {
                    // Deccelerate
                    Vector3 deceleration = currentVelocity.normalized * (enemyAttributes.Deceleration * dt);
                    if (deceleration.sqrMagnitude < currentVelocity.sqrMagnitude) 
                    {
                        currentVelocity -= deceleration;
                    }
                    else
                    {
                        currentVelocity = Vector3.zero;
                        enemyState = EnemyState.IDLE;
                    }
                }
                else
                {
                    // Accelerate
                    positionDiff.y = 0;
                    currentVelocity += positionDiff.normalized * (enemyAttributes.Acceleration * dt);

                    if (currentVelocity.sqrMagnitude > attributes.MaxVelocity * attributes.MaxVelocity)
                    {
                        currentVelocity = currentVelocity.normalized * attributes.MaxVelocity;
                    }

                    Attack(playerList, dt);
                }

                break;
        }
    }

    private void FixedUpdate()
    {
        if (!Alive)
        {
            return;
        }

        float dt = Time.fixedDeltaTime;

        transform.position += currentVelocity * dt;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Bullet>() is Bullet b && b != null) {
            TakeDamage(b.Damage);
            if (!Alive)
            {
                onDeath?.Invoke(this);
                gameObject.SetActive(false);
                weapons.Clear();
            }
        }
    }

    public void Initialize(Vector3 position, EnemyAttributes enemyAttributes, List<Weapon> weapons, Action<Enemy> onDeath)
    {
        transform.position = position;
        this.attributes = enemyAttributes;
        this.weapons = weapons;
        Start();
        AddOnDeath(onDeath);
        gameObject.SetActive(true);
    }

    public void AddOnDeath(Action<Enemy> onDeath)
    {
        this.onDeath += onDeath;
    }

    public void RemoveOnDeath(Action<Enemy> onDeath)
    {
        this.onDeath -= onDeath;
    }
}
