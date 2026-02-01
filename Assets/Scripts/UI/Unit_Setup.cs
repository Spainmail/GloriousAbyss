using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.UI;

public class Unit_Setup : MonoBehaviour
{
    [Header("Behavior Setup")]
    public int _currentUnit;
    public Model_Unit[] _tempSquad;
    public Model_Behaviour _tempBehaviour;

    [Header("Unit Pane")]
    public TextMeshProUGUI _unitName;
    public TMP_Dropdown _targetDropdown_0;
    public TMP_Dropdown _conditionDropdown_0;
    public TMP_Dropdown _actionDropdown_0;

    [Header("Inventory")]
    public List<Enum_Targets> _inventoryTargets;
    public List<Enum_Conditions> _inventoryConditions;
    public List<Enum_Actions> _inventoryActions;

    [Header("Debug")]
    public bool _debugInventory;

    private void Start()
    {
        _tempSquad = DataManager.instance.GetCurrentSquad();
        PopulateInventory();
    }

    private void PopulateInventory()
    {
        if (_debugInventory)
        {
            Debug_Inventory();
            return;
        }

        //Actually load inventory from DataManager.
    }

    public void UnitToEdit(int whichUnit) //Set which unit is being edited, and populate unit pane accordingly.
    {
        _currentUnit = whichUnit;

        _unitName.text = _tempSquad[_currentUnit].name; //Set unit name.

        foreach (TMP_Dropdown.OptionData data in _targetDropdown_0.options)
        {
            if (data.text == _tempSquad[_currentUnit].behaviourCurrent[0].target.ToString())
            {
                if (Debug.isDebugBuild) Debug.Log("Setting behaviour target of unit " + _currentUnit + " to " + data.text);
                _targetDropdown_0.value = _targetDropdown_0.options.IndexOf(data);
            }
        }

        foreach (TMP_Dropdown.OptionData data in _conditionDropdown_0.options)
        {
            if (data.text == _tempSquad[_currentUnit].behaviourCurrent[0].condition.ToString())
            {
                if (Debug.isDebugBuild) Debug.Log("Setting behaviour condition of unit " + _currentUnit + " to " + data.text);
                _conditionDropdown_0.value = _conditionDropdown_0.options.IndexOf(data);
            }
        }

        foreach (TMP_Dropdown.OptionData data in _actionDropdown_0.options)
        {
            if (data.text == _tempSquad[_currentUnit].behaviourCurrent[0].action.ToString())
            {
                if (Debug.isDebugBuild) Debug.Log("Setting behaviour action of unit " + _currentUnit + " to " + data.text);
                _actionDropdown_0.value = _actionDropdown_0.options.IndexOf(data);
            }
        }
    }

    #region UI Elements

    public void ShowProperties_Action(int whichAction) //Show action properties in inventory "modal" section.
    {
        //Populate item description pane with appropriate data/assets.
    }

    private void PopulateDropdowns()
    {
        _targetDropdown_0.ClearOptions();
        foreach (Enum_Targets target in _inventoryTargets)
        {
            _targetDropdown_0.options.Add(new TMP_Dropdown.OptionData(target.ToString()));
        }

        _conditionDropdown_0.ClearOptions();
        foreach (Enum_Conditions condition in _inventoryConditions)
        {
            _conditionDropdown_0.options.Add(new TMP_Dropdown.OptionData(condition.ToString()));
        }

        _actionDropdown_0.ClearOptions();
        foreach (Enum_Actions action in _inventoryActions)
        {
            _actionDropdown_0.options.Add(new TMP_Dropdown.OptionData(action.ToString()));
        }
    }

    #endregion

    #region Behavior

    public void Behaviour0_UpdateTarget(int newValue)
    {
        switch (_targetDropdown_0.options[newValue].ToString())
        {
            case "Ally":
                _tempBehaviour.target = Enum_Targets.Ally;
                break;
            case "Enemy":
                _tempBehaviour.target = Enum_Targets.Enemy;
                break;
            case "Self":
                _tempBehaviour.target = Enum_Targets.Self;
                break;
        }
        Behaviour0_SaveOnUpdate();
    }

    public void Behaviour0_UpdateCondition(int newValue)
    {
        switch (_conditionDropdown_0.options[newValue].ToString())
        {
            case "Nearest":
                _tempBehaviour.condition = Enum_Conditions.Nearest;
                break;
            case "Farthest":
                _tempBehaviour.condition = Enum_Conditions.Farthest;
                break;
            case "Lowest HP":
                _tempBehaviour.condition = Enum_Conditions.HPLowest;
                break;
            case "Highest HP":
                _tempBehaviour.condition = Enum_Conditions.HPHighest;
                break;
        }
        Behaviour0_SaveOnUpdate();
    }

    public void Behaviour0_UpdateAction(int newValue)
    {
        switch (_actionDropdown_0.options[newValue].ToString())
        {
            case "Follow":
                _tempBehaviour.action = Enum_Actions.Follow;
                break;
            case "Bow":
                _tempBehaviour.action = Enum_Actions.Bow;
                break;
            case "Handgun":
                _tempBehaviour.action = Enum_Actions.Handgun;
                break;
            case "Rifle":
                _tempBehaviour.action = Enum_Actions.Rifle;
                break;
            case "Javelin":
                _tempBehaviour.action = Enum_Actions.Javelin;
                break;
        }
        Behaviour0_SaveOnUpdate();
    }

    private void Behaviour0_SaveOnUpdate()
    {
        _tempSquad[_currentUnit].behaviourCurrent[0] = _tempBehaviour;
    }

    #endregion

    public void Button_StartBattle() //Save squad and load battle scene.
    {
        DataManager.instance.SetCurrentSquad(_tempSquad, true);
    }

    #region Debug

    public void Debug_Inventory()
    {
        _inventoryTargets.Add(Enum_Targets.Enemy);
        _inventoryTargets.Add(Enum_Targets.Ally);
        _inventoryTargets.Add(Enum_Targets.Self);

        _inventoryConditions.Add(Enum_Conditions.Nearest);
        _inventoryConditions.Add(Enum_Conditions.Farthest);
        _inventoryConditions.Add(Enum_Conditions.HPLowest);
        _inventoryConditions.Add(Enum_Conditions.HPHighest);

        _inventoryActions.Add(Enum_Actions.Follow);
        _inventoryActions.Add(Enum_Actions.Bow);
        _inventoryActions.Add(Enum_Actions.Handgun);
        _inventoryActions.Add(Enum_Actions.Rifle);
        _inventoryActions.Add(Enum_Actions.Javelin);

        PopulateDropdowns();
    }

    [ContextMenu("Test Unit pane setup")]
    public void Debug_EditUnit1()
    {
        UnitToEdit(0);
    }

    #endregion
}
