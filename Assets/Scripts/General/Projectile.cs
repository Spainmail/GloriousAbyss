using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Projectile : MonoBehaviour
{
    public float damage;
    private Rigidbody _bulletRB;
    [SerializeField] private float _bulletVelocity;
    private Vector3 _previousPosition;
    public bool _hitPlayer;
    [SerializeField] private LayerMask _enemyLayerMask;
    [SerializeField] private LayerMask _playerLayerMask;
    [SerializeField] private GameObject _defaultImpactVFXPrefab;

    public bool _stopExecuting;
    public float _bulletTimer = 5f;

    [Header("Debugging")]
    public bool _debugMessages;
    public bool _debugDamage;

    private void Awake()
    {
        _bulletRB = GetComponent<Rigidbody>();
    }

    void Start()
    {
        _bulletRB.linearVelocity = transform.forward * _bulletVelocity;
    }

    private void OnEnable()
    {
        // Reset state for pooled projectiles
        _stopExecuting = false;
        _bulletTimer = 5f;
        _previousPosition = transform.position;
        if (_bulletRB != null)
        {
            _bulletRB.linearVelocity = transform.forward * _bulletVelocity;
        }
    }

    void Update()
    {
        if (_stopExecuting) return;

        Ray ray = new Ray(_previousPosition, transform.forward);

        Vector3 currentPosition = transform.position;
        Vector3 movement = currentPosition - _previousPosition;
        float travelDistance = movement.magnitude;
        Vector3 direction = movement / travelDistance;
        LayerMask mask = _hitPlayer ? _playerLayerMask : _enemyLayerMask;

        // Guard against zero distance to avoid NaNs/Infs
        if (travelDistance > Mathf.Epsilon)
        {
            direction = movement / travelDistance;
        }
        else
        {
            // fallback short check forward (first frame or tiny movement)
            direction = transform.forward;
            travelDistance = 0.25f;
        }

        if (Physics.Raycast(_previousPosition, direction, out RaycastHit rayHit, travelDistance, mask, QueryTriggerInteraction.Collide))

            //if (Physics.Raycast(ray, out RaycastHit rayHit, Vector3.Distance(_previousPosition, gameObject.transform.position), _hitPlayer ? _playerLayerMask : _enemyLayerMask, queryTriggerInteraction: QueryTriggerInteraction.Collide))
        {
            //Check what was hit.
            Debug.Log($"Projectile hit collider={rayHit.collider.name} on layer={LayerMask.LayerToName(rayHit.collider.gameObject.layer)}");

            IDamageable hitEnemy = rayHit.collider.GetComponentInParent<IDamageable>();
            if (hitEnemy != null)
            {
                if (hitEnemy._parentAI != null) Debug.Log($"Applying {damage} damage to {hitEnemy._parentAI.GetUnitName()}");
                if (_debugDamage) hitEnemy.TakeDamage(damage);
            }
            else if (_debugMessages)
            {
                Debug.Log("Hit collider did not expose IDamageable (GetComponentInParent returned null)");
            }

            if (_defaultImpactVFXPrefab != null) Instantiate(_defaultImpactVFXPrefab, rayHit.point, Quaternion.LookRotation(rayHit.normal)); //Spawn default impact effect.

            Destroy(gameObject); //Destroy this bullet.
        }

        if (_stopExecuting) Destroy(gameObject);

        AdvanceBulletTimer();
    }

    private void AdvanceBulletTimer()
    {
        _bulletTimer -= Time.deltaTime;
        if (_bulletTimer <= 0)
        {
            Debug.Log("Bullet dying from timer.");
            Destroy(gameObject);
        }
    }

    private void LateUpdate()
    {
        _previousPosition = gameObject.transform.position;
    }
}
