using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : ScriptableObject
{
    [SerializeField] protected int damage;
    private BasicUnit holder;

    public BasicUnit Holder { get { return holder; } }

    public virtual void Initialize(BasicUnit holder)
    {
        this.holder = holder;
    }

    public abstract void UpdateWeapon(List<BasicUnit> targets, float dt);
}
