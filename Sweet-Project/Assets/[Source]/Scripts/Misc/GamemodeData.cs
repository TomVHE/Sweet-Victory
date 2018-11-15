using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "GamemodeData", menuName = "Assets/Create/GamemodeData", order = 1)]
public class GameModeData : ScriptableObject
{
    public float respawnTime = 3f;
    public int maxLives = 3;
}