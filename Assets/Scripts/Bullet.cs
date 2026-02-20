using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private int damage;
    private float lifeTime;
    private Vector3 velocity;
    private Action<Bullet> onDestroy;

    public int Damage => damage;

    public void Initialize(int damage, float lifeTime, int layer, Vector3 initialPosition, Vector3 velocity, Action<Bullet> onDestroy)
    {
        gameObject.SetActive(true);
        this.damage = damage;
        this.lifeTime = lifeTime;
        this.velocity = velocity;
        this.onDestroy = onDestroy;
        transform.position = initialPosition;
        gameObject.layer = layer;
    }

    private void FixedUpdate()
    {
        lifeTime -= Time.fixedDeltaTime;
        if (lifeTime > Mathf.Epsilon)
        {
            transform.position = transform.position + velocity * Time.fixedDeltaTime;
        }
        else
        {
            gameObject.SetActive(false);
            onDestroy.Invoke(this);
        }
    }
}
