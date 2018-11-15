using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour {



	public bool fullscreen;


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
		print(state + "vsync");
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
		print(state + "fullscreen");
	}
	public void Resolution (int wid, int hei, bool state)
	{
		Screen.SetResolution(wid, hei, state);
	}
	#endregion
	#region Audio
	public void MasterVolume (float value)
	{
		
	}
	public void EffectVolume (float value)
	{
		
	}
	public void MusicVolume (float value)
	{
		
	}
	#endregion
}
