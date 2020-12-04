using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssaultRifle : Gun
{
    // Update is called once per frame
    override protected void Update()
    {
        base.Update();
        //Shotgun & Pistol have semi auto fire rate
        if (Input.GetMouseButton(0) && (Time.time - lastFireTime > fireRate))
        {
            lastFireTime = Time.time;
            Fire();
        }
    }
}
