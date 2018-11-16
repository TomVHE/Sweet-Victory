using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(MatchManager), typeof(UIManager), typeof(PlayerPool))]
[RequireComponent(typeof(CameraSystem))]
public class GameManager : MonoBehaviour
{
    private static GameManager Instance;

    #region Managers and Systems
    public static PlayerPool PlayerPool
    {
        get; private set;
    }
    public static MatchManager MatchManager { get; private set; }
    public static UIManager UIManager { get; private set; }
    public static CameraSystem CameraSystem { get; private set; }
    public static LevelManager LevelManager { get; private set; }
    #endregion

    private void Awake()
    {
        Instance = this;

        Debug.Log("Joppiesaus");

        PlayerPool = GetComponent<PlayerPool>();
        MatchManager = GetComponent<MatchManager>();
        UIManager = GetComponent<UIManager>();
        CameraSystem = GetComponent<CameraSystem>();
        LevelManager = GetComponent<LevelManager>();

        PlayerPool.Init();
        MatchManager.Init();
        //UIManager.Init();
        CameraSystem.Init();
        LevelManager.Init();
    }

    private void Update()
    {
        if (!MatchManager.IsGameActive)
            return;

        // Get camera data
        List<PoolablePlayer> players = PlayerPool.players;
        List<Vector3> playerPositions = new List<Vector3>();
        foreach (PoolablePlayer player in players)
            playerPositions.Add(player.movementController.transform.position);

        CameraSystem.CalcLookPos(playerPositions);
    }

    public static void OnSelectedLevel()
    {
        // Fall to level
        MatchManager.BeginPlay();
    }

    public static void LoadLobbyLevel()
    {

        LevelManager.ResetLevel();
        //SceneManager.LoadScene(lobbyLevelName, LoadSceneMode.Single);
    }
}
