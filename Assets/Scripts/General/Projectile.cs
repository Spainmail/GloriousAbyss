using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody bulletRB;
    [SerializeField] private float bulletVelocity;
    private Vector3 previousPosition;
    [SerializeField] private LayerMask shootableLayerMask;
    [SerializeField] private GameObject defaultImpactVFXPrefab;

    public bool stopExecuting;
    public float bulletTimer = 5f;

    [Header("Debugging")]
    private bool _debugMessages;

    private void Awake()
    {
        bulletRB = GetComponent<Rigidbody>();
    }

    void Start()
    {
        bulletRB.linearVelocity = transform.forward * bulletVelocity; //If a normal Ruby projectile, set velocity and be done with it.
    }

    void Update()
    {
        if (stopExecuting) return;

        Ray ray = new Ray(previousPosition, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit rayHit, Vector3.Distance(previousPosition, gameObject.transform.position), shootableLayerMask, queryTriggerInteraction: QueryTriggerInteraction.Collide))
        {
            //Check what was hit.
            //rayHit.transform.TryGetComponent<Health_Enemy>(out Health_Enemy enemyHealth);
            //if (enemyHealth != null)
            //{
            //    enemyHealth.DealDamage(bulletDamage, rayHit.point, damageType, bulletForce, false, bulletStunDamage, false);
            //}

            if (defaultImpactVFXPrefab != null) Instantiate(defaultImpactVFXPrefab, rayHit.point, Quaternion.LookRotation(rayHit.normal)); //Spawn default impact effect.

            Destroy(gameObject); //Destroy this bullet.
        }

        if (stopExecuting) Destroy(gameObject);

        AdvanceBulletTimer();
    }

    private void AdvanceBulletTimer()
    {
        bulletTimer -= Time.deltaTime;
        if (bulletTimer <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void LateUpdate()
    {
        previousPosition = gameObject.transform.position;
    }
}
