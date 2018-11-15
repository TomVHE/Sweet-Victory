using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Utilities;

public class StartingLight : DestroyableSingleton<StartingLight>
{
	public Color red, green;

	public List<GameObject> lampsList = new List<GameObject>();

	protected override void Awake () 
	{
		ResetLights();
	}
	public void ChangeLight (int amount)
	{
		for (int i = 0; i < amount; i++)
		{
            Material material = lampsList[i].GetComponent<Renderer>().material; 
			material.color = green;
		}
		// Play beep sound like mario kart
	}
	public void ResetLights ()
	{
		for(int i = 0; i < lampsList.Count; i++)
		{
            Material material = lampsList[i].GetComponent<Renderer>().material;
			material.color = red;
		}
	}
}
