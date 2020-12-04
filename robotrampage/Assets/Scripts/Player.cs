using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Game game;
    public AudioClip playerDead;

    public int health;
    public int armor;
    public GameUI gameUI;
    private GunEquipper gunEquipper;
    private Ammo ammo;

    private float height;

    // Start is called before the first frame update
    void Start()
    {
        ammo = GetComponent<Ammo>();
        gunEquipper = GetComponent<GunEquipper>();

        height = transform.position.y; // initializing the height
    }

    // Update is called once per frame
    void Update()
    {
        // setting the player's height each frame because they will sometimes get put into the air when colliding with robots
        transform.position = new Vector3(transform.position.x,
                                         height,
                                         transform.position.z);
    }

    public void TakeDamage(int amount)
    {
        int healthDamage = amount;
        if(armor > 0)
        {
            int effectiveArmor = armor * 2;
            effectiveArmor -= healthDamage;

            if(effectiveArmor > 0)
            {
                armor = effectiveArmor / 2;
                gameUI.SetArmorText(armor);
                return;
            }
            armor = 0;
            gameUI.SetArmorText(armor);
        }
        health -= healthDamage;
        gameUI.SetHealthText(health);

        if (health <= 0)
        {
            GetComponent<AudioSource>().PlayOneShot(playerDead);
            game.GameOver();
        }
    }

    private void PickUpHealth()
    {
        health += 50;
        if (health > 200)
        {
            health = 200;
        }
        //updating the health text after picking up health
        gameUI.SetPickupText("Health picked up + 50 health");
        gameUI.SetHealthText(health);
    }

    private void PickUpArmor()
    {
        armor += 15;
        gameUI.SetPickupText("Armor picked up + 15 armor");
        gameUI.SetArmorText(armor);
    }

    private void PickUpPistolAmmo()
    {
        ammo.AddAmmo(Constants.Pistol, 20);
        gameUI.SetPickupText("Pistol ammo picked up + 20 ammo");
        if (gunEquipper.GetActiveWeapon().tag == Constants.Pistol)
        {
            gameUI.SetAmmoText(ammo.GetAmmo(Constants.Pistol));
        }
    }

    private void PickUpAssaultRifleAmmo()
    {
        ammo.AddAmmo(Constants.AssaultRifle, 50);
        gameUI.SetPickupText("Assault rifle ammo picked up + 50 ammo");
        if (gunEquipper.GetActiveWeapon().tag == Constants.AssaultRifle)
        {
            gameUI.SetAmmoText(ammo.GetAmmo(Constants.AssaultRifle));
        }
    }

    private void PickUpShotgunAmmo()
    {
        ammo.AddAmmo(Constants.Shotgun, 10);
        gameUI.SetPickupText("Shotgun ammo picked up + 10 ammo");
        if (gunEquipper.GetActiveWeapon().tag == Constants.Shotgun)
        {
            gameUI.SetAmmoText(ammo.GetAmmo(Constants.Shotgun));
        }
    }

    public void PickUpItem(int pickupType)
    {
        switch (pickupType)
        {
            case Constants.PickUpArmor:
                PickUpArmor();
                break;
            case Constants.PickUpHealth:
                PickUpHealth();
                break;            
            case Constants.PickUpAssaultRifleAmmo:
                PickUpAssaultRifleAmmo();
                break;
            case Constants.PickUpPistolAmmo:
                PickUpPistolAmmo();
                break;
            case Constants.PickUpShotgunAmmo:
                PickUpShotgunAmmo();
                break;
            default:
                Debug.LogError("Bad pickup type passed: " + pickupType);
                break;
        }
    }
}
