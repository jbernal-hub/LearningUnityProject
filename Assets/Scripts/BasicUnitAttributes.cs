using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Basic Unit Attributes", menuName = "Entalto/Units Attributes")]
public class BasicUnitAttributes : ScriptableObject
{
    [SerializeField, Min(1)] private int health = 5;
    [SerializeField, Min(0)] private float maxVelocity = 20, acceleration = 40;
    [SerializeField] private float deceleration = 80f;

    public int Health
    {
        get { return health; }
    }

    public float MaxVelocity
    {
        get { return maxVelocity; }
    }
    public float Acceleration
    {
        get { return acceleration; }
    }

    public float Deceleration 
    {
        get { return deceleration; }
    }

    
}
