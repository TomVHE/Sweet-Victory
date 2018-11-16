using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Core.Utilities;
using UnityEngine.SceneManagement;

public class LevelManager : DestroyableSingleton<LevelManager>
{
    public Level[] levels = new Level[0];

    public Level CurrentLevel { get; private set; }

    [SerializeField] private string lobbyLevelName = "Main Scene";

    [SerializeField] private float voteTime = 3;
    private int[] votes;

    private bool spawning;

    public void Init()
    {
        for (int level = 1; level < levels.Length; level++)
        {
            levels[level].levelObject.SetActive(false);
        }
        votes = new int[levels.Length];

        //StartCoroutine(WaitForVotes());
    }

    public void ResetLevel()
    {
        SceneManager.LoadScene(lobbyLevelName, LoadSceneMode.Single);
    }

    /*
    private IEnumerator WaitForVotes()
    {
        Func<bool> hasVoted = delegate ()
        {
            foreach (int i in votes)
                if (i > 0)
                    return true;
            return false;
        };

        while (true)
        {
            while (!hasVoted())
                yield return null;

            float remainingTime = voteTime;

            Action changeLight = delegate ()
            {
                //HERE
                StartingLight.Instance.ChangeLight(3 - Mathf.RoundToInt(remainingTime));
            };

            Action resetLight = delegate ()
            {
                //HERE
                StartingLight.Instance.ResetLights();
            };

            while (remainingTime > 0)
            {
                // Show remaining time UI
                changeLight();

                if (!hasVoted())
                {
                    // Reset remaining time UI
                    resetLight();
                    break;
                }
                yield return null;
            }

            if (!hasVoted())
            {
                // Reset remaining time UI 
                remainingTime = voteTime;
                changeLight();
                continue;
            }

            // Finish remaining time UI
            resetLight();

            break;
        }
        SelectLevel();
    }
    */

    public void Vote(int index)
    {
        Debug.Log("vote");

        votes[index]++;
    }

    public void UnVote(int index)
    {
        Debug.Log("unvote");

        votes[index]--;
    }

    public void Update()
    {
        List<int> combinedVotes = new List<int>();

        for (int i = 0; i < votes.Length; i++)
        {
            for (int j = 0; j < votes[i]; j++)
            {
                combinedVotes.Add(i);
            }
        }

        if(combinedVotes.Count > 0) // -1
        {
            if (!spawning)
            {
                Invoke("SelectLevel", voteTime);
            }
        }
        else
        {
            spawning = false;
            CancelInvoke("SelectLevel");
        }
    }

	public void SelectLevel()
    {
        spawning = true;
        Debug.Log("SelectLevel");

        List<int> combinedVotes = new List<int>();

        for (int i = 0; i < votes.Length; i++)
        {
            for (int j = 0; j < votes[i]; j++)
            {
                combinedVotes.Add(i);
            }
        }

        int selectedLevel = combinedVotes[UnityEngine.Random.Range(0, combinedVotes.Count)]; //- 1

        for (int level = 0; level < levels.Length; level++)
        {
            levels[level].levelObject.SetActive(false);
        }

        levels[selectedLevel].levelObject.SetActive(true);

        GameManager.OnSelectedLevel();
    }

    public void LoadLevel()
    {

    }
}

[Serializable]
public class Level
{
    [SceneObjectsOnly]
    public GameObject levelObject;

    public Transform[] spawnPoints = new Transform[0];

    public Vector3 RandomSpawnPoint()
    {
        return spawnPoints.Length > 0 ? spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length - 1)].position : Vector3.zero;
    }
}