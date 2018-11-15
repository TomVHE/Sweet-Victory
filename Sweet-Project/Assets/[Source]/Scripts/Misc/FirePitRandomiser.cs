using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePitRandomiser : MonoBehaviour {

	private List<Transform> firepits = new List<Transform>();

	private int randomPitID;

	private ParticleSystem firepitParticle;

	private GameObject firepitObject;

	public bool go, resetlist;

	public float minFireTime, maxFireTime, randomFloat;


	void Awake ()
	{
		MakeList();
		firepitObject = GameObject.Find("Firepit System");
		firepitParticle = firepitObject.GetComponent<ParticleSystem>();
		NewFire();
	}
	void Update ()
	{
		FireTimer();
	}
	void MakeList ()
	{
		foreach (var item in GameObject.FindGameObjectsWithTag("FirePit"))
		{
			firepits.Add(item.transform);
		}
	}
	void FireTimer ()
	{
		randomFloat -= Time.deltaTime;
		if(randomFloat <= 0f)
		{
			NewFire();
		}
	}
	void NewFire ()
	{
		//particle should die out turn loop off
		firepitParticle.Stop();
		if(firepitObject.transform.parent != null)
		{
			firepitObject.transform.parent = null;
		}
		randomPitID = Random.Range(0,5); //we only have 6 pits
		Mathf.RoundToInt(randomPitID);
		firepitParticle.gameObject.transform.SetParent(firepits[randomPitID].transform);
		ResetPosition();
		firepitParticle.Play();
		//start loop particle 
		randomFloat = Random.Range(minFireTime,maxFireTime);
	}
	void ResetPosition ()
	{
		print("reset");
		firepitObject.transform.localPosition = new Vector3(0,0,0);
	}
	void ResetList () 
	{
		firepits.Clear();
	}
}
