using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour {

/*
	Resolution DROPDOWN:
	multiple buttons
	- + to switch between resolutions
*/
	public int width, height;
	public enum ResolutionSize {Small, Medium, Big, Glorious4K}
	public ResolutionSize _Size;
	public bool fullscreen;

	void SetState (ResolutionSize state)
	{
		_Size = state;
		SetResolution();
	}	
	public void SetResolution ()
	{
		switch(_Size)
		{
			case ResolutionSize.Small:

				width = 720;
				height = 480;
				Resolution(width, height, fullscreen);
				
			break;

			case ResolutionSize.Medium:

				width = 1280;
				height = 720;
				Resolution(width, height, fullscreen);

			break;

			case ResolutionSize.Big:

				width = 1920;
				height = 1080;
				Resolution(width, height, fullscreen);

			break;

			case ResolutionSize.Glorious4K:

				width = 3840;
				height = 2160;
				Resolution(width, height, fullscreen);

			break;
		}
	}
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
			Resolution(width, height, false);
		}
	}
	public void Resolution (int wid, int hei, bool state)
	{
		Screen.SetResolution(wid, hei, state);
	}
	#endregion
	#region Audio
	public void MasterVolume (int value)
	{

	}
	public void EffectVolume (int value)
	{
		
	}
	public void MusicVolume (int value)
	{
		
	}
	public void Announcer (bool state)
	{
		
	}
	#endregion
	public void ControllerRemap (int buttonID, string newKey)
	{
		//In case this will happen
	}
}
