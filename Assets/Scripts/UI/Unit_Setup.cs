using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Unit_Setup : MonoBehaviour
{
    [Header("Behavior Setup")]
    public int _currentUnit;
    public Model_Unit[] _tempSquad;
    public Model_Behaviour[] _tempBehaviour = new Model_Behaviour[4];

    [Header("Unit Pane")]
    public TextMeshProUGUI _unitName;
    public Image[] _unitColorables;
    public Color[] _unitColors;
    #region Dropdowns
    public TMP_Dropdown _targetDropdown_0;
    public TMP_Dropdown _conditionDropdown_0;
    public TMP_Dropdown _actionDropdown_0;
    public TMP_Dropdown _targetDropdown_1;
    public TMP_Dropdown _conditionDropdown_1;
    public TMP_Dropdown _actionDropdown_1;
    public TMP_Dropdown _targetDropdown_2;
    public TMP_Dropdown _conditionDropdown_2;
    public TMP_Dropdown _actionDropdown_2;
    public TMP_Dropdown _targetDropdown_3;
    public TMP_Dropdown _conditionDropdown_3;
    public TMP_Dropdown _actionDropdown_3;
    #endregion
    [Header("Inventory")]
    public List<Enum_Targets> _inventoryTargets;
    public List<Enum_Conditions> _inventoryConditions;
    public List<Enum_Actions> _inventoryActions;

    [Header("Audio")]
    public AudioClip _clipButton;
    private AudioSource _source;

    [Header("Debug")]
    public bool _debugInventory;

    private void Start()
    {
        _tempSquad = DataManager.instance.GetCurrentSquad();
        _source = GetComponent<AudioSource>();
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
        _tempBehaviour[0] = _tempSquad[_currentUnit].behaviourCurrent[0];
        _tempBehaviour[1] = _tempSquad[_currentUnit].behaviourCurrent[1];
        _tempBehaviour[2] = _tempSquad[_currentUnit].behaviourCurrent[2];
        _tempBehaviour[3] = _tempSquad[_currentUnit].behaviourCurrent[3];

        for (int i = 0; i < _unitColorables.Length; i++) //Set unit colors.
        {
            _unitColorables[i].color = _unitColors[_currentUnit];
        }

        #region Behaviour 0
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
        #endregion

        #region Behaviour 1
        foreach (TMP_Dropdown.OptionData data in _targetDropdown_1.options)
        {
            if (data.text == _tempSquad[_currentUnit].behaviourCurrent[1].target.ToString())
            {
                if (Debug.isDebugBuild) Debug.Log("Setting behaviour target of unit " + _currentUnit + " to " + data.text);
                _targetDropdown_1.value = _targetDropdown_1.options.IndexOf(data);
            }
        }

        foreach (TMP_Dropdown.OptionData data in _conditionDropdown_1.options)
        {
            if (data.text == _tempSquad[_currentUnit].behaviourCurrent[1].condition.ToString())
            {
                if (Debug.isDebugBuild) Debug.Log("Setting behaviour condition of unit " + _currentUnit + " to " + data.text);
                _conditionDropdown_1.value = _conditionDropdown_1.options.IndexOf(data);
            }
        }

        foreach (TMP_Dropdown.OptionData data in _actionDropdown_1.options)
        {
            if (data.text == _tempSquad[_currentUnit].behaviourCurrent[1].action.ToString())
            {
                if (Debug.isDebugBuild) Debug.Log("Setting behaviour action of unit " + _currentUnit + " to " + data.text);
                _actionDropdown_1.value = _actionDropdown_1.options.IndexOf(data);
            }
        }
        #endregion

        #region Behaviour 2
        foreach (TMP_Dropdown.OptionData data in _targetDropdown_2.options)
        {
            if (data.text == _tempSquad[_currentUnit].behaviourCurrent[2].target.ToString())
            {
                if (Debug.isDebugBuild) Debug.Log("Setting behaviour target of unit " + _currentUnit + " to " + data.text);
                _targetDropdown_2.value = _targetDropdown_2.options.IndexOf(data);
            }
        }

        foreach (TMP_Dropdown.OptionData data in _conditionDropdown_2.options)
        {
            if (data.text == _tempSquad[_currentUnit].behaviourCurrent[2].condition.ToString())
            {
                if (Debug.isDebugBuild) Debug.Log("Setting behaviour condition of unit " + _currentUnit + " to " + data.text);
                _conditionDropdown_2.value = _conditionDropdown_2.options.IndexOf(data);
            }
        }

        foreach (TMP_Dropdown.OptionData data in _actionDropdown_2.options)
        {
            if (data.text == _tempSquad[_currentUnit].behaviourCurrent[2].action.ToString())
            {
                if (Debug.isDebugBuild) Debug.Log("Setting behaviour action of unit " + _currentUnit + " to " + data.text);
                _actionDropdown_2.value = _actionDropdown_2.options.IndexOf(data);
            }
        }
        #endregion

        #region Behaviour 3
        foreach (TMP_Dropdown.OptionData data in _targetDropdown_3.options)
        {
            if (data.text == _tempSquad[_currentUnit].behaviourCurrent[3].target.ToString())
            {
                if (Debug.isDebugBuild) Debug.Log("Setting behaviour target of unit " + _currentUnit + " to " + data.text);
                _targetDropdown_3.value = _targetDropdown_3.options.IndexOf(data);
            }
        }

        foreach (TMP_Dropdown.OptionData data in _conditionDropdown_3.options)
        {
            if (data.text == _tempSquad[_currentUnit].behaviourCurrent[3].condition.ToString())
            {
                if (Debug.isDebugBuild) Debug.Log("Setting behaviour condition of unit " + _currentUnit + " to " + data.text);
                _conditionDropdown_3.value = _conditionDropdown_3.options.IndexOf(data);
            }
        }

        foreach (TMP_Dropdown.OptionData data in _actionDropdown_3.options)
        {
            if (data.text == _tempSquad[_currentUnit].behaviourCurrent[3].action.ToString())
            {
                if (Debug.isDebugBuild) Debug.Log("Setting behaviour action of unit " + _currentUnit + " to " + data.text);
                _actionDropdown_3.value = _actionDropdown_3.options.IndexOf(data);
            }
        }
        #endregion
    }

    #region UI Elements

    public void PlayButtonSound()
    {
        _source.clip = _clipButton;
        _source.Play();
    }

    public void ShowProperties_Action(int whichAction) //Show action properties in inventory "modal" section.
    {
        //Populate item description pane with appropriate data/assets.
    }

    private void PopulateDropdowns()
    {
        _targetDropdown_0.ClearOptions();
        _conditionDropdown_0.ClearOptions();
        _actionDropdown_0.ClearOptions();
        _targetDropdown_1.ClearOptions();
        _conditionDropdown_1.ClearOptions();
        _actionDropdown_1.ClearOptions();
        _targetDropdown_2.ClearOptions();
        _conditionDropdown_2.ClearOptions();
        _actionDropdown_2.ClearOptions();
        _targetDropdown_3.ClearOptions();
        _conditionDropdown_3.ClearOptions();
        _actionDropdown_3.ClearOptions();

        foreach (Enum_Targets target in _inventoryTargets)
        {
            _targetDropdown_0.options.Add(new TMP_Dropdown.OptionData(target.ToString()));
        }

        foreach (Enum_Conditions condition in _inventoryConditions)
        {
            _conditionDropdown_0.options.Add(new TMP_Dropdown.OptionData(condition.ToString()));
        }

        foreach (Enum_Actions action in _inventoryActions)
        {
            _actionDropdown_0.options.Add(new TMP_Dropdown.OptionData(action.ToString()));
        }

        foreach (Enum_Targets target in _inventoryTargets)
        {
            _targetDropdown_1.options.Add(new TMP_Dropdown.OptionData(target.ToString()));
        }

        foreach (Enum_Conditions condition in _inventoryConditions)
        {
            _conditionDropdown_1.options.Add(new TMP_Dropdown.OptionData(condition.ToString()));
        }

        foreach (Enum_Actions action in _inventoryActions)
        {
            _actionDropdown_1.options.Add(new TMP_Dropdown.OptionData(action.ToString()));
        }

        foreach (Enum_Targets target in _inventoryTargets)
        {
            _targetDropdown_2.options.Add(new TMP_Dropdown.OptionData(target.ToString()));
        }

        foreach (Enum_Conditions condition in _inventoryConditions)
        {
            _conditionDropdown_2.options.Add(new TMP_Dropdown.OptionData(condition.ToString()));
        }

        foreach (Enum_Actions action in _inventoryActions)
        {
            _actionDropdown_2.options.Add(new TMP_Dropdown.OptionData(action.ToString()));
        }

        foreach (Enum_Targets target in _inventoryTargets)
        {
            _targetDropdown_3.options.Add(new TMP_Dropdown.OptionData(target.ToString()));
        }

        foreach (Enum_Conditions condition in _inventoryConditions)
        {
            _conditionDropdown_3.options.Add(new TMP_Dropdown.OptionData(condition.ToString()));
        }

        foreach (Enum_Actions action in _inventoryActions)
        {
            _actionDropdown_3.options.Add(new TMP_Dropdown.OptionData(action.ToString()));
        }

        UnitToEdit(0);
    }

    #endregion

    #region Behavior 0

    public void Behaviour0_UpdateTarget(int newValue)
    {
        switch (_targetDropdown_0.options[newValue].text)
        {
            case "Ally":
                _tempBehaviour[0].target = Enum_Targets.Ally;
                break;
            case "Enemy":
                _tempBehaviour[0].target = Enum_Targets.Enemy;
                break;
            case "Self":
                _tempBehaviour[0].target = Enum_Targets.Self;
                break;
        }


        //string currentTargetName = _targetDropdown_0.options[newValue].ToString();
        //string[] enumNames = Enum.GetNames(typeof(Enum_Targets));
        //for (int i = 0; i < enumNames.Length; i++)
        //{
        //    if (currentTargetName == enumNames[i]) _tempBehaviour.target = (Enum_Targets)i;
        //}

        Behaviour0_SaveOnUpdate();
    }

    public void Behaviour0_UpdateCondition(int newValue)
    {
        switch (_conditionDropdown_0.options[newValue].text)
        {
            case "Nearest":
                _tempBehaviour[0].condition = Enum_Conditions.Nearest;
                break;
            case "Farthest":
                _tempBehaviour[0].condition = Enum_Conditions.Farthest;
                break;
            case "HPLowest":
                _tempBehaviour[0].condition = Enum_Conditions.HPLowest;
                break;
            case "HPHighest":
                _tempBehaviour[0].condition = Enum_Conditions.HPHighest;
                break;
        }

        //string currentConditionName = _conditionDropdown_0.options[newValue].ToString();
        //string[] enumNames = Enum.GetNames(typeof(Enum_Conditions));
        //for (int i = 0; i < enumNames.Length; i++)
        //{
        //    if (currentConditionName == enumNames[i]) _tempBehaviour.condition = (Enum_Conditions)i;
        //}

        Behaviour0_SaveOnUpdate();
    }

    public void Behaviour0_UpdateAction(int newValue)
    {
        switch (_actionDropdown_0.options[newValue].text)
        {
            case "Follow":
                _tempBehaviour[0].action = Enum_Actions.Follow;
                break;
            case "Bow":
                _tempBehaviour[0].action = Enum_Actions.Bow;
                break;
            case "Handgun":
                _tempBehaviour[0].action = Enum_Actions.Handgun;
                break;
            case "Rifle":
                _tempBehaviour[0].action = Enum_Actions.Rifle;
                break;
            case "Javelin":
                _tempBehaviour[0].action = Enum_Actions.Javelin;
                break;
        }

        //string currentActionName = _actionDropdown_0.options[newValue].ToString();
        //string[] enumNames = Enum.GetNames(typeof(Enum_Actions));
        //for (int i = 0; i < enumNames.Length; i++)
        //{
        //    if (currentActionName == enumNames[i]) _tempBehaviour.action = (Enum_Actions)i;
        //}

        Behaviour0_SaveOnUpdate();
    }

    [ContextMenu("Save behaviour 0")]
    private void Behaviour0_SaveOnUpdate()
    {
        if (Debug.isDebugBuild) Debug.Log("Saving unit " + _currentUnit + " behaviour as " + _tempBehaviour[0].target + " " + _tempBehaviour[0].condition + " " + _tempBehaviour[0].action);
        _tempSquad[_currentUnit].behaviourCurrent[0] = _tempBehaviour[0];
    }

    #endregion

    #region Behavior 1

    public void Behaviour1_UpdateTarget(int newValue)
    {
        switch (_targetDropdown_1.options[newValue].text)
        {
            case "Ally":
                _tempBehaviour[1].target = Enum_Targets.Ally;
                break;
            case "Enemy":
                _tempBehaviour[1].target = Enum_Targets.Enemy;
                break;
            case "Self":
                _tempBehaviour[1].target = Enum_Targets.Self;
                break;
        }


        //string currentTargetName = _targetDropdown_0.options[newValue].ToString();
        //string[] enumNames = Enum.GetNames(typeof(Enum_Targets));
        //for (int i = 0; i < enumNames.Length; i++)
        //{
        //    if (currentTargetName == enumNames[i]) _tempBehaviour.target = (Enum_Targets)i;
        //}

        Behaviour1_SaveOnUpdate();
    }

    public void Behaviour1_UpdateCondition(int newValue)
    {
        switch (_conditionDropdown_1.options[newValue].text)
        {
            case "Nearest":
                _tempBehaviour[1].condition = Enum_Conditions.Nearest;
                break;
            case "Farthest":
                _tempBehaviour[1].condition = Enum_Conditions.Farthest;
                break;
            case "HPLowest":
                _tempBehaviour[1].condition = Enum_Conditions.HPLowest;
                break;
            case "HPHighest":
                _tempBehaviour[1].condition = Enum_Conditions.HPHighest;
                break;
        }

        //string currentConditionName = _conditionDropdown_0.options[newValue].ToString();
        //string[] enumNames = Enum.GetNames(typeof(Enum_Conditions));
        //for (int i = 0; i < enumNames.Length; i++)
        //{
        //    if (currentConditionName == enumNames[i]) _tempBehaviour.condition = (Enum_Conditions)i;
        //}

        Behaviour1_SaveOnUpdate();
    }

    public void Behaviour1_UpdateAction(int newValue)
    {
        switch (_actionDropdown_1.options[newValue].text)
        {
            case "Follow":
                _tempBehaviour[1].action = Enum_Actions.Follow;
                break;
            case "Bow":
                _tempBehaviour[1].action = Enum_Actions.Bow;
                break;
            case "Handgun":
                _tempBehaviour[1].action = Enum_Actions.Handgun;
                break;
            case "Rifle":
                _tempBehaviour[1].action = Enum_Actions.Rifle;
                break;
            case "Javelin":
                _tempBehaviour[1].action = Enum_Actions.Javelin;
                break;
        }

        //string currentActionName = _actionDropdown_0.options[newValue].ToString();
        //string[] enumNames = Enum.GetNames(typeof(Enum_Actions));
        //for (int i = 0; i < enumNames.Length; i++)
        //{
        //    if (currentActionName == enumNames[i]) _tempBehaviour.action = (Enum_Actions)i;
        //}

        Behaviour1_SaveOnUpdate();
    }

    private void Behaviour1_SaveOnUpdate()
    {
        _tempSquad[_currentUnit].behaviourCurrent[1] = _tempBehaviour[1];
    }

    #endregion

    #region Behavior 2

    public void Behaviour2_UpdateTarget(int newValue)
    {
        switch (_targetDropdown_2.options[newValue].text)
        {
            case "Ally":
                _tempBehaviour[2].target = Enum_Targets.Ally;
                break;
            case "Enemy":
                _tempBehaviour[2].target = Enum_Targets.Enemy;
                break;
            case "Self":
                _tempBehaviour[2].target = Enum_Targets.Self;
                break;
        }


        //string currentTargetName = _targetDropdown_0.options[newValue].ToString();
        //string[] enumNames = Enum.GetNames(typeof(Enum_Targets));
        //for (int i = 0; i < enumNames.Length; i++)
        //{
        //    if (currentTargetName == enumNames[i]) _tempBehaviour.target = (Enum_Targets)i;
        //}

        Behaviour2_SaveOnUpdate();
    }

    public void Behaviour2_UpdateCondition(int newValue)
    {
        switch (_conditionDropdown_2.options[newValue].text)
        {
            case "Nearest":
                _tempBehaviour[2].condition = Enum_Conditions.Nearest;
                break;
            case "Farthest":
                _tempBehaviour[2].condition = Enum_Conditions.Farthest;
                break;
            case "HPLowest":
                _tempBehaviour[2].condition = Enum_Conditions.HPLowest;
                break;
            case "HPHighest":
                _tempBehaviour[2].condition = Enum_Conditions.HPHighest;
                break;
        }

        //string currentConditionName = _conditionDropdown_0.options[newValue].ToString();
        //string[] enumNames = Enum.GetNames(typeof(Enum_Conditions));
        //for (int i = 0; i < enumNames.Length; i++)
        //{
        //    if (currentConditionName == enumNames[i]) _tempBehaviour.condition = (Enum_Conditions)i;
        //}

        Behaviour2_SaveOnUpdate();
    }

    public void Behaviour2_UpdateAction(int newValue)
    {
        switch (_actionDropdown_2.options[newValue].text)
        {
            case "Follow":
                _tempBehaviour[2].action = Enum_Actions.Follow;
                break;
            case "Bow":
                _tempBehaviour[2].action = Enum_Actions.Bow;
                break;
            case "Handgun":
                _tempBehaviour[2].action = Enum_Actions.Handgun;
                break;
            case "Rifle":
                _tempBehaviour[2].action = Enum_Actions.Rifle;
                break;
            case "Javelin":
                _tempBehaviour[2].action = Enum_Actions.Javelin;
                break;
        }

        //string currentActionName = _actionDropdown_0.options[newValue].ToString();
        //string[] enumNames = Enum.GetNames(typeof(Enum_Actions));
        //for (int i = 0; i < enumNames.Length; i++)
        //{
        //    if (currentActionName == enumNames[i]) _tempBehaviour.action = (Enum_Actions)i;
        //}

        Behaviour2_SaveOnUpdate();
    }

    private void Behaviour2_SaveOnUpdate()
    {
        _tempSquad[_currentUnit].behaviourCurrent[2] = _tempBehaviour[2];
    }

    #endregion

    #region Behavior 3

    public void Behaviour3_UpdateTarget(int newValue)
    {
        switch (_targetDropdown_3.options[newValue].text)
        {
            case "Ally":
                _tempBehaviour[3].target = Enum_Targets.Ally;
                break;
            case "Enemy":
                _tempBehaviour[3].target = Enum_Targets.Enemy;
                break;
            case "Self":
                _tempBehaviour[3].target = Enum_Targets.Self;
                break;
        }


        //string currentTargetName = _targetDropdown_0.options[newValue].ToString();
        //string[] enumNames = Enum.GetNames(typeof(Enum_Targets));
        //for (int i = 0; i < enumNames.Length; i++)
        //{
        //    if (currentTargetName == enumNames[i]) _tempBehaviour.target = (Enum_Targets)i;
        //}

        Behaviour3_SaveOnUpdate();
    }

    public void Behaviour3_UpdateCondition(int newValue)
    {
        switch (_conditionDropdown_3.options[newValue].text)
        {
            case "Nearest":
                _tempBehaviour[3].condition = Enum_Conditions.Nearest;
                break;
            case "Farthest":
                _tempBehaviour[3].condition = Enum_Conditions.Farthest;
                break;
            case "HPLowest":
                _tempBehaviour[3].condition = Enum_Conditions.HPLowest;
                break;
            case "HPHighest":
                _tempBehaviour[3].condition = Enum_Conditions.HPHighest;
                break;
        }

        //string currentConditionName = _conditionDropdown_0.options[newValue].ToString();
        //string[] enumNames = Enum.GetNames(typeof(Enum_Conditions));
        //for (int i = 0; i < enumNames.Length; i++)
        //{
        //    if (currentConditionName == enumNames[i]) _tempBehaviour.condition = (Enum_Conditions)i;
        //}

        Behaviour3_SaveOnUpdate();
    }

    public void Behaviour3_UpdateAction(int newValue)
    {
        switch (_actionDropdown_3.options[newValue].text)
        {
            case "Follow":
                _tempBehaviour[3].action = Enum_Actions.Follow;
                break;
            case "Bow":
                _tempBehaviour[3].action = Enum_Actions.Bow;
                break;
            case "Handgun":
                _tempBehaviour[3].action = Enum_Actions.Handgun;
                break;
            case "Rifle":
                _tempBehaviour[3].action = Enum_Actions.Rifle;
                break;
            case "Javelin":
                _tempBehaviour[3].action = Enum_Actions.Javelin;
                break;
        }

        //string currentActionName = _actionDropdown_0.options[newValue].ToString();
        //string[] enumNames = Enum.GetNames(typeof(Enum_Actions));
        //for (int i = 0; i < enumNames.Length; i++)
        //{
        //    if (currentActionName == enumNames[i]) _tempBehaviour.action = (Enum_Actions)i;
        //}

        Behaviour3_SaveOnUpdate();
    }

    private void Behaviour3_SaveOnUpdate()
    {
        _tempSquad[_currentUnit].behaviourCurrent[3] = _tempBehaviour[3];
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
