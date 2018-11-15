using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using Rewired;
using Sirenix.OdinInspector;
using Random = UnityEngine.Random;
//using Core.Movement;
//using MovementController = Core.Movement.MovementController;
using PlayerController = Tom.PlayerController;
using Core.Damage;
using Core.Utilities;
using JacobGames.SuperInvoke;

public class PlayerPool : DestroyableSingleton<PlayerPool>
{
    #region Variables

    #region Serialized

    [SerializeField] private Team[] teams = new Team[4];

    [AssetsOnly]
    [SerializeField] private GameObject playerPrefab;

    public int respawnTime = 3;

    [SceneObjectsOnly]
    [ValidateInput("ValidateVariable", "Must have a pool object assigned!")]
    public Transform pool;

    #endregion

    #region Non-Serialized

    [NonSerialized] public List<PoolablePlayer> players = new List<PoolablePlayer>();

    private List<PlayerMap> playerMap;

    private IList<Rewired.Player> playerMaps
    {
        get
        {
            return ReInput.isReady ? ReInput.players.Players : null;
        }
    }

    private class PlayerMap
    {
        public int rewiredPlayerId;
        public int gamePlayerId;

        public PlayerMap(int rewiredPlayerId, int gamePlayerId)
        {
            this.rewiredPlayerId = rewiredPlayerId;
            this.gamePlayerId = gamePlayerId;
        }
    }

    private LevelManager levelManager;

    #endregion

    #region Events

    public event Action<int> PlayerJoined, PlayerLeft;

    #endregion

    #endregion

    #region Methods

    //EDIT
    public void Init()
    {
        //Debug.Log("Frans zijn rughaar");

        levelManager = GameManager.LevelManager;

        pool.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!ReInput.isReady) { return; }

        foreach(PoolablePlayer player in players)
        {
            player.DamageableConfig.LostLife += OnLostLife;
            player.DamageableConfig.Finished += OnFinished;
        }

        //player.configuration.LostLife += (damageInfo) => DeathExplosionParticle(damageInfo, player.Position);
        //player.configuration.Damaged += HitParticle;

        AssignJoysticksToPlayers();

        for (int id = 0; id < playerMaps.Count; id++)
        {
            if (players.ElementAtOrDefault(id) == null)
            {
                Level level = levelManager.levels[0];

                if (level == null) { Debug.LogError("HubLevel is NULL"); }

                if (playerMaps[id].controllers.joystickCount > 0)
                {
                    Initialize(id, level.RandomSpawnPoint());
                }

            }
        }

    }

    private void OnLostLife(DamageChangeInfo info)
    {
        DeSpawn(info.playerID);
    }
    private void OnFinished(DamageChangeInfo info)
    {
        Delete(info.playerID);
    }


    #region Editor Classes

    private bool ValidateVariable(Transform input)
    {
        bool validation = input != null;
        if(!validation)
        {
            Debug.LogError("PlayerPool.cs must have a pool object assigned!"); //Temp
        }

        return validation;
    }

    #endregion

    #region Spawning & Despawning

    public void Initialize(int playerID)
    {
        if (levelManager.CurrentLevel != null)
        {
            Initialize(playerID, levelManager.CurrentLevel.RandomSpawnPoint(), Quaternion.identity);
        }
        else
        {
            Debug.LogError("Couldn't Initialize, levelManager.currentlevel is NULL");
        }
    }
    public void Initialize(int playerID, Vector3 position)
    {
        Initialize(playerID, position, Quaternion.identity);
    }
    public void Initialize(int playerID, Vector3 position, Quaternion rotation)
    {
        //spawned[playerID] = true;
        var playerObject = Instantiate(playerPrefab, position, rotation, pool.parent);

        playerObject.name = (playerObject.name + playerID);

        PlayerController controller = playerObject.GetComponentInChildren<PlayerController>();
        if (controller == null)
            Debug.LogError("ERROR: Couldn't find PlayerController");

        DamageableBehaviour initDamageable = playerObject.GetComponentInChildren<DamageableBehaviour>();
        if (initDamageable == null)
            Debug.LogError("ERROR: Couldn't find DamageableBehaviour");

        controller.myID = playerID;

        PoolablePlayer PlayerInit = new PoolablePlayer
        {
            movementController = controller,
            damageableBehaviour = initDamageable
        };

        if (PlayerInit.DamageableConfig != null)
        {
            PlayerInit.DamageableConfig.playerID = playerID;
            PlayerInit.DamageableConfig.AlignmentProvider = teams[playerID].alignment;

            var meshRenderer = PlayerInit.DamageableConfig.materialObject.GetComponentInChildren<SkinnedMeshRenderer>();

            if (meshRenderer != null)
            {
                meshRenderer.material = teams[playerID].material;
            }

            PlayerInit.DamageableConfig.IsAlive = true;
        }
        else
        {
            Debug.LogError("No DamageableBehaviour component on player object");
        }

        PlayerJoined?.Invoke(playerID);

        players.Add(PlayerInit);
    }

    public void Spawn(int playerID)
    {
        if (levelManager.CurrentLevel != null)
        {
            Spawn(playerID, levelManager.CurrentLevel.RandomSpawnPoint(), Quaternion.identity);
        }
        else
        {
            Debug.LogError("Couldn't Spawn, levelManager.currentlevel is NULL");
        }
    }
    public void Spawn(int playerID, Vector3 position)
    {
        Spawn(playerID, position, Quaternion.identity);
    }
    public void Spawn(int playerID, Vector3 position, Quaternion rotation)
    {
        //Debug.Log("spawning at " + characters[playerID].damageable.transform.root.name);
        if(players[playerID] == null)
        {
            Debug.LogError("Couldn't Spawn, character is NULL");
        }

        players[playerID].IsAlive = true;

        Transform characterTransform = players[playerID].damageableBehaviour.transform;

        for(int i = 0; i <= characterTransform.childCount - 1; i++)
        {
            characterTransform.GetChild(i).SetPositionAndRotation(position, rotation);

            //Debug.Log(string.Format("int = {0}, obj {1}, pos = {2}, rot = {3}",i , characterTransform.GetChild(i) , position, rotation));
        }

        players[playerID].damageableBehaviour.transform.GetComponentInChildren<RootMotion.Dynamics.PuppetMaster>().mode = RootMotion.Dynamics.PuppetMaster.Mode.Active;

        characterTransform.parent = pool.parent;
    }

    public void DeSpawn(int playerID)
    {
        if (players[playerID].IsAlive)
        {
            Debug.Log(string.Format("respawning player {0} in {1} seconds...", playerID, respawnTime));
            players[playerID].IsAlive = false;

            Transform characterTransform = players[playerID].damageableBehaviour.transform;

            players[playerID].damageableBehaviour.transform.GetComponentInChildren<RootMotion.Dynamics.PuppetMaster>().mode = RootMotion.Dynamics.PuppetMaster.Mode.Disabled;

            characterTransform.parent = pool;

            players[playerID].ResetDamage();

            SuperInvoke.Run( () => Spawn(playerID), (respawnTime != 0 ? respawnTime : 3));
        }

    }

    public void Delete(int playerID)
    {
        //Destroy(characters[playerID].playerController.transform.gameObject); EDIT
        //characters.RemoveAt(playerID);
        Debug.Log("FINISH HIM");
        PlayerLeft?.Invoke(playerID);
    }

    #endregion

    #region Joystick Assignment

    private void AssignJoysticksToPlayers()
    {
        IList<Joystick> joysticks = ReInput.controllers.Joysticks; //Get connected joysticks

        for (int i = 0; i < joysticks.Count; i++)
        {
            Joystick joystick = joysticks[i];
            if (ReInput.controllers.IsControllerAssigned(joystick.type, joystick.id)) continue; //Next iteration if joystick is already assigned to a Player

            if (joystick.GetAnyButtonDown())
            {
                Rewired.Player player = FindPlayerWithoutJoystick();
                if (player == null) return; // return on no free joysticks

                //rewiredPlayer.controllers.maps.SetMapsEnabled(false, "Assignment");
                player.controllers.AddController(joystick, false);
            }
        }

        // If all players have joysticks, enable joystick auto-assignment
        if (DoAllPlayersHaveJoysticks())
        {
            ReInput.configuration.autoAssignJoysticks = true;
        }
    }

    private Rewired.Player FindPlayerWithoutJoystick()
    {
        for (int i = 0; i < playerMaps.Count; i++)
        {
            if (playerMaps[i].controllers.joystickCount > 0) continue;
            return playerMaps[i];
        }
        return null;
    }

    private bool DoAllPlayersHaveJoysticks()
    {
        return (FindPlayerWithoutJoystick() == null);
    }

    #endregion

    #endregion
}

