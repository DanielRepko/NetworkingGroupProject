using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;

public class Ammo : MonoBehaviour
{
    [SerializeField]
    GameUI gameUI;

    [SerializeField]
    private int pistolAmmo = 20;
    [SerializeField]
    private int shotgunAmmo = 10;
    [SerializeField]
    private int assaultRifleAmmo = 50;
    [SerializeField]
    private int robotGunAmmo = 15;

    // Dictionary is a list containing key value pairs, with this one having a string key for the gun type, and an int value for the amount of ammo
    public Dictionary<string, int> tagToAmmo;

    public void Awake()
    {
        tagToAmmo = new Dictionary<string, int>
        {
            { Constants.Pistol, pistolAmmo },
            { Constants.Shotgun, shotgunAmmo},
            { Constants.AssaultRifle, assaultRifleAmmo },
            { Constants.RobotGun, robotGunAmmo }
        };
    }

    public void AddAmmo(string tag, int ammo)
    {
        if (!tagToAmmo.ContainsKey(tag))
        {
            Debug.LogError("Unrecognized gun type passed: " + tag);
        }

        tagToAmmo[tag] += ammo;
    }

    // used to check if a certain weapon has any ammo
    public bool HasAmmo(string tag)
    {
        if (!tagToAmmo.ContainsKey(tag))
        {
            Debug.LogError("Unrecognized gun type passed: " + tag);
        }

        return tagToAmmo[tag] > 0;
    }

    // used to get how much ammo a certain weapon has
    public int GetAmmo(string tag)
    {
        if (!tagToAmmo.ContainsKey(tag))
        {
            Debug.LogError("Unrecognized gun type passed: " + tag);
        }

        return tagToAmmo[tag];
    }

    // used to consume ammo for a certain weapon
    public void ConsumeAmmo(string tag)
    {
        if (!tagToAmmo.ContainsKey(tag))
        {
            Debug.LogError("Unrecognized gun type passed: " + tag);
        }

        tagToAmmo[tag]--;
        gameUI.SetAmmoText(tagToAmmo[tag]);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
