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
			//ChangeButtonColor(i);

			if(i == tabnmr)
			{
				panels[i].gameObject.SetActive(true);
				//ChangeColor(tabnmr);
			}
		}
	}
	//changes color of the tabs			IS FUCKED NOW WITH NEW UI
	public void ChangeColor (int numb)
	{
		bgColor = panels[numb].color;
		selected = bgColor;

		foreach (var img in buttons)
		{
			img.color = unselected;
		}
		buttons[numb].color = selected;
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
