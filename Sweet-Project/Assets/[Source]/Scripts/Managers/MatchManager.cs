using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Core.Damage;

public class MatchManager : MonoBehaviour
{
    public static bool IsGameActive;

    [SerializeField] private int matchDuration;

    public event Action<string> MatchEnded;

    [NonSerialized] int matchDurMinutes, matchDurSeconds;

    public void Init()
    {
        IsGameActive = false;

        foreach (var player in GameManager.PlayerPool.players)
        {
            //player.configuration.LostLife += (damageInfo) => DeathExplosionParticle(damageInfo, player.Position);
            //player.DamageableConfig.LostLife += () => CheckLives);
            //player.DamageableConfig.LostLife += (CheckLives() => );
            player.DamageableConfig.LostLife += CheckLives;
        }
        //StartCoroutine(WaitForPlayers());
    }

    public void Reset()
    {

    }

    /*
    private IEnumerator WaitForPlayers()
    {
        yield return new WaitUntil(() => GameManager.PlayerPool.players.Count > 0);

        int timer = 0;
        while (timer < startDuration)
        {
            yield return new WaitForSeconds(1f);
            timer++;
        }

        StartCoroutine(Timer());
    }
    */

    public void BeginPlay()
    {
        IsGameActive = true;
        StartCoroutine(Timer());
    }

    private IEnumerator Timer()
    {
        int minutes = matchDuration;
        int seconds = 0;

        while (minutes > 0 || seconds > 0)
        {
            yield return new WaitForSeconds(1f);
            if (seconds > 0)
            {
                seconds--;
            }
            else
            {
                if (minutes > 0)
                {
                    minutes--;
                    seconds = 59;
                }
            }
            GameManager.UIManager.UpdateTimer(minutes, seconds);
        }
        MatchEnd("Time's up!");        //MatchEnd((noWinner) ? "Time's up!" : GetFistPlace() + " won!");
    }

    private void MatchEnd(string endMessage)
    {
        StopCoroutine(Timer());

        MatchEnded?.Invoke(endMessage);

        GameManager.LevelManager.ResetLevel();

        //StartCoroutine(ToLevelSelect());
    }

    private string GetFistPlace()
    {
        var playerOrder = GameManager.PlayerPool.players;
        playerOrder.Sort();

        return "Player " + playerOrder[0];
    }

    private void CheckLives(DamageChangeInfo info)
    {
        int peopleLeft = 0;
        foreach (PoolablePlayer player in GameManager.PlayerPool.players)
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
