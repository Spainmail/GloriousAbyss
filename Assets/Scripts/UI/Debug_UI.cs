using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;


public class Debug_UI : MonoBehaviour
{
    [Header("UI Elements")]
    public CanvasGroup _canvasGroup_Main;
    public Transform _mainMenu_Pos_Hidden;
    public Transform _mainMenu_Pos_Shown;
    public Image _menuArrow;

    public void Button_ToggleMenuExpand() //Pull out or push back in the main debug menu.
    {
        if (_canvasGroup_Main.interactable == true) //If debug menu is already being shown on-screen.
        {
            _menuArrow.transform.DOMove(_mainMenu_Pos_Hidden.position, 0.15f); //Push menu out.
            
            _canvasGroup_Main.interactable = false; 
        }
        else //If debug menu is currently hidden.
        {
            _menuArrow.transform.DOMove(_mainMenu_Pos_Shown.position, 0.15f); //Pull menu in.

            _canvasGroup_Main.interactable = true; //Let player interact with debug options.
        }
    }

    public void Button_TestMovement()
    {
        Button_ToggleMenuExpand(); //Close menu.
        BattleManager.instance.Debug_TestMovement(); //Start movement of all deployed units.
    }

    public void Button_ToggleMusic() //Toggle BGM sounds on/off.
    {
        BGMManager.instance.ToggleMute();
    }

    public void Button_MainMenu() //Return to main menu.
    {
        SceneManager.LoadScene("TitleScreen");
    }
}
