using UnityEngine;

public class Debug_Enabler : MonoBehaviour, IDebug
{
    public void ToggleActive() { this.enabled = !this.enabled; }

    public GameObject[] _objectsToEnable;

    private void Start()
    {
        foreach (GameObject obj in _objectsToEnable)
        {
            obj.SetActive(true);
        }
    }
}
