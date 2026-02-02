using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections.Generic;

public class TitleMenu : MonoBehaviour
{
    [Header("UI Elements")]
    public CanvasGroup _canvasGroup_Title;
    public Transform _titleObject;
    public float _title_TweenDuration;
    public float _title_StartScale;
    public float _title_EndScale = 1f;
    [Space(6)]
    public CanvasGroup _canvasGroup_Main;
    public List<GameObject> _mainButtons;
    public CanvasGroup[] _canvasGroups_MainButtons;
    public Transform[] _mainButton_AnchorsHidden;
    public Transform[] _mainButton_AnchorsShown;
    public float _mainButtons_TweenDuration;
    [Space(6)]
    public CanvasGroup _canvasGroup_Setting;
    public List<GameObject> _settingButtons;
    public CanvasGroup[] _canvasGroups_SettingButtons;
    public Transform[] _settingButton_AnchorsHidden;
    public Transform[] _settingButton_AnchorsShown;
    public float _settingButtons_TweenDuration;

    [Header("Scene Management")]
    public string _scene_Battle0;
    public string _scene_Interim0;

    [Header("Debugging")]
    [SerializeField] private bool _debugBattle;
    [SerializeField] private bool _debugMessages;
    [SerializeField] private bool _debugBGM;

    private void Start()
    {
        if (Debug.isDebugBuild && _debugBGM) BGMManager.instance.ToggleMute(); //Mute immediately if debugging.
        BGMManager.instance.StartNewTrack(0f, 0.5f, 1);
        //Load player settings.
        SetUpUI(); //Disable and reset all UI.
    }

    #region Main Menu

    public void SetUpUI()
    {
        _canvasGroup_Title.alpha = 0f;
        _titleObject.DOScale(_title_StartScale, 0f);

        _canvasGroup_Main.alpha = 0f;
        _canvasGroup_Main.interactable = false;
        Button[] tempButtons = _canvasGroup_Main.gameObject.GetComponentsInChildren<Button>();
        for (int i = 0; i < tempButtons.Length; i++) //Find buttons under main canvas group.
        {
            _mainButtons.Add(tempButtons[i].gameObject);
        }
        _canvasGroups_MainButtons = new CanvasGroup[_mainButtons.Count];
        for (int i = 0; i < _mainButtons.Count; i++)
        {
            _mainButtons[i].transform.DOMove(_mainButton_AnchorsHidden[i].position, 0f);
            _canvasGroups_MainButtons[i] = _mainButtons[i].GetComponent<CanvasGroup>();
        }

        _canvasGroup_Setting.alpha = 0f;
        _canvasGroup_Setting.interactable = false;
        tempButtons = _canvasGroup_Setting.gameObject.GetComponentsInChildren<Button>();
        for (int i = 0; i < tempButtons.Length; i++) //Find buttons under settings canvas group.
        {
            _settingButtons.Add(tempButtons[i].gameObject);
        }
        _canvasGroups_SettingButtons = new CanvasGroup[_settingButtons.Count];
        for (int i = 0; i < _settingButtons.Count; i++)
        {
            _settingButtons[i].transform.DOMove(_settingButton_AnchorsHidden[i].position, 0f);
            _canvasGroups_SettingButtons[i] = _settingButtons[i].GetComponent<CanvasGroup>();
        }

        StartCoroutine(AnimateTitle());
    }

    public IEnumerator AnimateTitle()
    {
        _canvasGroup_Title.DOFade(1f, _title_TweenDuration - 0.1f);
        _titleObject.DOScale(_title_EndScale, _title_TweenDuration);

        yield return new WaitForSeconds(0.5f);
        StartCoroutine(Toggle_MainMenu());
    }

