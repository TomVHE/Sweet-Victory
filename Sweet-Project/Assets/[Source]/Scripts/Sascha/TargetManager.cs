using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour {

	public MultipleTargetCamera cameraScript;

	public void OnTriggerEnter (Collider other)
	{
		if (this.tag == "AddToList" &&  other.tag == "Player")
		{
			if(!cameraScript.targets.Contains(other.gameObject))
			cameraScript.targets.Add(other.gameObject);
		}
		if (this.tag == "RemoveFromList" && other.tag == "Player")
		{
			if(cameraScript.targets.Contains(other.gameObject))
			cameraScript.targets.Remove(other.gameObject);
		}
	}
}
