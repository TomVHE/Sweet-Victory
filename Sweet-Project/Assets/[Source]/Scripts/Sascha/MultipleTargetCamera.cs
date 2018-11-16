using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class MultipleTargetCamera : MonoBehaviour {

	public List<GameObject> targets = new List<GameObject>();
	public Vector3 offset = new Vector3(0,6,-20);
	private Vector3 _Velocity;
	private PlayerPool playerPool;
	private Camera cam;	
	public float rotationSpeed = 5f, movementSpeed = 1f;
	public float maxZoom = 10, minZoom = 60, zoomLimiter = 10f;

	void Start ()
	{
		cam = GetComponent<Camera>();
		playerPool = FindObjectOfType<PlayerPool>();
	}

	void LateUpdate ()
	{
		if (targets.Count == 0)
		{
			return;
		}

		Move();
		Zoom();
	}
	
	void Move ()
	{
		Vector3 centerPoint = GetCenterPoint ();

		Vector3 newPosition = centerPoint + offset;

		transform.position = Vector3.SmoothDamp (transform.position, newPosition, ref _Velocity, movementSpeed);

		var rotation = Quaternion.LookRotation (centerPoint - transform.position);
        transform.rotation = Quaternion.Slerp (transform.rotation, rotation, Time.deltaTime * rotationSpeed);
	
	}

	void Zoom ()
	{
		float newZoom = Mathf.Lerp(maxZoom, minZoom, GetGreatestDistance() / zoomLimiter);
		cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, newZoom, Time.deltaTime);
	}

	private GameObject GetPlayer (int playerID)
	{
        //GameObject playerRoot  = playerPool.characters[playerID].MyTransform.root.gameObject;
        GameObject playerRoot = playerPool.players[playerID].movementController.root.gameObject;
        return (playerRoot);
	}

	/*void CheckPlayerStatus(int playerID)
	{
		var player = GetPlayer(playerID);
		
		if(healthManager.players[playerID].alive == false)
		{
			targets.Remove(targets[playerID]);
		}
		else if(!targets.Contains(player))
		{
			targets.Add(player);
		}
	}*/

	float GetGreatestDistance ()
	{
		var bounds = new Bounds (targets[0].transform.position, Vector3.zero);
		for (int i = 0; i < targets.Count; i++)
		{
			bounds.Encapsulate (targets[i].transform.position);
		}
		return bounds.size.x;
	}

	Vector3 GetCenterPoint ()
	{
		if (targets.Count == 1)
		{
			return targets[0].transform.position;
		}
		
		var bounds = new Bounds(targets[0].transform.position, Vector3.zero);
		for (int i = 0; i < targets.Count; i++)
		{
			bounds.Encapsulate (targets[i].transform.position);
		}

		return bounds.center;
	}
}
