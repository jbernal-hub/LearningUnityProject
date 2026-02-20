using System;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using static UnityEngine.UI.Image;

[CreateAssetMenu(fileName = "Shotgun", menuName = "Entalto/Shotgun Weapon")]
public class Shotgun : Weapon
{
    [SerializeField, Min(0)] private float maxRange;
    [SerializeField, Min(0)] private float cooldown = 1;
    [SerializeField, Min(0)] private float bulletVelocity = 20;
    [SerializeField, Min(0)] private float bulletLifeTime = 3;
    [SerializeField, Min(1)] private int maxBullets = 10;
    [SerializeField] private Bullet bulletPrefab;

    private float currentCooldown;
    private MonoBehaviourPool<Bullet> bulletsPool;

    public override void Initialize(BasicUnit holder)
    {
        base.Initialize(holder);
        currentCooldown = cooldown;
        bulletsPool = new MonoBehaviourPool<Bullet>(bulletPrefab, maxBullets);
    }

    public override void UpdateWeapon(List<BasicUnit> targets, float dt)
    {
        currentCooldown -= dt;

        if(currentCooldown <= 0)
        {
            Shoot(targets);
            currentCooldown = cooldown;
        }
    }

    private void Shoot(List<BasicUnit> targets)
    {
        if (targets.Count == 0)
        {
            return;
        }

        Vector3 diffDistance = targets[0].transform.position - Holder.transform.position;
        float sqrDistMin = diffDistance.sqrMagnitude;

        for (int i = 1; i < targets.Count; i++)
        {
            Vector3 currentDiffDistance = (targets[i].transform.position - Holder.transform.position);
            float currentSqrDistMin = currentDiffDistance.sqrMagnitude;
            if (sqrDistMin > currentSqrDistMin)
            {
                sqrDistMin = currentSqrDistMin;
                diffDistance = currentDiffDistance;
            }
        }
        // If the enemy is out of range dont shoot
        if (sqrDistMin > maxRange * maxRange) return;

        Bullet shootBullet = bulletsPool.GetObject();

        if (shootBullet != null)
        {
            diffDistance.y = 0;
            Vector3 shootingDirection = diffDistance.normalized;
            Vector3 bulletVelocity = shootingDirection * this.bulletVelocity;

            bool isPlayer = Holder is Player;
            int layer = isPlayer ? LayerMask.NameToLayer("PlayerBullet") : LayerMask.NameToLayer("EnemyBullet");
            shootBullet.Initialize(damage, bulletLifeTime, layer, Holder.transform.position, bulletVelocity, bulletsPool.ReturnObject);
        }
    }
}

