using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Core.Utilities;
using UnityEngine.SceneManagement;

public class LevelManager : DestroyableSingleton<LevelManager>
{
    //[SerializeField]
    public string mainScene = "Main Scene";

    public Level[] levels = new Level[0];

    public Level CurrentLevel { get; private set; }
    
    [SerializeField] private float voteTime;
    private int[] votes;

    public void Init()
    {
        for (int level = 0; level < levels.Length; level++)
        {
            levels[level].levelObject.SetActive(false);
        }
        votes = new int[levels.Length];

        StartCoroutine(WaitForVotes());
    }

    public void ResetLevel()
    {
        SceneManager.LoadScene(mainScene, LoadSceneMode.Single);
    }

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

    public void Voted(int index)
    {
        votes[index]++;
    }

    public void UnVoted(int index)
    {
        votes[index]--;
    }

	public void SelectLevel()
    {
        List<int> votable = new List<int>();

        for (int i = 0; i < votes.Length; i++)
            for (int j = 0; j < votes[i]; j++)
                votable.Add(i);

        int selectedLevel = votable[UnityEngine.Random.Range(0, votable.Count - 1)];

        for (int level = 0; level < levels.Length; level++)
            levels[level].levelObject.SetActive(false);
        levels[selectedLevel].levelObject.SetActive(true);

        GameManager.OnSelectedLevel();
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