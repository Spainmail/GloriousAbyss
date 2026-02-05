using System;
using System.Collections.Generic;
using System.Xml;
using UnityEditor;
using UnityEngine;

public class Unit_AI : MonoBehaviour
{
    [Header("Unit Parameters")]
    [SerializeField] public bool _isEnemy;
    [SerializeField] private Model_Unit _unitStats;                                                 //This reference is set on unit instantiation.
    [SerializeField] private float _decisionInterval_Current;
    [SerializeField] private List<Character_Movement> _characterMovers;
    [SerializeField] private List<Character_Action> _characterActions;

    private bool _validating;

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
        if (!BattleManager.instance._battleActive || _validating == true) return;
                                                                                                        //TO DO: Check if all characters in unit dead.

        if (_decisionInterval_Current > 0f)
        {
            _decisionInterval_Current -= Time.deltaTime;
        }
        else //Time to make a decision.                                                                 //TO DO: Check if any characters in unit alive.
        {
            _validating = true;
            ValidateBehaviour();
        }
    }

    private void ValidateBehaviour()
    {
        if (Debug.isDebugBuild && _unitStats.name == "Unit Beta") Debug.Log(transform.parent.name + " making a decision!");
        for (int i = 0; i < _unitStats.behaviourCurrent.Count; i++) //Check behaviours one after one.
        {
            if (Debug.isDebugBuild && _unitStats.name == "Unit Beta") Debug.Log("Validating target " + _unitStats.behaviourCurrent[i].target + " and condition " + _unitStats.behaviourCurrent[i].condition);
            if (Debug.isDebugBuild && _unitStats.name == "Unit Beta") Debug.Log(GetValidTarget(_unitStats.behaviourCurrent[i].target, _unitStats.behaviourCurrent[i].condition));
            if (GetValidTarget(_unitStats.behaviourCurrent[i].target, _unitStats.behaviourCurrent[i].condition) != null) //If null, check next behaviour.
            {
                //if (Debug.isDebugBuild && _unitStats.name == "Unit Beta") Debug.Log("Valid target for " + _unitStats.behaviourCurrent[i].target + " under condition " + _unitStats.behaviourCurrent[i].condition);
                Unit_AI tempTargetAI = GetValidTarget(_unitStats.behaviourCurrent[i].target, _unitStats.behaviourCurrent[i].condition); //Get valid target unit.
                Model_Action tempAction = GameParameters.instance.GetAction(_unitStats.behaviourCurrent[i].action.ToString()); //Get action data.
                //if (Debug.isDebugBuild && _unitStats.name == "Unit Beta") Debug.Log("Checking range to " + tempTargetAI.GetUnitName());
                if (ValidateRange_Unit(transform, tempTargetAI.transform, tempAction) == true) //Check range of action. If true, unit is within range.
                {
                    if (Debug.isDebugBuild && _unitStats.name == "Unit Beta") Debug.Log("Range validated for action " + tempAction.actionName);
                    ExecuteAction(tempAction, tempTargetAI, null);                        //TO DO: Select target character based on range calculation.
                    _decisionInterval_Current = GameParameters.instance.GetInterval() + tempAction.delay; //Add delay of action to decision interval.
                    _validating = false;
                    return; //Exit after first valid behaviour is processed.
                }
                else //Unit needs to be moved towards target to enter action range.
                {
                    if (Debug.isDebugBuild && _unitStats.name == "Unit Beta") Debug.Log("Target " + tempTargetAI.GetUnitName() + " outside range for " + _unitStats.name + " (" + tempAction.actionName + ").");
                    MoveUnitToTarget(tempTargetAI, tempAction);
                    _decisionInterval_Current = GameParameters.instance.GetInterval();
                    _validating = false;
                    return; //Exit after first valid behaviour is processed.
                }
            }
        }

        _decisionInterval_Current = GameParameters.instance.GetInterval();
        _validating = false;
    }

    #region Behaviour Checks

    public Unit_AI GetValidTarget(Enum_Targets targetType, Enum_Conditions condition)
    {
        switch (targetType)
        {
            case Enum_Targets.Ally:
                return GetValidAlly(condition);
            case Enum_Targets.Enemy:
                //if (Debug.isDebugBuild && _unitStats.name == "Unit Beta") Debug.Log(_unitStats.name + " checking for nearest enemy.");
                return GetValidEnemy(condition);
            case Enum_Targets.Self:
                return GetValidSelf(condition);
            default:
                return null;
        }
    }

    public List<Unit_AI> ValidateEnemyList() //Return the list of OPPOSING team's AI.
    {
        if (_isEnemy) return BattleManager.instance._playerUnitAIs;
        else return BattleManager.instance._enemyUnitAIs;
    }

    public List<Unit_AI> ValidateAllyList() //Return the list of team's AI.
    {
        if (_isEnemy) return BattleManager.instance._enemyUnitAIs;
        else return BattleManager.instance._playerUnitAIs;
    }

    private Unit_AI GetValidAlly(Enum_Conditions condition)
    {
        switch (condition)
        {
            case Enum_Conditions.HPHighest: //Check list of allies, remove self, and find unit with highest total HP.
                int tempUnitID_HPHigh = 0;
                float tempHP_High = 0;
                for (int i = 0; i < ValidateAllyList().Count; i++)
                {
                    if (ValidateAllyList()[i] != this) //First make sure we are not checking ourself.
                    {
                        float tempTotalHP = ValidateAllyList()[i]._unitStats.GetTotalHP();
                        if (tempTotalHP > 0f && tempTotalHP > tempHP_High)
                        {
                            tempUnitID_HPHigh = i;
                            tempHP_High = tempTotalHP;
                        }
                    }
                }
                if (tempHP_High == 0) return null; //Only this unit is left alive.
                else return ValidateAllyList()[tempUnitID_HPHigh];

            case Enum_Conditions.HPLowest: //Check list of allies, remove self, and find unit with lowest total HP.
                int tempUnitID_HPLow = 0;
                float tempHP_Low = 0;
                for (int i = 0; i < ValidateAllyList().Count; i++)
                {
                    if (ValidateAllyList()[i] != this) //First make sure we are not checking ourself.
                    {
                        float tempTotalHP = ValidateAllyList()[i]._unitStats.GetTotalHP();
                        if (tempTotalHP > 0f && tempTotalHP < tempHP_Low)
                        {
                            tempUnitID_HPLow = i;
                            tempHP_Low = tempTotalHP;
                        }
                    }
                }
                if (tempHP_Low == 0) return null; //Only this unit is left alive.
                else return ValidateAllyList()[tempUnitID_HPLow];

            case Enum_Conditions.Nearest: //Check list of allies, remove self, and find nearest unit.
                int tempUnitID_Near = 0;
                float tempDistance_Near = 0f;
                for (int i = 0; i < ValidateAllyList().Count; i++)
                {
                    if (Debug.isDebugBuild && _unitStats.name == "Unit Beta") Debug.Log("Checking if " + ValidateAllyList()[i] + " is nearest ally.");
                    if (ValidateAllyList()[i] != this) //First make sure we are not checking ourself.
                    {
                        Transform comparePosition = _characterMovers[0].transform;
                        for (int j = 0; j < _unitStats.health_Current.Length; j++) //Find first alive character of this unit.
                        {
                            if (_unitStats.health_Current[j] > 0f)
                            {
                                comparePosition = _characterMovers[j].transform;
                                break;
                            }
                        }

                        Character_Movement[] tempUnitChars = ValidateAllyList()[i].GetCharacters().ToArray(); //Get all alive characters in unit.
                        for (int k = 0; k < tempUnitChars.Length; k++) //Compare distance to first alive character of this unit.
                        {
                            //Update tempunitID and tempdistance if closer.
                            float tempDistance = Vector3.Distance(comparePosition.position, tempUnitChars[k].transform.position);
                            //if (Debug.isDebugBuild) Debug.Log("Comparing distance between unit characters (" + tempDistance + ").");
                            if (tempDistance_Near == 0 || tempDistance < tempDistance_Near)
                            {
                                tempDistance_Near = tempDistance;
                                tempUnitID_Near = i;
                                //if (Debug.isDebugBuild) Debug.Log("Updated tempDistance_Near is now " + tempDistance_Near);
                            }
                                
                        }
                    }
                }
                //if (Debug.isDebugBuild) Debug.Log("Nearest ally is " + BattleManager.instance._playerUnitAIs[tempUnitID_Near] + " (" + tempDistance_Near + " away).");
                if (tempDistance_Near == 0f) return null; //Only this unit is left alive.
                else return ValidateAllyList()[tempUnitID_Near];

            case Enum_Conditions.Farthest: //Check list of allies, remove self, and find farthest unit.
                int tempUnitID_Far = 0;
                float tempDistance_Far = 0f;
                for (int i = 0; i < ValidateAllyList().Count; i++)
                {
                    if (ValidateAllyList()[i] != this) //First make sure we are not checking ourself.
                    {
                        Transform comparePosition = _characterMovers[0].transform;
                        for (int j = 0; j < _unitStats.health_Current.Length; j++) //Find first alive character of this unit.
                        {
                            if (_unitStats.health_Current[j] > 0f)
                            {
                                comparePosition = _characterMovers[j].transform;
                                break;
                            }
                        }

                        Character_Movement[] tempUnitChars = ValidateAllyList()[i].GetCharacters().ToArray(); //Get all alive characters in unit.
                        for (int k = 0; k < tempUnitChars.Length; k++) //Compare distance to first alive character of this unit.
                        {
                            //Update tempunitID and tempdistance if closer.
                            float tempDistance = Vector3.Distance(comparePosition.position, tempUnitChars[k].transform.position);
                            if (tempDistance > tempDistance_Far)
                            {
                                tempDistance_Far = tempDistance;
                                tempUnitID_Far = i;
                            }
                                
                        }
                    }
                }
                if (tempDistance_Far == 0f) return null; //Only this unit is left alive.
                else return ValidateAllyList()[tempUnitID_Far];

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
                for (int i = 0; i < ValidateEnemyList().Count; i++)
                {
                    if (ValidateEnemyList()[i] != this) //First make sure we are not checking ourself.
                    {
                        float tempTotalHP = ValidateEnemyList()[i]._unitStats.GetTotalHP();
                        if (tempTotalHP > 0f && tempTotalHP > tempHP_High)
                        {
                            tempUnitID_HPHigh = i;
                            tempHP_High = tempTotalHP;
                        }
                    }
                }
                if (tempHP_High == 0) return null; //Only this unit is left alive.
                else return ValidateEnemyList()[tempUnitID_HPHigh];

            case Enum_Conditions.HPLowest: //Check list of allies, remove self, and find unit with lowest total HP.
                int tempUnitID_HPLow = 0;
                float tempHP_Low = 0;
                for (int i = 0; i < ValidateEnemyList().Count; i++)
                {
                    if (ValidateEnemyList()[i] != this) //First make sure we are not checking ourself.
                    {
                        float tempTotalHP = ValidateEnemyList()[i]._unitStats.GetTotalHP();
                        if (tempTotalHP > 0f && tempTotalHP < tempHP_Low)
                        {
                            tempUnitID_HPLow = i;
                            tempHP_Low = tempTotalHP;
                        }
                    }
                }
                if (tempHP_Low == 0) return null; //Only this unit is left alive.
                else return ValidateEnemyList()[tempUnitID_HPLow];

            case Enum_Conditions.Nearest: //Check list of allies, remove self, and find nearest unit.
                int tempUnitID_Near = 0;
                float tempDistance_Near = 0f;
                for (int i = 0; i < ValidateEnemyList().Count; i++)
                {
                    if (ValidateEnemyList()[i] != this) //First make sure we are not checking ourself.
                    {
                        Transform comparePosition = _characterMovers[0].transform;
                        for (int j = 0; j < _unitStats.health_Current.Length; j++) //Find first alive character of this unit.
                        {
                            if (_unitStats.health_Current[j] > 0f)
                            {
                                comparePosition = _characterMovers[j].transform;
                                break;
                            }
                        }

                        Character_Movement[] tempUnitChars = ValidateEnemyList()[i].GetCharacters().ToArray(); //Get all alive characters in unit.
                        for (int k = 0; k < tempUnitChars.Length; k++) //Compare distance to first alive character of this unit.
                        {
                            //Update tempunitID and tempdistance if closer.
                            float tempDistance = Vector3.Distance(comparePosition.position, tempUnitChars[k].transform.position);
                            if (tempDistance_Near == 0 || tempDistance < tempDistance_Near)
                            {
                                tempDistance_Near = tempDistance;
                                tempUnitID_Near = i;
                            }
                        }
                    }
                }
                //if (Debug.isDebugBuild && _unitStats.name == "Unit Beta") Debug.Log("Returning enemy " + BattleManager.instance._enemyUnitAIs[tempUnitID_Near].GetUnitName());
                if (tempDistance_Near == 0f) return null; //Only this unit is left alive.
                else if (_isEnemy) return BattleManager.instance._playerUnitAIs[tempUnitID_Near];
                else return BattleManager.instance._enemyUnitAIs[tempUnitID_Near];

            case Enum_Conditions.Farthest: //Check list of allies, remove self, and find farthest unit.
                int tempUnitID_Far = 0;
                float tempDistance_Far = 0f;
                for (int i = 0; i < ValidateEnemyList().Count; i++)
                {
                    if (ValidateEnemyList()[i] != this) //First make sure we are not checking ourself.
                    {
                        Transform comparePosition = _characterMovers[0].transform;
                        for (int j = 0; j < _unitStats.health_Current.Length; j++) //Find first alive character of this unit.
                        {
                            if (_unitStats.health_Current[j] > 0f)
                            {
                                comparePosition = _characterMovers[j].transform;
                                break;
                            }
                        }

                        Character_Movement[] tempUnitChars = ValidateEnemyList()[i].GetCharacters().ToArray(); //Get all alive characters in unit.
                        for (int k = 0; k < tempUnitChars.Length; k++) //Compare distance to first alive character of this unit.
                        {
                            //Update tempunitID and tempdistance if closer.
                            float tempDistance = Vector3.Distance(comparePosition.position, tempUnitChars[k].transform.position);
                            if (tempDistance > tempDistance_Far)
                            {
                                tempDistance_Far = tempDistance;
                                tempUnitID_Near = i;
                            }
                        }
                    }
                }
                if (tempDistance_Far == 0f) return null; //Only this unit is left alive.
                else return ValidateEnemyList()[tempUnitID_Far];

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

    private bool ValidateRange_Unit(Transform thisUnit, Transform targetUnit, Model_Action action)
    {
        bool tempBool = false;
        if (Vector3.Distance(thisUnit.position, targetUnit.position) <= action.rangeUnit) tempBool = true;
        return tempBool;
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////CONTINNUE HERE
    private bool ValidateRange_Character(Enum_Actions action, Unit_AI targetUnit) //Check if target is within action range for validated behaviour.
    {
        //Find nearest characters between units.
        //Check distance against action range.

        return true;
    }

    private Character_Movement DetermineCharacterTarget()
    {
        

        return null;
    }

    #endregion

    #region Movement

    private void MoveUnitToTarget(Unit_AI targetUnit, Model_Action action) //Called if target is outside if action range.
    {
        if (Debug.isDebugBuild && _unitStats.name == "Unit Beta") Debug.Log("Moving unit within action range.");
        _movement.Move_Start(targetUnit.gameObject, action.rangeUnit); //Start movement.
    }

    private void StopUnitMovement() //Call to stop unit when within action range (might not be necessary, depending on if we want movement + action).
    {
        //Stop movement of unit, and let characters catch up to their squad positions.
    }

    #endregion

    #region Actions

    public void ExecuteAction(Model_Action action, Unit_AI targetAI, Character_Movement targetCharacter)
    {
        if (action == GameParameters.instance.Actions[0]) Action_Follow();
        else if (action == GameParameters.instance.Actions[1]) Action_Bow();
        else if (action == GameParameters.instance.Actions[2]) Action_Handgun(action, targetAI, null);
        else if (action == GameParameters.instance.Actions[3]) Action_Rifle();
        else if (action == GameParameters.instance.Actions[4]) Action_Javelin();
    }


    private void Action_Follow()
    {
        //Don't do anything, just wait and see if movement needs to be updated to continue following target next interval.
    }

    private void Action_Bow()
    {
        //Particles, sound, prefabs, etc.
        //Calculate damage, apply to target unit's character.
    }

    private void Action_Handgun(Model_Action action, Unit_AI targetUnit, Character_Movement targetCharacter)
    {
        for (int i = 0; i < _characterActions.Count; i++)
        {
            if (_unitStats.health_Current[i] > 0) _characterActions[i].Action_Handgun(targetUnit);
        }
    }

    private void Action_Rifle()
    {
        throw new NotImplementedException();
    }

    private void Action_Javelin()
    {
        throw new NotImplementedException();
    }

    #endregion

    #region GetData Functions

    public string GetUnitName()
    {
        return _unitStats.name;
    }

    public List<Character_Movement> GetCharacters()
    {
        return _characterMovers;
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
