using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonTrigger : MonoBehaviour
{
    public float timerShoot = 5f;
    public PirateShip ship;
    private float timer;

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            timer += Time.deltaTime;
            if(timer >= timerShoot)
            {
                ship.ShootLeftCannon();
                timer = 0f;
            }
        }
    }
}
