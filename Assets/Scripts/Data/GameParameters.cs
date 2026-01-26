using System.Runtime.ExceptionServices;
using UnityEngine;

public class GameParameters : MonoBehaviour
{
    public static GameParameters instance;

    [Header("Unit Rules")]
    [SerializeField] private float _decisionInterval;
    [SerializeField] private int[] _Upgrade_SquadSize;
    [SerializeField] private int[] _Upgrade_UnitSize;

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

    public float GetInterval() { return _decisionInterval; }
}
