using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunEquipper : MonoBehaviour
{
    [SerializeField]
    GameUI gameUI;
    [SerializeField]
    Ammo ammo;

    public static string activeWeaponType;

    // GameObject fields to hold the gun objects
    public GameObject pistol;
    public GameObject assaultRifle;
    public GameObject shotgun;
    public GameObject robotGun;

    public GameObject activeGun;


    // Start is called before the first frame update
    void Start()
    {
        // initializing the starting gun to be the pistol
        activeWeaponType = Constants.Pistol;
        LoadWeapon(pistol);
    }


    // Update is called once per frame
    void Update()
    {
        // checking which key has been pressed and changing the active weapon accordingly
        if (Input.GetKeyDown("1"))
        {
            LoadWeapon(pistol);
            activeWeaponType = Constants.Pistol;
            gameUI.UpdateReticle();
        }
        else if (Input.GetKeyDown("2"))
        {
            LoadWeapon(assaultRifle);
            activeWeaponType = Constants.AssaultRifle;
            gameUI.UpdateReticle();
        }
        else if (Input.GetKeyDown("3"))
        {
            LoadWeapon(shotgun);
            activeWeaponType = Constants.Shotgun;
            gameUI.UpdateReticle();
        }
        else if (Input.GetKeyDown("4"))
        {
            LoadWeapon(robotGun);
            activeWeaponType = Constants.RobotGun;
            gameUI.UpdateReticle();
        }
    }

    public GameObject GetActiveWeapon()
    {
        return activeGun;
    }

    private void LoadWeapon(GameObject weapon)
    {
        pistol.SetActive(false);
        assaultRifle.SetActive(false);
        shotgun.SetActive(false);
        robotGun.SetActive(false);

        weapon.SetActive(true);
        activeGun = weapon;

        gameUI.SetAmmoText(ammo.GetAmmo(activeGun.tag));
    }
}
