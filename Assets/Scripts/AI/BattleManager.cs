using NUnit.Framework;
using System;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.UIElements;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;
    [Space(6)]
    public bool _battleActive;
    [Space(6)]
    public PreBattleUI _UI_PreBattle;
    public BattleUI _UI_Battle;

    [Header("Battle Participants")]
    public GameObject _playerUnitPrefab;
    public GameObject _enemyUnitPrefab;
    [Space(6)]
    public List<GameObject> _playerUnits;
    public List<Unit_AI> _playerUnitAIs;
    public List<GameObject> _enemyUnits;
    public List<Unit_AI> _enemyUnitAIs;
    public int _enemyUnitsAliveCurrent;
    public int _playerUnitsAliveCurrent;

    [Header("Level Parameters")]
    public GameObject _unitParent;
    public List<Transform> _playerSpawnpoints;
    public List<Transform> _enemySpawnpoints;

    [Header("Debug")]
    public bool _debugUnitSetup;
    public List<GameObject> _debugPlayerUnits;
    public List<GameObject> _debugEnemyUnits;
    public List<Transform> _debugPlayerSpawns;
    public List<Transform> _debugEnemySpawns;
    public Model_Unit _debugUnit0;
    public Model_Unit _debugUnit1;
    public Model_Unit _debugUnit2;
    public Model_Unit[] _debugUnits;

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

    private void Start()
    {
        if (_debugUnitSetup) Debug_UnitSetup();
        else SetupBattle();
    }

    public void SetupBattle() //Set up units and level.
    {
        //Populate level with terrain, destructibles, unit spawnpoints, key, and gate.
        PopulateLevel();

        //Load player squad from DataManager.
        Model_Unit[] units = DataManager.instance.GetCurrentSquad();
        if (Debug.isDebugBuild) Debug.Log("Player units this battle: " + units.Length);
        _playerUnitsAliveCurrent = units.Length;
        if (Debug.isDebugBuild) Debug.Log("Enemy units this battle: " + _debugUnits.Length);
        _enemyUnitsAliveCurrent = _debugUnits.Length;

        List<Transform> tempSpawns = new List<Transform>(); //Find spawn points for all units.
        for (int i = 0; i < units.Length; i++)
        {
            float tempInt = units[i].health_Max[0];
            units[i].health_Current[0] = tempInt;
            units[i].health_Current[1] = tempInt;
            units[i].health_Current[2] = tempInt;
            units[i].health_Current[3] = tempInt;
            units[i].health_Current[4] = tempInt;

            bool foundSpawnpoint = false;
            Transform tempTransform = _playerSpawnpoints[0];
            while (foundSpawnpoint == false)
            {
                tempTransform = _playerSpawnpoints[UnityEngine.Random.Range(0, _playerSpawnpoints.Count)]; //Get randomized spawnpoint.
                if (!tempSpawns.Contains(tempTransform)) foundSpawnpoint = true;
            }
            tempSpawns.Add(tempTransform); //Set to randomized result.
        }

        for (int i = 0; i < units.Length; i++) //Instantiate player units according to spawnpoint list.
        {
            GameObject playerUnit = Instantiate(_playerUnitPrefab, tempSpawns[i].position, Quaternion.identity, parent: _unitParent.transform);
            playerUnit.GetComponentInChildren<Unit_AI>().SetupComponents(false, units[i]);
            _playerUnits.Add(playerUnit);
            _playerUnitAIs.Add(playerUnit.GetComponentInChildren<Unit_AI>());
        }

        tempSpawns.Clear(); //Clear player spawns.
        for (int i = 0; i < _debugUnits.Length; i++) //Randomize enemy unit spawnpoints.
        {
            bool foundSpawnpoint = false;
            Transform tempTransform = _enemySpawnpoints[0];
            while (foundSpawnpoint == false)
            {
                tempTransform = _enemySpawnpoints[UnityEngine.Random.Range(0, _enemySpawnpoints.Count)]; //Get randomized spawnpoint.
                if (!tempSpawns.Contains(tempTransform)) foundSpawnpoint = true;
            }
            tempSpawns.Add(tempTransform); //Set to randomized result.
        }

        for (int i = 0; i < units.Length; i++) //Instantiate enemy units according to spawnpoint list.
        {
            GameObject enemyUnit = Instantiate(_enemyUnitPrefab, tempSpawns[i].position, Quaternion.identity, parent: _unitParent.transform);
            enemyUnit.GetComponentInChildren<Unit_AI>().SetupComponents(true, _debugUnits[i]);
            _enemyUnits.Add(enemyUnit);
            _enemyUnitAIs.Add(enemyUnit.GetComponentInChildren<Unit_AI>());
        }

        _battleActive = true; //Start battle.                                                               TO DO: Have player press button to start.
    }

    public bool PopulateLevel()
    {
        return true;
    }

    #region Game State

    public void RelayDeath(bool enemy) //Update unit counters and check if level is complete (game over / victory).
    {
        if (!enemy)
        {
            _playerUnitsAliveCurrent -= 1;
            if (_playerUnitsAliveCurrent <= 0) TriggerGameOver(); //Game Over.

        }
        else
        {
            _enemyUnitsAliveCurrent -= 1;
            if (_enemyUnitsAliveCurrent <= 0) TriggerVictory(); //Victory.
        }
    }

    private void TriggerVictory() //Show victory screen on battle UI canvas, stop player units from taking new actions.
    {
        _battleActive = false;
        _UI_Battle.Victory();
    }

    private void TriggerGameOver() //Show game over screen on battle UI canvas, stop enemy units from taking new actions.
    {
        _battleActive = false;
        _UI_Battle.GameOver();
    }

    #endregion

    #region Debug

    public void Debug_UnitSetup()
    {
        _debugPlayerUnits[0].SetActive(true);
        _playerUnits.Add(_debugPlayerUnits[0]);
        _playerUnitAIs.Add(_debugPlayerUnits[0].GetComponentInChildren<Unit_AI>());
        _debugPlayerUnits[1].SetActive(true);
        _playerUnits.Add(_debugPlayerUnits[1]);
        _playerUnitAIs.Add(_debugPlayerUnits[1].GetComponentInChildren<Unit_AI>());
        _debugPlayerUnits[2].SetActive(true);
        _playerUnits.Add(_debugPlayerUnits[2]);
        _playerUnitAIs.Add(_debugPlayerUnits[2].GetComponentInChildren<Unit_AI>());

        _debugEnemyUnits[0].SetActive(true);
        _enemyUnits.Add(_debugEnemyUnits[0]);
        _enemyUnitAIs.Add(_debugEnemyUnits[0].GetComponentInChildren<Unit_AI>());
        _enemyUnitAIs[0]._isEnemy = true;
        _debugEnemyUnits[1].SetActive(true);
        _enemyUnits.Add(_debugEnemyUnits[1]);
        _enemyUnitAIs.Add(_debugEnemyUnits[1].GetComponentInChildren<Unit_AI>());
        _enemyUnitAIs[1]._isEnemy = true;
        _debugEnemyUnits[2].SetActive(true);
        _enemyUnits.Add(_debugEnemyUnits[2]);
        _enemyUnitAIs.Add(_debugEnemyUnits[2].GetComponentInChildren<Unit_AI>());
        _enemyUnitAIs[2]._isEnemy = true;

        _battleActive = true;
    }

    public void Debug_EnemyUnitSetup()
    {
        _debugEnemyUnits[0].transform.position = _enemySpawnpoints[0].position;
        _debugEnemyUnits[0].SetActive(true);
        _enemyUnits.Add(_debugEnemyUnits[0]);
        _enemyUnitAIs.Add(_debugEnemyUnits[0].GetComponentInChildren<Unit_AI>());
        _enemyUnitAIs[0].SetupComponents(true, _debugUnit0);

        _debugEnemyUnits[1].transform.position = _enemySpawnpoints[1].position;
        _debugEnemyUnits[1].SetActive(true);
        _enemyUnits.Add(_debugEnemyUnits[1]);
        _enemyUnitAIs.Add(_debugEnemyUnits[1].GetComponentInChildren<Unit_AI>());
        _enemyUnitAIs[1].SetupComponents(true, _debugUnit1);

        _debugEnemyUnits[2].transform.position = _enemySpawnpoints[2].position;
        _debugEnemyUnits[2].SetActive(true);
        _enemyUnits.Add(_debugEnemyUnits[2]);
        _enemyUnitAIs.Add(_debugEnemyUnits[2].GetComponentInChildren<Unit_AI>());
        _enemyUnitAIs[2].SetupComponents(true, _debugUnit2);
    }

    public void Debug_TestMovement()
    {
        foreach (Unit_AI ai in _playerUnitAIs)
        {
            ai.Debug_Movement();
        }
        foreach (Unit_AI ai in _enemyUnitAIs)
        {
            ai.Debug_Movement();
        }
    }

    #endregion
}
