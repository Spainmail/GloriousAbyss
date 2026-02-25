using UnityEngine;

public class Character_Action : MonoBehaviour
{
    [Header("References & Parameters")]
    public Unit_AI _parentAI;
    public int _characterID;
    public HealthBarUI _healthBarUI;
    public Transform _muzzlePoint;

    [Header("Debug")]
    [Tooltip("Deal direct damage instead of using projectiles/hitboxes.")]
    public bool _debugDamage;

    public void Action_Bow(Unit_AI targetAI, Character_Movement targetCharacter = null) //Change to arrow projectile.
    {
        GameObject bullet = PoolCollector.instance._pool_ProjectileGenericBullet.GetPrefab(); //Projectile
        Projectile proj = bullet.GetComponent<Projectile>();
        proj._hitPlayer = _parentAI._isEnemy;
        proj.damage = Random.Range(GameParameters.instance.GetDamageMin("Bow"), GameParameters.instance.GetDamageMax("Bow"));
        bullet.transform.position = _muzzlePoint.position;
        bullet.transform.rotation = Quaternion.LookRotation(targetCharacter != null ? (targetCharacter.transform.position - _muzzlePoint.position).normalized :
                                                                                        (targetAI.transform.position - _muzzlePoint.position).normalized);

        if (_debugDamage)
        {
            Debug.Log(_parentAI.GetUnitName() + " dealing damage to " + targetAI.GetUnitName());
            targetAI.DamageRandomCharacter(Random.Range(GameParameters.instance.GetDamageMin("Bow"), GameParameters.instance.GetDamageMax("Bow")));
        }
    }

    public void Action_Handgun(Unit_AI targetAI, Character_Movement targetCharacter = null)
    {
        GameObject bullet = PoolCollector.instance._pool_ProjectileGenericBullet.GetPrefab(); //Projectile
        Projectile proj = bullet.GetComponent<Projectile>();
        proj._hitPlayer = _parentAI._isEnemy;
        proj.damage = Random.Range(GameParameters.instance.GetDamageMin("Handgun"), GameParameters.instance.GetDamageMax("Handgun"));
        bullet.transform.position = _muzzlePoint.position;
        bullet.transform.rotation = Quaternion.LookRotation(targetCharacter != null ? (targetCharacter.transform.position - _muzzlePoint.position).normalized : 
                                                                                        (targetAI.transform.position - _muzzlePoint.position).normalized);

        if (_debugDamage)
        {
            Debug.Log(_parentAI.GetUnitName() + " dealing damage to " + targetAI.GetUnitName());
            targetAI.DamageRandomCharacter(Random.Range(GameParameters.instance.GetDamageMin("Handgun"), GameParameters.instance.GetDamageMax("Handgun")));
        }       
    }

    public void Action_Rifle(Unit_AI targetAI, Character_Movement targetCharacter = null)
    {
        GameObject bullet = PoolCollector.instance._pool_ProjectileGenericBullet.GetPrefab(); //Projectile
        Projectile proj = bullet.GetComponent<Projectile>();
        proj._hitPlayer = _parentAI._isEnemy;
        proj.damage = Random.Range(GameParameters.instance.GetDamageMin("Rifle"), GameParameters.instance.GetDamageMax("Rifle"));
        bullet.transform.position = _muzzlePoint.position;
        bullet.transform.rotation = Quaternion.LookRotation(targetCharacter != null ? (targetCharacter.transform.position - _muzzlePoint.position).normalized :
                                                                                        (targetAI.transform.position - _muzzlePoint.position).normalized);

        if (_debugDamage)
        {
            Debug.Log(_parentAI.GetUnitName() + " dealing damage to " + targetAI.GetUnitName());
            targetAI.DamageRandomCharacter(Random.Range(GameParameters.instance.GetDamageMin("Rifle"), GameParameters.instance.GetDamageMax("Rifle")));
        }
    }

    public void Action_Javelin(Unit_AI targetAI, Character_Movement targetCharacter = null)
    {
        GameObject bullet = PoolCollector.instance._pool_ProjectileGenericBullet.GetPrefab(); //Projectile
        Projectile proj = bullet.GetComponent<Projectile>();
        proj._hitPlayer = _parentAI._isEnemy;
        proj.damage = Random.Range(GameParameters.instance.GetDamageMin("Javelin"), GameParameters.instance.GetDamageMax("Javelin"));
        bullet.transform.position = _muzzlePoint.position;
        bullet.transform.rotation = Quaternion.LookRotation(targetCharacter != null ? (targetCharacter.transform.position - _muzzlePoint.position).normalized :
                                                                                        (targetAI.transform.position - _muzzlePoint.position).normalized);

        if (_debugDamage)
        {
            Debug.Log(_parentAI.GetUnitName() + " dealing damage to " + targetAI.GetUnitName());
            targetAI.DamageRandomCharacter(Random.Range(GameParameters.instance.GetDamageMin("Javelin"), GameParameters.instance.GetDamageMax("Javelin")));
        }
    }
}
