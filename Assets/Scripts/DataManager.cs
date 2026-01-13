using UnityEngine;

public class DataManager : MonoBehaviour
{
    [Header("TempForCourse")]
    [SerializeField] private string stringToSave;
    public int loadedValue;
    public int valueIncrement = 2;

    private void Start()
    {
        if (PlayerPrefs.HasKey(stringToSave))
        {
            Debug.Log("Loading data.");
            loadedValue = PlayerPrefs.GetInt(stringToSave);
        }
    }

    void Update()
    {
        #region Temporary for course
        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("Saving data.");
            loadedValue += valueIncrement;
            PlayerPrefs.SetInt(stringToSave, loadedValue);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log("Deleting data.");
            PlayerPrefs.DeleteKey(stringToSave);
        }
        #endregion
    }
}
