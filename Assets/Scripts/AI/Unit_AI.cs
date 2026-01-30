using System;
using System.Collections.Generic;
using UnityEngine;

public class Unit_AI : MonoBehaviour
{
    [Header("Unit Parameters")]
    [SerializeField] public bool _isEnemy;
    [SerializeField] private Model_Unit _unitStats;                                                 //This reference is set on unit instantiation.
    [SerializeField] private float _decisionInterval_Current;
    [SerializeField] private List<Character_Movement> _characters_Current;

    [Header("Unit Components")]
    [SerializeField] private Unit_Movement _movement;

    void Start()
    {   
        //SetupComponents();
    }

    public void SetupComponents(bool isEnemy, Model_Unit unit)
    {
        _movement = GetComponent<Unit_Movement>();
        _movement._unitAI = this;
        _unitStats = unit;
        transform.parent.name = unit.name;
        _isEnemy = isEnemy;
        _movement._moveSpeed = _unitStats.moveSpeed;
    }

    void Update()
    {
        if (!BattleManager.instance._battleActive) return;
                                                                                                        //TO DO: Check if all characters in unit dead.

        if (_decisionInterval_Current > 0f)
        {
            _decisionInterval_Current -= Time.deltaTime;
        }
        else //Time to make a decision.                                                                 //TO DO: Check if any characters in unit alive.
        {
            ValidateBehaviour();
            _decisionInterval_Current = GameParameters.instance.GetInterval();
        }
    }

    private void ValidateBehaviour()
    {
        if (Debug.isDebugBuild) Debug.Log(transform.parent.name + " making a decision!");
        //Check behaviours one after one.
        //Check if the first valid one is already in progress.
        //If not, check if unit needs to move for it.
        //If yes, start movement.
        //If no, execute action on target.
        //If yes, update target and let movement continue.
        //If no other valid behaviour, put on standby for victory screens.
        for (int i = 0; i < _unitStats.behaviourCurrent.Count; i++)
        {
            if (GetValidTarget(_unitStats.behaviourCurrent[i].target, _unitStats.behaviourCurrent[i].condition) != null) //If null, check next behaviour.
            {
                //Check range of action.
                //If UNIT center within range, execute action (for characters who are caught up with their squad positions).
                //If not within range, start movement towards target's nearest character.
            }
        }
    }

    #region Behaviour Checks

    public Unit_AI GetValidTarget(Enum_Targets targetType, Enum_Conditions condition)
    {
        switch (targetType)
        {
            case Enum_Targets.Ally:
                return GetValidAlly(condition);
            case Enum_Targets.Enemy:
                return GetValidEnemy(condition);
            case Enum_Targets.Self:
                return GetValidSelf(condition);
            default:
                return null;
        }
    }

