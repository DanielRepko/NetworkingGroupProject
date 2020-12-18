using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    // SerializeField makes the properties accessible from the inspector but not other scripts
    [SerializeField]
    Sprite redReticle;
    [SerializeField]
    Sprite yellowReticle;
    [SerializeField]
    Sprite blueReticle;
    [SerializeField]
    Image reticle;

    //fields for all of the ui text
    [SerializeField]
    private Text ammoText;
    [SerializeField]
    private Text healthText;
    [SerializeField]
    private Text armorText;
    [SerializeField]
    private Text scoreText;
    [SerializeField]
    private Text pickupText;
    [SerializeField]
    private Text waveText;
    [SerializeField]
    private Text enemyText;
    [SerializeField]
    private Text waveClearText;
    [SerializeField]
    private Text newWaveText;    
    [SerializeField]
    Player player;

    // changes the reticle being used
    public void UpdateReticle()
    {
        switch (GunEquipper.activeWeaponType)
        {
            case Constants.Pistol:
                reticle.sprite = redReticle;
                break;
            case Constants.Shotgun:
                reticle.sprite = yellowReticle;
                break;
            case Constants.AssaultRifle:
                reticle.sprite = blueReticle;
                break;
            case Constants.RobotGun:
                reticle.sprite = redReticle;
                break;
            default:
                return;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        SetArmorText(player.armor);
        SetHealthText(player.health);
    }

    // Update is called once per frame
    void Update()
    {

    }

    // these methods set the text for the various ui text objects
    public void SetArmorText(int armor)
    {
        armorText.text = "Armor: " + armor;
    }

    public void SetHealthText(int health)
    {
        healthText.text = "Health: " + health;
    }

    public void SetAmmoText(int ammo)
    {
        ammoText.text = "Ammo: " + ammo;
    }

    public void SetScoreText(int score)
    {
        scoreText.text = "" + score;
    }
    public void SetWaveText(int time)
    {
        waveText.text = "Next Wave: " + time;
    }
    public void SetEnemyText(int enemies)
    {
        enemyText.text = "Enemies: " + enemies;
    }    

    public void ShowWaveClearBonus()
    {
        waveClearText.GetComponent<Text>().enabled = true;
        StartCoroutine("hideWaveClearBonus");
    }

    IEnumerator hideWaveClearBonus()
    {
        yield return new WaitForSeconds(4);
        waveClearText.GetComponent<Text>().enabled = false;
    }

    public void SetPickupText(string text)
    {
        pickupText.GetComponent<Text>().enabled = true;
        pickupText.text = text;
        StopCoroutine("hidePickupText");
        StartCoroutine("hidePickupText");
    }

    IEnumerator hidePickupText()
    {
        yield return new WaitForSeconds(4);
        pickupText.GetComponent<Text>().enabled = false;
    }

    public void ShowNewWaveText()
    {
        StartCoroutine("hideNewWaveText");
        newWaveText.GetComponent<Text>().enabled = true;
    }

    IEnumerator hideNewWaveText()
    {
        yield return new WaitForSeconds(4);
        newWaveText.GetComponent<Text>().enabled = false;
    }

}
