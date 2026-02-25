using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class HealthBarUI : MonoBehaviour
{
    public Character_Action _parentActions;

    [SerializeField] private Camera _mainCam;

    [Header("UI Elements")]
    public Image _healthbar_Main;
    public Image _healthbar_Shadow;
    public Transform _barHolder;

    [Header("Tweenery")]
    private Tween _runningShakeTween;
    private Tween _tweenBar;
    private Tween _tweenShadow;
    public float _reduceBarDuration;
    public float _reduceShadowDelay;
    public float _reduceShadowDuration;

    private void OnEnable()
    {
        if (_mainCam == null)
        {
            _mainCam = Camera.main;
        }
    }

    private void Update()
    {
        Vector3 upAxis = Vector3.up;
        transform.rotation = Quaternion.LookRotation(Vector3.Cross(upAxis, Vector3.Cross(upAxis, -_mainCam.transform.forward)), upAxis);
    }

    public void DecreaseHealth(float damage, float currentHealth, float maxHealth)
    {
        float tempHealth = ((currentHealth - damage) / maxHealth);

        StartCoroutine(Reduce_MainBar(tempHealth));
        StartCoroutine(Reduce_ShadowBar(tempHealth));

        //if (_runningShakeTween != null && _runningShakeTween.IsPlaying()) _runningShakeTween.Restart(); //If bar is shaking from a prior hit, restart that one to not upset the status quo.
        //else _runningShakeTween = _barHolder.transform.DOShakePosition(0.2f, strength: 0.10f, vibrato: 15, randomness: 3f, fadeOut: true); //Regardless, shake health bar.
    }

    public IEnumerator Reduce_MainBar(float newValue)
    {
        if (_tweenBar != null)
        {
            if (_tweenBar.IsPlaying()) //If active, we need to kill it before launching next one.
            {
                _tweenBar.Kill();
                _tweenBar = _healthbar_Main.DOFillAmount(newValue, _reduceBarDuration);
            }
        }

        _tweenBar = _healthbar_Main.DOFillAmount(newValue, _reduceBarDuration);

        yield return new WaitForSeconds(0.05f);
    }

    public IEnumerator Reduce_ShadowBar(float newValue)
    {
        yield return new WaitForSeconds(_reduceShadowDelay);

        if (_tweenShadow != null)
        {
            if (_tweenShadow.IsPlaying()) //If active, we need to kill it before launching next one.
            {
                _tweenShadow.Kill();
                _tweenShadow = _healthbar_Shadow.DOFillAmount(newValue, _reduceShadowDuration);
            }
        }

        _tweenShadow = _healthbar_Shadow.DOFillAmount(newValue, _reduceShadowDuration);

        yield return new WaitForSeconds(0.05f);
    }
}
