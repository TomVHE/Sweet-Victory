using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using Rewired;
using Sirenix.OdinInspector;
using Random = UnityEngine.Random;
using PlayerController = Tom.PlayerController;
using Core.Damage;
using JacobGames.SuperInvoke;

public class PlayerPool : DestroyableSingleton<PlayerPool>
{
    #region Variables

    #region Editor Variables

    [SerializeField] private Team[] teams = new Team[4];

    [NonSerialized] public List<PoolablePlayer> characters = new List<PoolablePlayer>();

    [AssetsOnly]
    [SerializeField] private GameObject playerPrefab;

    public int respawnTime = 3;

    [SceneObjectsOnly]
    [ValidateInput("ValidateVariable", "Must have a pool object assigned!")]
    public Transform pool;

    #endregion

    public event Action<int> PlayerJoined, PlayerLeft;

    private List<PlayerMap> playerMap;

    private IList<Rewired.Player> playerMaps
    {
        get
        {
            return ReInput.isReady ? ReInput.players.Players : null;
        }
    }

    //private bool[] spawned = new bool[4];

    //private List<Transform> characterRoots = new List<Transform>();

    //private HealthManager m_HealthManager = null;

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

    //private int characterId = 0;

    private LevelManager levelManager;

    #endregion

    #region Methods

    private void Start()
    {
        //characters.ForEach(c => characterRoots.Add(c.root));

        pool.gameObject.SetActive(false);

        levelManager = FindObjectOfType<LevelManager>();
    }

    private void Update()
    {
        if (!ReInput.isReady) { return; }

        AssignJoysticksToPlayers();

        for (int id = 0; id < playerMaps.Count; id++)
        {
            //if (spawned[id] == false) //First Spawn
            if (characters.ElementAtOrDefault(id) == null)
            {
                var level = levelManager.hubLevel;

                if (playerMaps[id].controllers.joystickCount > 0)
                {
                    //if()
                    Initialize(id, level.RandomSpawnPoint());
                    //m_HealthManager.PlayerSpawn(id);

                }
            }
        }

    }

    private bool ValidateVariable(Transform input)
    {
        bool validation = input != null;
        if(!validation)
        {
            Debug.LogError("PlayerPool.cs must have a pool object assigned!"); //Temp
        }

        return validation;
    }

    #region Spawning & Despawning

    public void Initialize(int playerID)
    {
        if (levelManager.currentlevel != null)
        {
            Initialize(playerID, levelManager.currentlevel.RandomSpawnPoint(), Quaternion.identity);
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

        DamageableBehaviour damageable = playerObject.GetComponentInChildren<DamageableBehaviour>();
        if (damageable == null)
            Debug.LogError("ERROR: Couldn't find DamageableBehaviour");

        var Character = new PoolablePlayer
        {
            //transform = controller.root,
            playerController = controller,
            damageable = damageable
        };

        controller.myID = playerID;

        //new PoolablePlayer { }

        //characters.Add();

        if (damageable != null)
        {
            damageable.configuration.playerID = playerID;
            damageable.configuration.AlignmentProvider = teams[playerID].alignment;

            var meshRenderer = damageable.configuration.materialObject.GetComponentInChildren<SkinnedMeshRenderer>();
            //var meshRenderer = damageable.configuration.materialObject.GetComponentInChildren<MeshRenderer>();
            if (meshRenderer != null)
            {
                //Debug.Log("IJSCOKRAAM");
                meshRenderer.material = teams[playerID].material;
            }

            damageable.IsAlive = true;
        }
        else
        {
            Debug.LogError("No DamageableBehaviour component on player object");
        }

        PlayerJoined?.Invoke(playerID);

        //Debug.Log(playerID);

        //characters[playerID] = Character;
        characters.Add(Character);
    }

    public void Finish(int playerID)
    {
        //Destroy(characters[playerID].playerController.transform.gameObject); EDIT
        //characters.RemoveAt(playerID);
        Debug.Log("FINISH HIM");
        PlayerLeft?.Invoke(playerID);
    }

    public void Spawn(int playerID)
    {
        if (levelManager.currentlevel != null)
        {
            Spawn(playerID, levelManager.currentlevel.RandomSpawnPoint(), Quaternion.identity);
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
        if(characters[playerID] == null)
        {
            Debug.LogError("Couldn't Spawn, character is NULL");
        }

        characters[playerID].IsAlive = true;

        characters[playerID].damageable.transform.parent = pool.parent;

        var characterTransform = characters[playerID].damageable.transform;

        for(int i = 0; i <= characterTransform.childCount - 1; i++)
        {
            characterTransform.GetChild(i).SetPositionAndRotation(position, rotation);

            Debug.Log(string.Format("int = {0}, obj {1}, pos = {2}, rot = {3}",i , characterTransform.GetChild(i) , position, rotation));
        }

        /*
        characters[playerID].damageable.transform.GetChild(0).transform.position = position;
        characters[playerID].playerController.transform.rotation = rotation;
        characters[playerID].playerController.transform.position = position;
        characters[playerID].playerController.transform.rotation = rotation;
        */

    }

    public void DeSpawn(int playerID)
    {
        if (characters[playerID].IsAlive)
        {
            Debug.Log(string.Format("respawning player {0} in {1} seconds...", playerID, respawnTime));
            characters[playerID].IsAlive = false;

            //characters[playerID].MyTransform.parent = pool;

            //characters[playerID].damageable.transform.parent = pool;

            characters[playerID].ResetDamage();

            SuperInvoke.Run(() => Spawn(playerID), (respawnTime != 0 ? respawnTime : 3));
        }

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

            //if (joystick.GetAnyButtonDown())
            //if (playerMaps[i].GetButtonDown("JoinGame"))

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
    //[NonSerialized] public Transform transform;
    /*
    public Transform MyTransform
    {
        get
        {
            return (playerController.transform.root);
        }
    }
    */

    [HorizontalGroup("Group 1", LabelWidth = 2), HideLabel]
    [SerializeField] public PlayerController playerController;

    [HorizontalGroup("Group 1", LabelWidth = 2), HideLabel]
    [SerializeField] public DamageableBehaviour damageable;

    public bool IsAlive
    {
        get
        {
            return damageable.IsAlive;
        }
        set
        {
            damageable.IsAlive = value;
        }
    }

    public int CurrentLives
    {
        get
        {
            return damageable.configuration.CurrentLives;
        }
        set
        {
            damageable.configuration.SetLives(value);
        }
    }

    public int CurrentDamage
    {
        get
        {
            return damageable.configuration.CurrentDamage;
        }
        set
        {
            damageable.configuration.SetDamage(value);
        }
    }

    public bool CanKnockback(IAlignmentProvider attackerAlignment)
    {
        return damageable.CanKnockback(attackerAlignment);
    }

    public void ResetDamage()
    {
        damageable.configuration.ResetDamage();
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