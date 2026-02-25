using UnityEngine;

public class QuitButton : MonoBehaviour
{
    public void Button_Quit()
    {
        DataManager.instance.SaveGame(true);
    }
}
