using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelecter : MonoBehaviour {

    [SerializeField]
    private int index;

    private void OnTriggerEnter(Collider other)
    {
        GameManager.LevelManager.Voted(index);
    }

    private void OnTriggerExit(Collider other)
    {
        GameManager.LevelManager.UnVoted(index);
    }
}
