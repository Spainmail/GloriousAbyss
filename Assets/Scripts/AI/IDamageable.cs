using UnityEngine;

public interface IDamageable
{
    public Unit_AI _parentAI { get; }

    public void TakeDamage(float damage);
}
