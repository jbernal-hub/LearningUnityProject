using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Attributes", menuName = "Entalto/Enemy Units Attributes")]
public class EnemyAttributes : BasicUnitAttributes
{
    [SerializeField] private float visionRange;
    [SerializeField] private float alertTime;

    public float VisionRange
    {
        get { return visionRange; }
    }

    public float AlertTime
    {
        get { return alertTime; }
    }

}
