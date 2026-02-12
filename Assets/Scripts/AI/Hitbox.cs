using UnityEngine;

public class Hitbox : MonoBehaviour, IDamageable
{
    public Unit_AI _parentAI;
    Unit_AI IDamageable._parentAI{ get { return _parentAI; } }

    public void TakeDamage(float damage)
    {
        Debug.Log("Relaying damage to " + _parentAI.GetUnitName());
        _parentAI.DamageRandomCharacter(damage);
    }
}
