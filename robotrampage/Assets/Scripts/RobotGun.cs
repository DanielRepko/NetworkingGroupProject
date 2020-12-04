using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotGun : Gun
{
    public GameObject missilePrefab;
    public Transform missileFireSpot;

    // Update is called once per frame
    override protected void Update()
    {
        base.Update();
        //Shotgun & Pistol have semi auto fire rate       
        // added additional code to make it so that the gun will ignore the fire rate if it is empty,
        // this the dry fire sound to not be limited by the fire rate
        if (ammo.HasAmmo(tag))
        {
            if (Input.GetMouseButtonDown(0) && (Time.time - lastFireTime > fireRate))
            {
                lastFireTime = Time.time;
                FireMissile();
            }
        }
        else if (Input.GetMouseButtonDown(0))
        {
            FireMissile();
        }
    }

    void FireMissile()
    {
        //checking to see if the current gun has ammo
        if (ammo.HasAmmo(tag))
        {
            //instantiating the missile
            GameObject missile = Instantiate(missilePrefab);
            //setting the damage of the missle
            missile.GetComponent<Missile>().shotByPlayer = true;
            missile.GetComponent<Missile>().damage = 5;
            //setting the rotation and position of the missile to that of the missileFireSpot
            missile.transform.position = missileFireSpot.position;
            missile.transform.rotation = missileFireSpot.rotation;
            GetComponentInChildren<Animator>().Play("Fire");
            GetComponent<AudioSource>().PlayOneShot(liveFire);
            ammo.ConsumeAmmo(tag);
        }
        else
        {
            // otherwise play the empty fire clip
            GetComponent<AudioSource>().PlayOneShot(dryFire);
        }
    }
}
