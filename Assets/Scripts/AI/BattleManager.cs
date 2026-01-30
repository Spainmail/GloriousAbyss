using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;
    [Space(6)]
    public bool _battleActive;

    [Header("Battle Participants")]
    public GameObject _playerUnitPrefab;
    public GameObject _enemyUnitPrefab;
    [Space(6)]
    public List<GameObject> _playerUnits;
    public List<Unit_AI> _playerUnitAIs;
    public List<GameObject> _enemyUnits;
    public List<Unit_AI> _enemyUnitAIs;

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
        for (int i = 0; i < units.Length; i++) //Instantiate player units at spawnpoints.
        {
            GameObject playerUnit = Instantiate(_playerUnitPrefab, _playerSpawnpoints[i].position, Quaternion.identity, parent: _unitParent.transform);
            playerUnit.GetComponentInChildren<Unit_AI>().SetupComponents(false, units[i]);
            _playerUnits.Add(playerUnit);
            _playerUnitAIs.Add(playerUnit.GetComponentInChildren<Unit_AI>());
        }

        Debug_EnemyUnitSetup(); //Set up enemy units according to spawnpoints.

        _battleActive = true; //Start battle.                                                               TO DO: Have player press button to start.
    }

    public bool PopulateLevel()
    {
        return true;
    }

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
        _debugEnemyUnits[1].SetActive(true);
        _enemyUnits.Add(_debugEnemyUnits[1]);
        _enemyUnitAIs.Add(_debugEnemyUnits[1].GetComponentInChildren<Unit_AI>());
        _debugEnemyUnits[2].SetActive(true);
        _enemyUnits.Add(_debugEnemyUnits[2]);
        _enemyUnitAIs.Add(_debugEnemyUnits[2].GetComponentInChildren<Unit_AI>());

        _battleActive = true;
    }

    public void Debug_EnemyUnitSetup()
    {
        _debugEnemyUnits[0].transform.position = _enemySpawnpoints[0].position;
        _debugEnemyUnits[0].SetActive(true);
        _enemyUnits.Add(_debugEnemyUnits[0]);
        _enemyUnitAIs.Add(_debugEnemyUnits[0].GetComponentInChildren<Unit_AI>());
        _enemyUnitAIs[0]._isEnemy = true;

        _debugEnemyUnits[1].transform.position = _enemySpawnpoints[1].position;
        _debugEnemyUnits[1].SetActive(true);
        _enemyUnits.Add(_debugEnemyUnits[1]);
        _enemyUnitAIs.Add(_debugEnemyUnits[1].GetComponentInChildren<Unit_AI>());
        _enemyUnitAIs[1]._isEnemy = true;
        
        _debugEnemyUnits[2].transform.position = _enemySpawnpoints[2].position;
        _debugEnemyUnits[2].SetActive(true);
        _enemyUnits.Add(_debugEnemyUnits[2]);
        _enemyUnitAIs.Add(_debugEnemyUnits[2].GetComponentInChildren<Unit_AI>());
        _enemyUnitAIs[2]._isEnemy = true;
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