    private Unit_AI GetValidAlly(Enum_Conditions condition)
    {
        switch (condition)
        {
            case Enum_Conditions.HPHighest: //Check list of allies, remove self, and find unit with highest total HP.
                int tempUnitID_HPHigh = 0;
                float tempHP_High = 0;
                for (int i = 0; i < BattleManager.instance._playerUnitAIs.Count; i++)
                {
                    if (BattleManager.instance._playerUnitAIs[i] != this) //First make sure we are not checking ourself.
                    {
                        float tempTotalHP = BattleManager.instance._playerUnitAIs[i]._unitStats.GetTotalHP();
                        if (tempTotalHP > 0f && tempTotalHP > tempHP_High)
                        {
                            tempUnitID_HPHigh = i;
                            tempHP_High = tempTotalHP;
                        }
                    }
                }
                if (tempHP_High == 0) return null; //Only this unit is left alive.
                else return BattleManager.instance._playerUnitAIs[tempUnitID_HPHigh];

            case Enum_Conditions.HPLowest: //Check list of allies, remove self, and find unit with lowest total HP.
                int tempUnitID_HPLow = 0;
                float tempHP_Low = 0;
                for (int i = 0; i < BattleManager.instance._playerUnitAIs.Count; i++)
                {
                    if (BattleManager.instance._playerUnitAIs[i] != this) //First make sure we are not checking ourself.
                    {
                        float tempTotalHP = BattleManager.instance._playerUnitAIs[i]._unitStats.GetTotalHP();
                        if (tempTotalHP > 0f && tempTotalHP < tempHP_Low)
                        {
                            tempUnitID_HPLow = i;
                            tempHP_Low = tempTotalHP;
                        }
                    }
                }
                if (tempHP_Low == 0) return null; //Only this unit is left alive.
                else return BattleManager.instance._playerUnitAIs[tempUnitID_HPLow];

            case Enum_Conditions.Nearest: //Check list of allies, remove self, and find nearest unit.
                int tempUnitID_Near = 0;
                float tempDistance_Near = 0f;
                for (int i = 0; i < BattleManager.instance._playerUnitAIs.Count; i++)
                {
                    if (BattleManager.instance._playerUnitAIs[i] != this) //First make sure we are not checking ourself.
                    {
                        Transform comparePosition = _characters_Current[0].transform;
                        for (int j = 0; j < _unitStats.health_Current.Length; j++) //Find first alive character of this unit.
                        {
                            if (_unitStats.health_Current[j] > 0f)
                            {
                                comparePosition = _characters_Current[j].transform;
                                break;
                            }
                        }

                        Character_Movement[] tempUnitChars = BattleManager.instance._playerUnitAIs[i].GetCharacters().ToArray(); //Get all alive characters in unit.
                        for (int k = 0; k < tempUnitChars.Length; k++) //Compare distance to first alive character of this unit.
                        {
                            //Update tempunitID and tempdistance if closer.
                            if (Vector3.Distance(comparePosition.position, tempUnitChars[k].transform.position) < tempDistance_Near) tempUnitID_Near = i;
                        }
                    }
                }
                if (tempDistance_Near == 0f) return null; //Only this unit is left alive.
                else return BattleManager.instance._playerUnitAIs[tempUnitID_Near];

            case Enum_Conditions.Farthest: //Check list of allies, remove self, and find farthest unit.
                int tempUnitID_Far = 0;
                float tempDistance_Far = 0f;
                for (int i = 0; i < BattleManager.instance._playerUnitAIs.Count; i++)
                {
                    if (BattleManager.instance._playerUnitAIs[i] != this) //First make sure we are not checking ourself.
                    {
                        Transform comparePosition = _characters_Current[0].transform;
                        for (int j = 0; j < _unitStats.health_Current.Length; j++) //Find first alive character of this unit.
                        {
                            if (_unitStats.health_Current[j] > 0f)
                            {
                                comparePosition = _characters_Current[j].transform;
                                break;
                            }
                        }

                        Character_Movement[] tempUnitChars = BattleManager.instance._playerUnitAIs[i].GetCharacters().ToArray(); //Get all alive characters in unit.
                        for (int k = 0; k < tempUnitChars.Length; k++) //Compare distance to first alive character of this unit.
                        {
                            //Update tempunitID and tempdistance if closer.
                            if (Vector3.Distance(comparePosition.position, tempUnitChars[k].transform.position) > tempDistance_Far) tempUnitID_Far = i;
                        }
                    }
                }
                if (tempDistance_Far == 0f) return null; //Only this unit is left alive.
                else return BattleManager.instance._playerUnitAIs[tempUnitID_Far];

            default:
                return null;
        }
    }

