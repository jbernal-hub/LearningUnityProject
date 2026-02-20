using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public abstract class BasicUnit : MonoBehaviour
{
    [SerializeField] protected BasicUnitAttributes attributes;
    [SerializeField] protected List<Weapon> weapons;
    private int currentHealth;
    protected Vector3 currentVelocity;
    private bool alive;

    public int CurrentHealth
    {
        get { return currentHealth; }
    }

    public bool Alive
    {
        get { return alive; }
    }


    static int weaponNum = 0;

    protected virtual void Start()
    {
        alive = true;

        if (attributes == null)
        {
            Debug.LogError($"Add the attributes to {gameObject.name}!");
            return;
        }

        currentHealth = attributes.Health;

        List<Weapon> newWeapons = new List<Weapon>();
        for (int i = 0; i < weapons.Count; i++)
        {
            newWeapons.Add(Instantiate(weapons[i]));
            newWeapons[i].name = weaponNum.ToString();
            weaponNum++;
            newWeapons[i].Initialize(this);
        }
        weapons = newWeapons;
    }

    protected void Attack(List<BasicUnit> targets, float dt)
    {
        for (int i = 0; i < weapons.Count; i++)
        {
            weapons[i].UpdateWeapon(targets, dt);
        }
    }

    public virtual void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            alive = false;
        }
    }

    public virtual void Heal(int amountHealth) { 

    }

    protected abstract void Update();

}