[Serializable]
public class PoolablePlayer : IComparable<PoolablePlayer>
{
    [HorizontalGroup("Group 1", LabelWidth = 2), HideLabel]
    [SerializeField] public PlayerController movementController;

    [HorizontalGroup("Group 1", LabelWidth = 2), HideLabel]
    [SerializeField] public DamageableBehaviour damageableBehaviour;

    //EDIT
    public Damageable DamageableConfig
    {
        get
        {
            if (damageableBehaviour != null)
            {
                return damageableBehaviour.configuration;
            }
            else
            {
                Debug.LogError("PoolablePlayer doesn't have an DamageableBehaviour is NULL");
                return null;
            }
        }
    }

    public int PlayerID
    {
        get
        {
            return DamageableConfig.playerID;
        }
        set
        {
            DamageableConfig.playerID = value;
        }
    }

    public bool IsAlive
    {
        get
        {
            return DamageableConfig.IsAlive;
        }
        set
        {
            DamageableConfig.IsAlive = value;
        }
    }

    public int CurrentLives
    {
        get
        {
            return DamageableConfig.CurrentLives;
        }
        set
        {
            DamageableConfig.SetLives(value);
        }
    }

    public int CurrentDamage
    {
        get
        {
            return DamageableConfig.CurrentDamage;
        }
        set
        {
            DamageableConfig.SetDamage(value);
        }
    }

    public bool CanKnockback(IAlignmentProvider attackerAlignment)
    {
        return damageableBehaviour.CanKnockback(attackerAlignment);
    }

    public void ResetDamage()
    {
        DamageableConfig.ResetDamage();
    }

    public int CompareTo(PoolablePlayer other)
    {
        if (CurrentLives < other.CurrentLives)
        {
            return -1;
        }
        else if (CurrentLives > other.CurrentLives)
        {
            return 1;
        }

        if (CurrentDamage > other.CurrentDamage)
        {
            return -1;
        }
        else if (CurrentDamage < other.CurrentDamage)
        {
            return 1;
        }

        return 0;
    }

}

[Serializable]
public class Team
{
    [AssetsOnly]
    [HorizontalGroup("Group 1", LabelWidth = 2), HideLabel]
    public Material material;
    [HorizontalGroup("Group 1", LabelWidth = 2), HideLabel]
    public Color color = Color.white;
    [HorizontalGroup("Group 1", LabelWidth = 2), HideLabel]
    public Alignment alignment;
}