    private Unit_AI GetValidEnemy(Enum_Conditions condition)
    {
        switch (condition)
        {
            case Enum_Conditions.HPHighest: //Check list of allies, remove self, and find unit with highest total HP.
                int tempUnitID_HPHigh = 0;
                float tempHP_High = 0;
                for (int i = 0; i < BattleManager.instance._enemyUnitAIs.Count; i++)
                {
                    if (BattleManager.instance._enemyUnitAIs[i] != this) //First make sure we are not checking ourself.
                    {
                        float tempTotalHP = BattleManager.instance._enemyUnitAIs[i]._unitStats.GetTotalHP();
                        if (tempTotalHP > 0f && tempTotalHP > tempHP_High)
                        {
                            tempUnitID_HPHigh = i;
                            tempHP_High = tempTotalHP;
                        }
                    }
                }
                if (tempHP_High == 0) return null; //Only this unit is left alive.
                else return BattleManager.instance._enemyUnitAIs[tempUnitID_HPHigh];

            case Enum_Conditions.HPLowest: //Check list of allies, remove self, and find unit with lowest total HP.
                int tempUnitID_HPLow = 0;
                float tempHP_Low = 0;
                for (int i = 0; i < BattleManager.instance._enemyUnitAIs.Count; i++)
                {
                    if (BattleManager.instance._enemyUnitAIs[i] != this) //First make sure we are not checking ourself.
                    {
                        float tempTotalHP = BattleManager.instance._enemyUnitAIs[i]._unitStats.GetTotalHP();
                        if (tempTotalHP > 0f && tempTotalHP < tempHP_Low)
                        {
                            tempUnitID_HPLow = i;
                            tempHP_Low = tempTotalHP;
                        }
                    }
                }
                if (tempHP_Low == 0) return null; //Only this unit is left alive.
                else return BattleManager.instance._enemyUnitAIs[tempUnitID_HPLow];

            case Enum_Conditions.Nearest: //Check list of allies, remove self, and find nearest unit.
                int tempUnitID_Near = 0;
                float tempDistance_Near = 0f;
                for (int i = 0; i < BattleManager.instance._enemyUnitAIs.Count; i++)
                {
                    if (BattleManager.instance._enemyUnitAIs[i] != this) //First make sure we are not checking ourself.
                    {
                        Transform comparePosition = _characters_Current[0].transform;
                        for (int j = 0; j < _unitStats.health_Current.Length; j++) //Find first alive character of this unit.
                        {
                            if (_unitStats.health_Current[j] > 0f)
                            {
                                comparePosition = _characters_Current[j].transform;
                                break;
                            }
                        }

                        Character_Movement[] tempUnitChars = BattleManager.instance._enemyUnitAIs[i].GetCharacters().ToArray(); //Get all alive characters in unit.
                        for (int k = 0; k < tempUnitChars.Length; k++) //Compare distance to first alive character of this unit.
                        {
                            //Update tempunitID and tempdistance if closer.
                            if (Vector3.Distance(comparePosition.position, tempUnitChars[k].transform.position) < tempDistance_Near) tempUnitID_Near = i;
                        }
                    }
                }
                if (tempDistance_Near == 0f) return null; //Only this unit is left alive.
                else return BattleManager.instance._enemyUnitAIs[tempUnitID_Near];

            case Enum_Conditions.Farthest: //Check list of allies, remove self, and find farthest unit.
                int tempUnitID_Far = 0;
                float tempDistance_Far = 0f;
                for (int i = 0; i < BattleManager.instance._enemyUnitAIs.Count; i++)
                {
                    if (BattleManager.instance._enemyUnitAIs[i] != this) //First make sure we are not checking ourself.
                    {
                        Transform comparePosition = _characters_Current[0].transform;
                        for (int j = 0; j < _unitStats.health_Current.Length; j++) //Find first alive character of this unit.
                        {
                            if (_unitStats.health_Current[j] > 0f)
                            {
                                comparePosition = _characters_Current[j].transform;
                                break;
                            }
                        }

                        Character_Movement[] tempUnitChars = BattleManager.instance._enemyUnitAIs[i].GetCharacters().ToArray(); //Get all alive characters in unit.
                        for (int k = 0; k < tempUnitChars.Length; k++) //Compare distance to first alive character of this unit.
                        {
                            //Update tempunitID and tempdistance if closer.
                            if (Vector3.Distance(comparePosition.position, tempUnitChars[k].transform.position) > tempDistance_Far) tempUnitID_Far = i;
                        }
                    }
                }
                if (tempDistance_Far == 0f) return null; //Only this unit is left alive.
                else return BattleManager.instance._enemyUnitAIs[tempUnitID_Far];

            default:
                return null;
        }
    }

    private Unit_AI GetValidSelf(Enum_Conditions condition)
    {
        switch (condition)
        {
            case Enum_Conditions.HPHighest:
                return this;
            case Enum_Conditions.HPLowest:
                return this;
            case Enum_Conditions.Nearest:
                return this;
            case Enum_Conditions.Farthest:
                return this;
            default:
                return null;
        }
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////CONTINNUE HERE
    private bool ValidateActionRange(Enum_Actions action, Unit_AI targetUnit) //Check if target is within action range for validated behaviour.
    {
        //Find nearest characters between units.
        //Check distance against action range.

        return true;
    }

    #endregion

    #region Movement

    private void MoveUnitToTarget(Unit_AI targetUnit) //Called if target is outside if action range.
    {
        //Find nearest characters between units.
        //Set movement destination to that character's position.
        //Start movement.
    }

    private void StopUnitMovement() //Call to stop unit when within action range (might not be necessary, depending on if we want movement + action).
    {
        //Stop movement of unit, and let characters catch up to their squad positions.
    }

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
