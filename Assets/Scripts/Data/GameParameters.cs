using UnityEngine;

public class GameParameters : MonoBehaviour
{
    public static GameParameters instance;

    [Header("Unit Rules")]
    [SerializeField] private float _decisionInterval;
    [SerializeField] private int[] _Upgrade_SquadSize;
    [SerializeField] private int[] _Upgrade_UnitSize;

    [Header("Action Rules")]
    public Model_Action[] Actions; //Action list.

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

    public float GetDamageMin(string name)
    {
        foreach (Model_Action action in Actions)
        {
            if (action.actionName == name) return action.damageMin;
        }
        return 0f;
    }

    public float GetDamageMax(string name)
    {
        foreach (Model_Action action in Actions)
        {
            if (action.actionName == name) return action.damageMax;
        }
        return 0f;
    }

    public Model_Action GetAction(string name)
    {
        foreach (Model_Action action in Actions)
        {
            if (action.actionName == name)
            {
                return action;
            }
        }

        return null;
    }
}
