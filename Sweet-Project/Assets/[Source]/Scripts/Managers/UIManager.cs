using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using Rewired;
using Core.Damage;


[Serializable]
public class UIPlayer 
{
	public TextMeshProUGUI damageText;
    //public Image[] liveIcons= new Image[3];

	
}
public class UIManager : DestroyableSingleton<UIManager>
{
	//public List<TextMeshProUGUI> damageTexts = new List<TextMeshProUGUI>();
	public UIPlayer[] uiPlayers = new UIPlayer[4];


	//timer
	public TextMeshProUGUI time;

	//scripts
	public PauseMenu pausemenu;

    [HideInInspector]
    public List<DamageableBehaviour> players = new List<DamageableBehaviour>();

    public List<Animator> playerIcons = new List<Animator>();

    //Tom
    private void Start()
    {
        //Cursor.visible = false;
        // foreach(TextMeshProUGUI dmgText in damageTexts)
		for (int i = 0; i < uiPlayers.Length; i++)
        {
		    var dmgText = uiPlayers[i];
            dmgText.damageText.text = "<size=50%> </size>0<size=50%>%";
		}
    }
    //updates damage in ui
    public void UpdateDamage (int playerID, float percentage)
	{
		TextMeshProUGUI playerText = uiPlayers[playerID].damageText; 

		playerText.text = "<size=50%> </size>" + percentage + "<size=50%>%"; //360<size=50%>%
	}
	//updates the timer
	public void UpdateTimer (int minute, float second)
	{
		//minute = Mathf.RoundToInt(min);
		second = Mathf.RoundToInt(second);
        string sec = (second >= 10) ? second.ToString() : "0" + second.ToString(); //Tom
		time.text = minute.ToString() + ":" + ((second >= 0) ? sec : "00"); //Tom
	}

    private void Update()
    {
        for (int i = 0; i < players.Count; i++)
        {
            UpdateDamage(i, players[i].configuration.CurrentDamage);
            for (int j = 1; j < 3; j++)
            {
                Animator heart = playerIcons[i].transform.GetChild(j).GetComponentInChildren<Animator>();
                if(players[i].configuration.CurrentLives < j)
                {
                    if (!heart.GetBool("LoseHeart"))
                    {
                        heart.SetBool("LoseHeart", true);
                    }
                }
            }
        }
    }

    public void Join(DamageableBehaviour player)
    {
        print(player.configuration.playerID);
        players.Add(player);
        playerIcons[player.configuration.playerID].SetBool("PlayerJoined", true);
    }
}
