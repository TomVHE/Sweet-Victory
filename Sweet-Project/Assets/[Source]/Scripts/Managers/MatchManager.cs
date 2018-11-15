using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Damage;

public class MatchManager : Singleton<MatchManager>
{
    public event Action<string> MatchEnded;

    [Header("delay of the timer in seconds")]
    public int startDuration = 3;
    [Header("Match duration in minutes")]
    public int matchDuration = 5;
    [Header("Time until it goes to level select in seconds")]
    public int endDuration = 5;
    [Header("Does somebody win when time's up?")]
    public bool noWinner;

    LevelManager levelManager;

    private UIManager uiManager;
    private PlayerPool playerPool;
    //private HealthManager healthManager;

    public void Begin()
    {
        Reset();
        StartCoroutine(WaitForPlayers());
    }

    private void Reset()
    {
        if (uiManager == null)
        {
            uiManager = FindObjectOfType<UIManager>();
        }
        if (playerPool == null)
        {
            playerPool = FindObjectOfType<PlayerPool>();
        }
        if(levelManager == null)
        {
            levelManager = FindObjectOfType<LevelManager>();
        }

        foreach (var player in FindObjectsOfType<DamageableBehaviour>())
        {
            player.configuration.LostLife += CheckLives;
        }
    }

    private IEnumerator WaitForPlayers()
    {
        yield return new WaitUntil(() => playerPool.characters.Count > 0);

        int timer = 0;
        while (timer < startDuration)
        {
            yield return new WaitForSeconds(1f);
            timer++;
        }

        StartCoroutine(Timer());
    }

    private IEnumerator Timer()
    {
        int _minutes = matchDuration;
        int _seconds = 0;

        while (_minutes > 0 || _seconds > 0)
        {
            yield return new WaitForSeconds(1f);
            if (_seconds > 0)
            {
                _seconds--;
            }
            else
            {
                if (_minutes > 0)
                {
                    _minutes--;
                    _seconds = 59;
                }
            }
            uiManager.UpdateTimer(_minutes, _seconds);
        }
        MatchEnd((noWinner) ? "Time's up!" : GetFistPlace() + " won!");
    }

    private void MatchEnd(string endMessage)
    {
        StopCoroutine(Timer());
        /*
        foreach (var player in FindObjectsOfType<DamageableBehaviour>())
        {
            player.configuration.LostLife -= CheckLives;
        }
        */
        //levelManager.ResetLevel();
        if(levelManager != null)
        {
            levelManager.ResetLevel();
        }
        MatchEnded?.Invoke(endMessage);
        //StartCoroutine(ToLevelSelect());
    }

    private IEnumerator ToLevelSelect()
    {
        int timer = 0;
        while (timer < endDuration)
        {
            yield return new WaitForSeconds(1f);
            timer++;
        }
        FindObjectOfType<LevelManager>().ResetLevel();
    }

    private string GetFistPlace()
    {
        List<PoolablePlayer> playerOrder = playerPool.characters; //healthManager.players.ToList();
        playerOrder.Sort();

        List<int> firstPlace = new List<int>
        {
            playerOrder[0].damageable.configuration.playerID
        };

        for (int i = 1; i < playerOrder.Count; i++)
        {
            if (playerOrder[i].IsAlive)
            {
                if (playerOrder[i].CurrentLives == playerOrder[0].CurrentLives && playerOrder[i].CurrentDamage == playerOrder[0].CurrentDamage)
                {
                    firstPlace.Add(playerOrder[i].damageable.configuration.playerID);
                }
                else
                {
                    break;
                }
            }
            else
            {
                break;
            }
        }

        string first = "";
        if (firstPlace.Count == 1)
        {
            first = "Player " + firstPlace[0].ToString();
        }
        else
        {
            for (int i = 0; i < firstPlace.Count; i++)
            {
                if (i != 0)
                {
                    first += (i < firstPlace.Count - 1) ? ", " : " and ";
                }
                first += firstPlace[i].ToString();
            }
        }

        return first;
    }

    private void CheckLives(DamageChangeInfo damageable)
    {
        int peopleLeft = 0;
        foreach (PoolablePlayer player in playerPool.characters)
        {
            if (player.CurrentLives > 0)
            {
                peopleLeft++;
            }
        }
        if (peopleLeft == 1)
        {
            MatchEnd(GetFistPlace() + " won!");
        }
    }
}
