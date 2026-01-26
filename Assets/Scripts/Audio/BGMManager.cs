using DG.Tweening;
using System.Collections;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public static BGMManager instance;

    [Header("Parameters & References")]
    [SerializeField] private AudioSource _audioSource_BGM0;
    [SerializeField] private AudioSource _audioSource_BGM1;
    [Space(6)]
    [SerializeField] private AudioClip[] _soundtrack_Intro; //If entry holds a track, play it first.
    [SerializeField] private AudioClip[] _soundtrack_Main; //Main looping track.
    [SerializeField] private AudioClip[] _soundtrack_Layer; //Battle layer when player is not in tactical pause mode. Always playing if main track is.

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

    public void StartNewTrack(float fadeOut = 0f, float fadeIn = 1f, int whichTrack = 0)
    {
        //First, check if this track has a separate intro.

        StartCoroutine(FadeInNew(fadeOut, fadeIn, whichTrack));
    }

    public IEnumerator FadeInNew(float fadeOut, float fadeIn, int whichTrack)
    {
        _audioSource_BGM0.DOFade(0f, fadeOut);
        _audioSource_BGM1.DOFade(0f, fadeOut);
        yield return new WaitForSeconds(fadeOut);
        _audioSource_BGM0.Stop();
        _audioSource_BGM1.Stop();
        _audioSource_BGM1.clip = null; //To avoid playing old layered track along new one.

        _audioSource_BGM0.clip = _soundtrack_Main[whichTrack];
        _audioSource_BGM0.Play();
        _audioSource_BGM0.DOFade(1f, fadeIn);
        if (_soundtrack_Layer[whichTrack] != null)
        {
            _audioSource_BGM1.clip = _soundtrack_Layer[whichTrack];
            _audioSource_BGM1.Play();
            _audioSource_BGM1.DOFade(1f, fadeIn);
        }
    }

    public void ToggleMute() //Toggle mute setting for all BGM audio sources.
    {
        _audioSource_BGM0.mute = !_audioSource_BGM0.mute;
        _audioSource_BGM1.mute = !_audioSource_BGM1.mute;
    }
}
