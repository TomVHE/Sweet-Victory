using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;
using Random = UnityEngine.Random;

[Serializable]
public class Level
{
    [HorizontalGroup("Group 1", LabelWidth = 2), HideLabel]
    [SceneObjectsOnly]
    public GameObject levelPrefab;

    public Transform[] spawnPoints = new Transform[0];

    public Vector3 RandomSpawnPoint()
    {
        return spawnPoints.Length > 0 ? spawnPoints[Random.Range(0, spawnPoints.Length - 1)].position : Vector3.zero;
    }
}
[Serializable]
public class SelectableLevel : Level
{
    [HorizontalGroup("Group 1"), HideLabel]
    public LevelSelector levelSelector;
}

public class LevelManager : DestroyableSingleton<LevelManager>
{
    [SerializeField] private List<SelectableLevel> levels = new List<SelectableLevel>();

    [PropertySpace(SpaceAfter = 10, SpaceBefore = 10)]
    public Level hubLevel;

    public AudioClip hubMusic;

    public string mainScene = "WaltersBS";

    [SerializeField] private float timeToLoad = 5f;

    public AudioPlayer audioPlayer;

    [NonSerialized] public Level currentlevel;

    private bool loading;

    private void Start()
    {
        currentlevel = hubLevel;
    }
    private void Update()
    {
        //if (levels != null && firstInit == false)
        if (levels != null)
        {
            for (int i = 0; i < levels.Count; i++)
            {
                if (levels[i].levelSelector.players.Count > 0 && loading == false)
                {
                    loading = true;
                    //uiAnimator.LevelSelectCountDown(timeToLoad);
                    Invoke("LoadLevel", timeToLoad);
                }
            }
        }
    }

    private GameObject LoadLevel()
    {
        int allVoters = 0;

        List<int> voters = new List<int>();

        //two loops because I use the first one to add them all up.
        for (int i = 0; i < levels.Count; i++)
        {
            int count = levels[i].levelSelector.players.Count;
            voters.Add(count);
            allVoters += count;
        }

        for (int i = 0; i < levels.Count; i++)
        {
            float currVoters = ((float)voters[i]);
            float currProbability = (currVoters / ((float)allVoters));

            if (Random.value <= currProbability)
            {
                levels[i].levelPrefab.SetActive(true);
                hubLevel.levelPrefab.SetActive(false);
                FindObjectOfType<MatchManager>().Begin();
                audioPlayer.Play(i);

                currentlevel = levels[i];

                return (levels[i].levelPrefab);
            }
        }

        return null;
    }
    private GameObject LoadLevel(int levelID)
    {
        return (levelID >= 0 && levelID < levels.Count) ? (levels[levelID].levelPrefab) : null;
    }

    public void ResetLevel()
    {
        if (mainScene != null)
        {
            SceneManager.LoadScene(mainScene);
        }
    }
}