using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Core.Damage;
using Tom;


public class ParticleManager : MonoBehaviour
{
	public ParticleSystem jumpParticle; 
	public ParticleSystem landParticle;
	public ParticleSystem hitParticle; 
	public ParticleSystem deathExplosionParticle;

	void Awake ()
	{
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
	}
	private void DeathExplosionParticle (DamageChangeInfo info, Vector3 deathPos)
	{
		Destroy(Instantiate(deathExplosionParticle, deathPos, Quaternion.identity), 2);
	}
	public void HitParticle (HitInfo info)
	{
		Destroy(Instantiate(hitParticle, info.damagePoint, Quaternion.identity), 1);
	}
	public void JumpParticle (Vector3 pos)
	{
		Destroy(Instantiate(jumpParticle, pos, Quaternion.identity), 2);
	}
	public void LandParticle (Vector3 pos)
	{
		Destroy(Instantiate(landParticle, pos, Quaternion.identity), 2);
	}
}
