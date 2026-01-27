using System;
using System.Collections.Generic;
using UnityEngine;

public class Unit_AI : MonoBehaviour
{
    [Header("Unit Parameters")]
    [SerializeField] private Model_Unit _unitStats;                                                 //This reference is set on unit instantiation.
    [SerializeField] private float _decisionInterval_Current;
    [SerializeField] private List<Character_Movement> _characters_Current;

    [Header("Unit Components")]
    [SerializeField] private Unit_Movement _movement;

    void Start()
    {   
        SetupComponents();
    }

    private void SetupComponents()
    {
        _movement = GetComponent<Unit_Movement>();
        _movement._unitAI = this;
        if (_unitStats != null) _movement._moveSpeed = _unitStats.moveSpeed;
    }

    void Update()
    {
        if (!BattleManager.instance._battleActive) return;
                                                                                                        //TO DO: Check if all characters in unit dead.

        if (_decisionInterval_Current > 0f)
        {
            _decisionInterval_Current -= Time.deltaTime;
        }
        else //Time to make a decision.
        {
            ValidateBehaviour();
            _decisionInterval_Current = GameParameters.instance.GetInterval();
        }
    }

    private void ValidateBehaviour()
    {
        //Check behaviours one after one.
        //Check if the first valid one is already in progress.
        //If not, check if unit needs to move for it.
        //If yes, start movement.
        //If no, execute action on target.
        //If yes, update target and let movement continue.
        //If no other valid behaviour, put on standby for victory screens.
        for (int i = 0; i < _unitStats.behaviourCurrent.Count; i++)
        {
            //if (GetValidTarget(_unitStats.behaviourCurrent[i].target, _unitStats.behaviourCurrent[i].condition) != null)
            //{

            //}
        }
    }

    #region Behaviour Checks

    //public GameObject GetValidTarget(Enum_Targets targetType, Enum_Conditions condition)
    //{
    //    switch (targetType)
    //    {
    //        case Enum_Targets.Ally:

    //            break;
    //        case Enum_Targets.Enemy:

    //            break;
    //        case Enum_Targets.Pickup:

    //            break;
    //        case Enum_Targets.Destructible:
                
    //            break;
    //        case Enum_Targets.Gate:

    //            break;
    //        case Enum_Targets.Self:
    //            return this.gameObject;
    //        default:
    //            return this.gameObject;
    //    }
    //}

    //private GameObject GetValidAlly(Enum_Conditions condition)
    //{
    //    switch (condition)
    //    {
    //        case Enum_Conditions.HPHighest:
    //            //Check list of allies, remove self, and find unit with highest total HP.
    //            break;
    //        case Enum_Conditions.HPLowest:
    //            //Check list of allies, remove self, and find unit with lowest total HP.
    //            break;
    //        case Enum_Conditions.Nearest:
    //            //Check list of allies, remove self, and find nearest unit.
    //            break;
    //        case Enum_Conditions.Farthest:
    //            //Check list of allies, remove self, and find farthest unit.
    //            break;
    //        case Enum_Conditions.OnStandBy:
    //            //Check list of allies, remove self, and find nearest unit currently on standby.
    //            break;
    //        case Enum_Conditions.Shielded:
    //            //Check list of allies, remove self, and find nearest shielded unit.
    //            break;
    //        case Enum_Conditions.Unshielded:
    //            //Check list of allies, remove self, and find nearest unshielded unit.
    //            break;
    //    }
    //}

    #endregion

    #region GetData Functions

    public List<Character_Movement> GetCharacters()
    {
        return _characters_Current;
    }

    public float GetInterval() 
    {
        return _decisionInterval_Current;
    }

    #endregion

    #region Debug

    public void Debug_Movement()
    {
        _movement.Debug_MovementStart();
    }

    #endregion
}
