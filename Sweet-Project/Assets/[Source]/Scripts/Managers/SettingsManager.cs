using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SettingsManager : DestroyableSingleton<SettingsManager> 
{
	public List<Image> panels = new List<Image>();
	public List<Image> buttons = new List<Image>();

    //private GameObject first

	public GameObject settings;

	public Color selected, unselected, bgColor;

	public Image backButton;

	public GameObject graphicsFirst, audioFirst;
 
	

	// Use this for initialization
	void Start () 
	{
		//backButton.color = unselected;
		//selected = bgColor;
		SwitchTab(0);
	}
	//switches the tab via buttons
	public void SwitchTab (int tabnmr)
	{
		for (int i = 0; i < panels.Count; i++)
		{
			panels[i].gameObject.SetActive(false);

			if(i == tabnmr)
			{
				panels[i].gameObject.SetActive(true);
				SettingsTabMenu(tabnmr);
			}
		}
	}
	public void SettingsTabMenu (int tabnmr) 
	{
		//set first selected button to settings
		if(tabnmr == 0)
		{
			SelectedButton(graphicsFirst);
		}
		if(tabnmr == 1)
		{
			SelectedButton(audioFirst);
		}
	}
	public void SelectedButton (GameObject button)
	{
		EventSystem eventSystem = EventSystem.current;

        eventSystem.SetSelectedGameObject(button);
	}
	//activates the settings or deactivates it
	public void ActivateSettings (bool state) 
	{
		if(state == settings.activeSelf) //is this even needed? prob not
		{

			state = !state;
		}
        settings.SetActive(state);
	}
}
