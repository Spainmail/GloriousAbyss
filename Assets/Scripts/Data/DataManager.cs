using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.SceneManagement;

public class DataManager : MonoBehaviour
{
    #region Temporary for course
    //[Header("TempForCourse")]
    //[SerializeField] private string stringToSave;
    //public int loadedValue;
    //public int valueIncrement = 2;

    //private void Start()
    //{
    //    if (PlayerPrefs.HasKey(stringToSave))
    //    {
    //        Debug.Log("Loading data.");
    //        loadedValue = PlayerPrefs.GetInt(stringToSave);
    //    }
    //}

    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.S))
    //    {
    //        Debug.Log("Saving data.");
    //        loadedValue += valueIncrement;
    //        PlayerPrefs.SetInt(stringToSave, loadedValue);
    //    }
    //    else if (Input.GetKeyDown(KeyCode.D))
    //    {
    //        Debug.Log("Deleting data.");
    //        PlayerPrefs.DeleteKey(stringToSave);
    //    }
    //}
    #endregion

    public static DataManager instance;

    [Header("Player Progress Data")]
    [SerializeField] private Model_PlayerData playerData_Loaded; //Loaded from game startup.
    [SerializeField] private Model_PlayerData playerData_Current; //Current, potentially unsaved data.

    [Header("Debugging")]
    [SerializeField] private bool _debug;
    [SerializeField] private bool _debugMessages;

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

    public void SaveGame(bool quit)
    {
        string saveData;
        if (SceneManager.GetActiveScene().name == "BattleScene") saveData = JsonUtility.ToJson(playerData_Loaded); //Save data as it was before battle started.
        else saveData = JsonUtility.ToJson(playerData_Current); //In any other scene, save data as it currently is.
        PlayerPrefs.SetString("playerData", saveData);

        if (quit) Application.Quit();
    }

    [ContextMenu("Test: Load player data.")]
    public bool LoadGame()
    {
        if (PlayerPrefs.HasKey("playerData"))
        {
            bool delaySceneLoad = false;
            string loadedData = PlayerPrefs.GetString("playerData");
            playerData_Loaded = JsonUtility.FromJson<Model_PlayerData>(loadedData);
            playerData_Current = JsonUtility.FromJson<Model_PlayerData>(loadedData);
            
            while (delaySceneLoad == false)
            {
                if (playerData_Loaded != null && playerData_Current != null) delaySceneLoad = true;
            }

            return true;
        }
        else 
        {
            return false;
        }
    }

    public void DeletePlayerData() //Permanently delete local player data.
    {
        if (PlayerPrefs.HasKey("playerData")) PlayerPrefs.DeleteKey("playerData");
        playerData_Current = null;
        playerData_Loaded = null;
    }

    public Model_Unit[] GetCurrentSquad()
    {
        if (playerData_Current == null) LoadGame();
        
        Model_Unit[] units = new Model_Unit[playerData_Current.units.Count];
        for (int i = 0; i < units.Length; i++)
        {
            units[i] = playerData_Current.units[i];
        }

        return units;
    }

    #region Debug
    [ContextMenu("Test: Create player data.")]
    public void Debug_CreatePlayerData()
    {
        Model_PlayerData newData = new Model_PlayerData();
        Model_Behaviour tempBehaviour = new Model_Behaviour();
        tempBehaviour.action = Enum_Actions.Handgun;
        tempBehaviour.target = Enum_Targets.Enemy;
        tempBehaviour.condition = Enum_Conditions.HPHighest;

        Model_Unit unit0 = new Model_Unit();
        unit0.name = "Unit Alpha";
        unit0.behaviourCurrent.Add(tempBehaviour);
        unit0.characters_Current = 5;
        unit0.characters_Max = 5;
        unit0.health_Max = new float[5] { 4, 4, 4, 4, 4 };
        unit0.health_Current = new float[5] { 4, 4, 4, 4, 4 };
        unit0.damage_Max = 2f;
        unit0.damage_Max = 1f;
        unit0.moveSpeed = 2.5f;
        unit0.range = 3f;

        Model_Unit unit1 = new Model_Unit();
        unit1.name = "Unit Beta";
        unit1.behaviourCurrent.Add(tempBehaviour);
        unit1.characters_Current = 5;
        unit1.characters_Max = 5;
        unit1.health_Max = new float[5] { 4, 4, 4, 4, 4 };
        unit1.health_Current = new float[5] { 4, 4, 4, 4, 4 };
        unit1.damage_Max = 2f;
        unit1.damage_Max = 1f;
        unit1.moveSpeed = 2.5f;
        unit1.range = 3f;

        Model_Unit unit2 = new Model_Unit();
        unit2.name = "The Bird Unit";
        unit2.behaviourCurrent.Add(tempBehaviour);
        unit2.characters_Current = 5;
        unit2.characters_Max = 5;
        unit2.health_Max = new float[5] { 4, 4, 4, 4, 4 };
        unit2.health_Current = new float[5] { 4, 4, 4, 4, 4 };
        unit2.damage_Max = 2f;
        unit2.damage_Max = 1f;
        unit2.moveSpeed = 2.5f;
        unit2.range = 3f;

        newData.units.Add(unit0);
        newData.units.Add(unit1);
        newData.units.Add(unit2);

        newData.currentBattle = 0;

        playerData_Current = newData;
        playerData_Loaded = newData;
    }

    [ContextMenu("Test: Save player data.")]
    public void Debug_SaveData()
    {
        SaveGame(false);
    }

    #endregion
}