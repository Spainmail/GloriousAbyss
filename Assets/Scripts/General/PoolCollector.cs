using UnityEngine;

public class PoolCollector : MonoBehaviour
{
    public static PoolCollector instance;

    [Header("Pools")]
    public ObjectPooler _pool_ProjectileGenericBullet;
    public ObjectPooler _pool_ProjectileBow;
    public ObjectPooler _pool_ProjectileJavelin;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }
}
