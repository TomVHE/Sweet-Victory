using System; //Tom
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.EventSystems;

public class PauseMenu : DestroyableSingleton<PauseMenu> 
{
	//script
	public SettingsManager settingsScript;

	//paused menu 
	public GameObject pausedMenu;


	//paused menu
	public bool paused, settings;
	public float currentTimeScale;

	public GameObject resumeButton, settingsButton;

	//public GameObject levelSelectObject;

    //Tom
    public event Action<bool> Menu;


	void Update ()
	{
		CheckInput();
	}
	public void ActivatePauseMenu (bool state)
	{
		if(state == pausedMenu.activeSelf)	//is this even needed? prob not
		{
			state = !state;
		}
        Menu?.Invoke(state); //Tom
        paused = state;
		pausedMenu.SetActive(state);
        
    }
	//checks for input for pausing the game
	public void CheckInput () 
	{
        if(ReInput.players.playerCount >= 0)
        {
            IList<Player> players = ReInput.players.Players;
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].GetButtonDown("Menu") && !paused)	//add b button to exit
                {
					SelectedButton(resumeButton);

					ActivatePauseMenu(true);
                    //SetTimeScale(0f);
                    paused = true;
                }
				else if (players[i].GetButtonDown("Menu") && paused && !settings)
                {
                    //SetTimeScale(1f);
					ActivatePauseMenu(false);
					paused = false;
                }
				else if (players[i].GetButtonDown("Menu") && paused && settings)
                {
					settingsScript.ActivateSettings(false);
					settings = false;
                }
            }
        }
    }
	public void SelectedButton (GameObject button)
	{
		EventSystem eventSystem = EventSystem.current;

        eventSystem.SetSelectedGameObject(button);
	}
	public void SettingsMenu (bool state) 
	{
		settings = state;
		settingsScript.ActivateSettings(state);
		//set first selected button to settings
		if(state)
		{
			SelectedButton(settingsButton);
		}
		else
		{
			SelectedButton(resumeButton);
		}
	}
	public void LevelSelector ()
	{
		//go to hub
	}
	public void LevelSelect (bool state) 
	{
		//levelSelectObject.SetActive(state);
		//get a vote warning for going to level select
	}
	public void QuitGame ()
	{
		Application.Quit();
	}
	//sets time scale
	public void SetTimeScale (float scale)
	{
		Time.timeScale = scale;
		if(scale == 0)
		{
			paused = true;
		}
		if(scale == 1)
		{
			paused = false;
		}
	}
}
