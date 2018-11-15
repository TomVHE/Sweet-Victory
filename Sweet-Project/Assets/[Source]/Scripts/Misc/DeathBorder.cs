using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Damage;

public class DeathBorder : MonoBehaviour
{
    //public MultipleTargetCamera camera;

    private LevelManager levelManager;
    private PlayerPool playerPool;

    private bool[] respawning = new bool[4];
    //public List<Transform> respawnPosition = new List<Transform>();

    private void Awake()
    {
        levelManager = FindObjectOfType<LevelManager>();
        playerPool = FindObjectOfType<PlayerPool>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            var damageableBehaviour = other.GetComponentInChildren<DamageableBehaviour>();
            if(damageableBehaviour == null) { damageableBehaviour = other.GetComponentInParent<DamageableBehaviour>(); }

            int playerID = damageableBehaviour.configuration.playerID;

            Debug.Log(string.Format("Player {0}'s {1} entered trigger", playerID, other.name));

            if (damageableBehaviour.IsAlive)// && respawning[playerID] == false)
            {
                respawning[playerID] = true;

                damageableBehaviour.configuration.SubtractLives(1, new DamageChangeInfo());

                playerPool.DeSpawn(playerID);
            }

            return;
        }
        else if(other.tag == "Object")
        {
            other.transform.position = levelManager.currentlevel.RandomSpawnPoint();
            return;
        }
	}

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            var damageableBehaviour = other.GetComponentInChildren<DamageableBehaviour>();
            if (damageableBehaviour == null) { damageableBehaviour = other.GetComponentInParent<DamageableBehaviour>(); }

            int playerID = damageableBehaviour.configuration.playerID;

            respawning[playerID] = false;
        }
    }

}
