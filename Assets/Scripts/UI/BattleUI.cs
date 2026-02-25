using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;

public class BattleUI : MonoBehaviour
{
    [Header("Victory")]
    public CanvasGroup _canvas_Victory;
    public float _victory_FadeInDuration;
    public float _victory_ScaleDuration;
    public Vector3 _victory_ScaleStart;
    public Vector3 _victory_ScaleEnd;
    public float _victory_SequenceDelay; //How long to delay next ienumerator.

    [Header("Game Over")]
    public CanvasGroup _canvas_GameOver;
    public float _gameOver_FadeInDuration;
    public float _gameOver_ScaleDuration;
    public Vector3 _gameOver_ScaleStart;
    public Vector3 _gameOver_ScaleEnd;
    public float _gameOver_SequenceDelay; //How long to delay next ienumerator.

    [Header("Miscellaneous")]
    public CanvasGroup _canvas_ReturningInterim;
    public float _return_FadeInDuration;
    public float _return_SceneLoadDelay;
    [Space(6)]
    public CanvasGroup _fadeImage;

    [Header("Debug")]
    public bool _debugMessages;
    public bool _debugLevelClear;

    private void Start()
    {
        _canvas_Victory.alpha = 0f;
        _canvas_Victory.transform.localScale = _victory_ScaleStart;
        _canvas_GameOver.alpha = 0f;
        _canvas_GameOver.transform.localScale = _gameOver_ScaleStart;
        _canvas_ReturningInterim.alpha = 0f;
    }

    #region Victory
    public void Victory()
    {
        if (_debugMessages && Debug.isDebugBuild) Debug.Log("Victory!");

        StartCoroutine(Sequence_Victory());
    }

    public IEnumerator Sequence_Victory()
    {
        _canvas_Victory.DOFade(1f, _victory_FadeInDuration);
        _canvas_Victory.transform.DOScale(_victory_ScaleEnd, _victory_ScaleDuration);
        yield return new WaitForSeconds(_victory_ScaleDuration);

        //Show rewards.
        //Show next room choices.

        yield return new WaitForSeconds(_victory_SequenceDelay);
        StartCoroutine(BackToInterim());
    }
    #endregion

    #region Game Over
    public void GameOver()
    {
        if (_debugMessages && Debug.isDebugBuild) Debug.Log("Game over!");

        StartCoroutine(Sequence_GameOver());
    }

    public IEnumerator Sequence_GameOver()
    {
        _canvas_GameOver.DOFade(1f, _gameOver_FadeInDuration);
        _canvas_GameOver.transform.DOScale(_gameOver_ScaleEnd, _gameOver_ScaleDuration);
        yield return new WaitForSeconds(_gameOver_ScaleDuration);

        //Show losses.

        yield return new WaitForSeconds(_gameOver_SequenceDelay);
        StartCoroutine(BackToInterim());
    }
    #endregion

    public IEnumerator BackToInterim()
    {
        _canvas_ReturningInterim.DOFade(1f, _return_FadeInDuration); //Fade in text.

        yield return new WaitForSecondsRealtime(_return_SceneLoadDelay / 2 + (_return_FadeInDuration));
        _fadeImage.DOFade(1f, _return_FadeInDuration / 2);

        yield return new WaitForSecondsRealtime(_return_SceneLoadDelay / 2);
        SceneManager.LoadScene("InterimScene");
    }
}
