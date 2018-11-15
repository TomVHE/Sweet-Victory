using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Settings : MonoBehaviour {


	public AudioMixer audioMixer;

	#region Graphics
	public void VSync (bool state)
	{
		if(state)
		{
			QualitySettings.vSyncCount = 1;
		}
		else 
		{
			QualitySettings.vSyncCount = 0;
		}
	}
	public void Fullscreen (bool state)
	{
		if(state)
		{
			Screen.fullScreen = true;
		}
		else
		{
			Screen.fullScreen = false;
			Resolution(1280, 800, false);
		}
	}
	public void Resolution (int wid, int hei, bool state)
	{
		Screen.SetResolution(wid, hei, state);
	}
	#endregion
	#region Audio
	public void MasterVolume (float value)
	{
		audioMixer.SetFloat("MasterVolume", value);
	}
	public void EffectVolume (float value)
	{
		audioMixer.SetFloat("EffectsVolume", value);
	}
	public void MusicVolume (float value)
	{
		audioMixer.SetFloat("MusicVolume", value);
	}
	#endregion
}
