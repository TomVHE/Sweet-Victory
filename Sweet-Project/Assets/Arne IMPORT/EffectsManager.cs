using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
//using Core.Damage;
//using Tom;

public class EffectsManager : MonoBehaviour {
/* 
	#region Variables

	[Space(10)]
	public ParticleSystem jumpParticle; 
	public ParticleSystem landParticle;
	public ParticleSystem hitParticle; 
	public ParticleSystem deathExplosionParticle;

	[Space(10)]
	public AudioSource jumpSource;
	public AudioSource landSource;
	public AudioSource hitSource;
	public AudioSource kaboomSource;

	#endregion

	void Awake ()
	{
		#region NullChecks
		if(jumpParticle == null)
		{
			Debug.LogError("jump particle is NULL");
		}
		if(landParticle == null)
		{
			Debug.LogError("land particle is NULL");
		}
		if(hitParticle == null)
		{
			Debug.LogError("hit particle is NULL");
		}
		if(deathExplosionParticle == null)
		{
			Debug.LogError("deathexplosion particle is NULL");
		}
		if(jumpSource == null)
		{
			Debug.LogError("jump source is NULL");
		}
		if(landSource == null)
		{
			Debug.LogError("land source is NULL");
		}
		if(hitSource == null)
		{
			Debug.LogError("hit source is NULL");
		}
		if(kaboomSource == null)
		{
			Debug.LogError("kaboom source is NULL");
		}
		#endregion

		#region Event Subscriber
		foreach (var player in FindObjectsOfType<DamageableBehaviour>())
		{
			player.configuration.LostLife += (damageInfo) => DeathExplosionParticle(damageInfo, player.Position);
			player.configuration.Damaged += HitParticle;
		}
		foreach (var player in FindObjectsOfType<PlayerController>())
		{
			player.JumpEvent += JumpParticle;
			player.LandEvent += LandParticle;
		}
		#endregion
	}
	#region Particles&Sound
	private void DeathExplosionParticle (DamageChangeInfo info, Vector3 deathPos)
	{
		Destroy(Instantiate(deathExplosionParticle, deathPos, Quaternion.identity), 2);
		kaboomSource.Play();	
	}
	public void HitParticle (HitInfo info)
	{
		Destroy(Instantiate(hitParticle, info.damagePoint, Quaternion.identity), 1);
		hitSource.Play();
	}
	public void JumpParticle (Vector3 pos)
	{
		Destroy(Instantiate(jumpParticle, pos, Quaternion.identity), 2);
		jumpSource.Play();
	}
	public void LandParticle (Vector3 pos)
	{
		Destroy(Instantiate(landParticle, pos, Quaternion.identity), 2);
		landSource.Play();	
	}
	#endregion*/
}