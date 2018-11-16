using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Core.Damage;
using Tom;

public class EffectsManager : DestroyableSingleton<EffectsManager>
{
	#region Variables

	[Space(10)]
	public ParticleSystem jumpParticle; 
	public ParticleSystem landParticle;
	public ParticleSystem hitParticle; 
	public ParticleSystem deathExplosionParticle;

	[Space(10)]
	public AudioSource jumpSource;
	//public AudioSource punchSource;
	public AudioSource hitSource;
	public AudioSource kaboomSource;

	#endregion

    public void Subscribe(DamageableBehaviour player)
    {
        player.configuration.LostLife += (damageInfo) => DeathExplosionParticle(damageInfo, player.Position);
        //player.configuration.Damaged += HitParticle;
        player.configuration.Damaged += HitParticle;

        PlayerController controller = player.transform.GetComponentInChildren<PlayerController>();

        controller.JumpEvent += JumpParticle;
        controller.LandEvent += LandParticle;
    }

	#region Particles&Sound
	private void DeathExplosionParticle (DamageChangeInfo info, Vector3 deathPos)
	{
		Destroy(Instantiate(deathExplosionParticle, deathPos, Quaternion.identity).gameObject, 2);
		kaboomSource.Play();
	}
    public void HitParticle(HitInfo info)
    {
        Destroy(Instantiate(hitParticle, info.damagePoint, Quaternion.identity).gameObject, 1);
		hitSource.Play();
    }
	public void JumpParticle (Vector3 pos)
	{
        Destroy(Instantiate(jumpParticle, pos, Quaternion.identity).gameObject, 1);
		jumpSource.Play();
	}
	public void LandParticle (Vector3 pos)
	{
		Destroy(Instantiate(landParticle, pos, Quaternion.identity).gameObject, 2);
	}
	#endregion
}