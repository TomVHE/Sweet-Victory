using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Core.Damage;
using Tom;
using TMPro;

public class UIAnimator : MonoBehaviour
{
	public List<Animator> playerJoin = new List<Animator>();
	public List<Animator> playerDamage = new List<Animator>();
	public List<Animator> playerDeath = new List<Animator>();
    public List<UILives> playerLives = new List<UILives>();

	public Animator matchended;  

	private MatchManager matchManager;
    //private UIManager uiManager;

	public TextMeshProUGUI levelSelectCountdownText;

    [Serializable]
    public class UILives
    {
        public Animator[] lives = new Animator[3];
    }

	void Awake ()
	{
		levelSelectCountdownText.text = "";

		foreach (var player in FindObjectsOfType<DamageableBehaviour>())
		{
            player.configuration.Damaged += PlayerDamage;
            player.configuration.LostLife += PlayerLostLife;
            player.configuration.Finished += PlayerDeath;
		}

		foreach (var player in FindObjectsOfType<PlayerPool>())
		{
			player.PlayerJoined += PlayerJoin;
            player.PlayerLeft += PlayerLeave;
		}

        matchManager = gameObject.GetComponent<MatchManager>();
        matchManager.MatchEnded += MatchEnd;

	}

	public void LevelSelectCountDown (float seconds)
	{
		seconds -= Time.deltaTime;
		if(seconds <= 0)
		{
			levelSelectCountdownText.text = "";
			return;
		}
		string value = Mathf.RoundToInt(seconds).ToString();
		levelSelectCountdownText.text = value;

	}
	public void PlayerJoin (int id)
	{
		playerJoin[id].SetBool("PlayerJoined", true);
	}
    public void PlayerLeave (int id)
    {
        playerJoin[id].SetBool("PlayerJoined", false);
    }

    public void PlayerDamage (HitInfo info)
	{	
		//playerDamage[playerid].SetTrigger("Do It");
	}
    public void PlayerLostLife(DamageChangeInfo info)
    {
        playerLives[info.playerID].lives[info.oldLives].SetBool("LoseHeart", true);
        //playerDeath[info.configuration.playerID].SetBool("Dead", info.configuration.);
        //playerDeath[playerid].SetBool("Dead",state);
    }
    public void PlayerDeath (DamageChangeInfo info)
	{
        //playerDeath[info.configuration.playerID].SetBool("Dead", info.configuration.);
		//playerDeath[playerid].SetBool("Dead",state);
	}

	public void MatchEnd (string text)
	{
		//matchended.SetBool("Over", state);
	}

	public void ResetAll ()
	{
		for (int i = 0; i < 4; i++)
		{
			PlayerJoin(i);
			//PlayerDamage(i);
			//PlayerDeath(i, false);
			//MatchEnd(false);
		}
	}
}
