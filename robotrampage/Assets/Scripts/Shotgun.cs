using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Gun
{
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
                Fire();
            }
        }
        else if(Input.GetMouseButtonDown(0))
        {
            Fire();
        }
    }
}
