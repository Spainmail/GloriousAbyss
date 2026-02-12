using UnityEngine;

public class Character_Action : MonoBehaviour
{
    [Header("References & Parameters")]
    public Unit_AI _parentAI;
    public Transform _muzzlePoint;

    [Header("Debug")]
    [Tooltip("Deal direct damage instead of using projectiles/hitboxes.")]
    public bool _debugDamage;

    //public void ValidateAction(Enum_Actions action, Transform target = null)
    //{
    //    switch (action)
    //    {
    //        case Enum_Actions.Bow:

    //            break;
    //        case Enum_Actions.Handgun:

    //            break;
    //        case Enum_Actions.Rifle:

    //            break;
    //        case Enum_Actions.Javelin:

    //            break;
    //    }
    //}

    public void Action_Bow()
    {
        //Projectile
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
}
