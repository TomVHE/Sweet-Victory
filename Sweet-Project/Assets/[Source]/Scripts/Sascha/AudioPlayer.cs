using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour {

    private IEnumerator[] _fader = new IEnumerator[2];
    public List<AudioClip> audioClips = new List<AudioClip>();
    [HideInInspector]
    public AudioSource _musicPlayer;
    AudioClip _clip;
    public float volumeChangesPerSecond = 15, fadeDuration = 1.0f;
	int _activeSong = 0, _nextSong = 1;


    void Awake()
    {
        //get everything setup
        _musicPlayer = this.GetComponent<AudioSource>();
    }

    public void Play(int nextOST)
    {
        //check if the requested song is already playing

        //stop all coroutines
        foreach (IEnumerator i in _fader)
        {
            if (i != null)
            {
                StopCoroutine(i);
            }
        }

        //Fade-out if active
        if (_musicPlayer.volume > 0)
        {
            _fader[0] = FadeAudio(fadeDuration, 0.0f);
            StartCoroutine(_fader[0]);
        }
        _nextSong = nextOST;
    }

    IEnumerator FadeAudio (float duration, float targetVolume)
    {
        //make steps
        int Steps = (int)(volumeChangesPerSecond * duration);
        float StepTime = duration / Steps;
        float StepSize = (targetVolume - _musicPlayer.volume) / Steps;
        
        //fade code stuff
        for (int i = 1; i < Steps; i++)
        {
            _musicPlayer.volume += StepSize;
            yield return new WaitForSeconds(StepTime);
        }
        _musicPlayer.volume = targetVolume;
        if(_musicPlayer.volume <= 0)
        {

        //Fade-in new song
        _musicPlayer.clip =  audioClips[_nextSong];
        _musicPlayer.Play();
        _fader[1] = FadeAudio(fadeDuration, 1.0f);
        StartCoroutine(_fader[1]);

        _activeSong = _nextSong;
        }
    }
}