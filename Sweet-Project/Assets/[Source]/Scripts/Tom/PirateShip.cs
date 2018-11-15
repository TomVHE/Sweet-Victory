using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PirateShip : MonoBehaviour
{
    public ParticleSystem rightCannon;
    public ParticleSystem leftCannon;

    public void ShootRightCannon()
    {
        rightCannon.Play();
    }

    public void ShootLeftCannon()
    {
        GetComponent<AudioSource>().Play();
        leftCannon.Play();
    }
}