    public IEnumerator Toggle_MainMenu()
    {
        if (_canvasGroup_Main.alpha == 0f) //Player requesting to show main menu.
        {
            if (_canvasGroup_Setting.alpha == 1f) //Settings menu needs to be disabled first.
            {
                StartCoroutine(Toggle_SettingsPane());
                yield return new WaitForSeconds(_settingButtons_TweenDuration * _settingButtons.Count);
            }

            _canvasGroup_Main.DOFade(1f, 0f);
            for (int i = 0; i < _mainButtons.Count; i++)
            {
                _canvasGroups_MainButtons[i].DOFade(1f, 0.1f);
                _mainButtons[i].transform.DOMove(_mainButton_AnchorsShown[i].position, _mainButtons_TweenDuration).SetEase(Ease.OutBack).SetDelay(i * 0.1f);
            }   

            yield return new WaitForSeconds((_mainButtons.Count * _mainButtons_TweenDuration) + 0.1f);
            _canvasGroup_Main.interactable = true;
        }
        else //Player requesting to hide main menu.
        {
            _canvasGroup_Main.interactable = false;

            for (int i = 0; i < _mainButtons.Count; i++)
            {
                _canvasGroups_MainButtons[i].DOFade(0f, 0.25f);
                _mainButtons[i].transform.DOMove(_mainButton_AnchorsHidden[i].position, _mainButtons_TweenDuration).SetEase(Ease.OutBack).SetDelay(i * 0.05f);
            }

            yield return new WaitForSeconds(_mainButtons.Count * _mainButtons_TweenDuration);
            _canvasGroup_Main.DOFade(0f, 0f);
        }
    }

    public void Button_StartGame()
    {
        if (_debugBattle)
        {
            SceneManager.LoadScene(_scene_Battle0);
            return;
        }

        bool gameInProgress = false;
        gameInProgress = DataManager.instance.LoadGame();
        if (gameInProgress == true)
        {
            SceneManager.LoadScene(_scene_Battle0);
        }
        else
        {
            SceneManager.LoadScene(_scene_Interim0);
        }
    }

    public void Button_Settings(GameObject obj)
    {
        Tween_PunchButton(obj.transform);
        StartCoroutine(Toggle_SettingsPane());
    }

    public IEnumerator Toggle_SettingsPane()
    {
        if (_canvasGroup_Setting.alpha == 0f) //Player requesting to show settings menu.
        {
            StartCoroutine(Toggle_MainMenu()); //Hide main menu.
            yield return new WaitForSeconds(_mainButtons_TweenDuration * _mainButtons.Count);

            _canvasGroup_Setting.DOFade(1f, 0f);
            for (int i = 0; i < _settingButtons.Count; i++)
            {
                _canvasGroups_SettingButtons[i].DOFade(1f, 0.1f);
                _settingButtons[i].transform.DOMove(_settingButton_AnchorsShown[i].position, _settingButtons_TweenDuration).SetEase(Ease.OutBack).SetDelay(i * 0.1f);
            }

            yield return new WaitForSeconds((_settingButtons.Count * _settingButtons_TweenDuration) + 0.1f);
            _canvasGroup_Setting.interactable = true;
        }
        else //Player requesting to hide settings menu.
        {
            _canvasGroup_Setting.interactable = false;

            for (int i = 0; i < _settingButtons.Count; i++)
            {
                _canvasGroups_SettingButtons[i].DOFade(0f, 0.25f);
                _settingButtons[i].transform.DOMove(_settingButton_AnchorsHidden[i].position, _settingButtons_TweenDuration).SetEase(Ease.OutBack).SetDelay(i * 0.05f);
            }

            yield return new WaitForSeconds(_settingButtons.Count * _settingButtons_TweenDuration);
            _canvasGroup_Setting.DOFade(0f, 0f);
        }
    }

    public void Button_Quit()
    {
        DataManager.instance.SaveGame(true);
    }

    #endregion

    #region Settings Menu

    public void Button_Setting_Return(GameObject obj) //Return to main menu from settings.
    {
        Tween_PunchButton(obj.transform);
        StartCoroutine(Toggle_MainMenu());
    }

    public void Button_Setting_ResetData(GameObject obj) //Reset all saved data.
    {
        Tween_PunchButton(obj.transform);
        //DataManager.instance.ResetPlayerData();
    }

    #endregion

    #region Misc Utility / Tweens

    public void Toggle_ModalWindow() //Modal for confirming data reset, etc.
    {
                                                                                                                    //TO DO: This.
    }

    public void Tween_PunchButton(Transform t)
    {
        t.DOPunchScale(new Vector3(t.localScale.x * 2f, t.localScale.y / 4f, t.localScale.z / 4f), 0.05f, vibrato: 5, elasticity: 1);
    }

    #endregion
}
