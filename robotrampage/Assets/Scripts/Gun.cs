using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public float fireRate;
    protected float lastFireTime;

    public Ammo ammo;
    public AudioClip liveFire;
    public AudioClip dryFire;

    public float zoomFactor;
    public int range;
    public int damage;

    private float zoomFOV;
    private float zoomSpeed = 6;

    //holds the player's height
    private float height;

    // Start is called before the first frame update
    void Start()
    {
        zoomFOV = Constants.CameraDefaultZoom / zoomFactor;
        lastFireTime = Time.time - 10;

        height = this.transform.position.y;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //making sure the player always stays at the same height (will sometimes go into the air when colliding with robots)
        this.transform.position = new Vector3(this.transform.position.x,
                                          height,
                                          this.transform.position.z);
        //zooms the camera (aims down sights)
        if (Input.GetMouseButton(1))
        {
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, zoomFOV, zoomSpeed * Time.deltaTime);
        }
    }

    protected void Fire()
    {
        //checking to see if the current gun has ammo
        if (ammo.HasAmmo(tag))
        {
            // if it does then play the regular fire sound clip
            GetComponent<AudioSource>().PlayOneShot(liveFire);
            GetComponentInChildren<Animator>().Play("Fire");
            ammo.ConsumeAmmo(tag);
        }
        else
        {
            // otherwise play the empty fire clip
            GetComponent<AudioSource>().PlayOneShot(dryFire);
        }

        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, range))
        {
            ProcessHit(hit.collider.gameObject);
        }
    }

    private void ProcessHit(GameObject hitObject)
    {
        if (hitObject.GetComponent<Player>() != null)
        {
            hitObject.GetComponent<Player>().TakeDamage(damage);
        }

        if (hitObject.GetComponent<Robot>() != null)
        {
            hitObject.GetComponent<Robot>().TakeDamage(damage);
        }
    }
}
