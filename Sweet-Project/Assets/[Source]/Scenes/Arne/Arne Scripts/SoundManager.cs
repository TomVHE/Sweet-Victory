using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Core.Damage;
using Tom;

public class SoundManager : MonoBehaviour
{
	public AudioClip jump;
	public AudioClip land;
	public AudioClip hit;
	public AudioClip kaboom;

    [Space(10)]
	public AudioSource jumpSource;
	public AudioSource landSource;
	public AudioSource hitSource;
	public AudioSource kaboomSource;

	void Awake ()
	{
		//subscribe on events
		foreach (var player in FindObjectsOfType<DamageableBehaviour>())
		{
			player.configuration.LostLife += PlayKaboom;
			player.configuration.Damaged += PlayHit;
		}
		foreach (var player in FindObjectsOfType<PlayerController>())
		{
			player.JumpEvent += PlayJump;
			player.LandEvent += PlayJump;
		}
	}
	//play jump sound
	public void PlayJump (Vector3 pos)
	{
        //jumpSource.transform.position = pos;
		jumpSource.Play();
	}
	//plays land sound
	public void PlayLand (Vector3 pos)
	{
		landSource.Play();	
	}
	//plays hit sound
	public void PlayHit (HitInfo info)
	{
		hitSource.Play();
	}
	//plays kaboom sound
	public void PlayKaboom (DamageChangeInfo info)
	{
		kaboomSource.Play();	
	}
}
