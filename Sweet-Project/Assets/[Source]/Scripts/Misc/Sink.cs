using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sink : MonoBehaviour {

	public Animator sinkAnimator;
	public GameObject waterFill;
	public bool isActive;

	void Start ()
	{
		sinkAnimator = GetComponent<Animator>();
	}

	void GoDown ()
	{
		sinkAnimator.SetTrigger("GoDown");
	}

	void GoUp ()
	{
		sinkAnimator.SetTrigger("GoUp");
	}

	public void Update()
	{	
		waterFill.SetActive(isActive);
			//waterFill.SetActive(!waterFill.activeInHierarchy);
	}
}
