using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class LevelSelector : MonoBehaviour
{
    [NonSerialized] public List<GameObject> players = new List<GameObject>();

    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            if (!players.Contains(col.gameObject))
            {
                players.Add(col.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider col)
    {
        players.Remove(col.gameObject);
    }
}