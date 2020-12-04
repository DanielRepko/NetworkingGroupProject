using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    public float speed = 30f;
    public int damage = 10;

    public bool shotByPlayer = false;

    // Start is called before the first frame update
    void Start()
    {
        // executes the DeathTimer coroutine
        StartCoroutine("DeathTimer");
    }

    // Update is called once per frame
    void Update()
    {
        //moving the missile forward
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    //destroys the missile after 10 seconds
    IEnumerator DeathTimer()
    {
        yield return new WaitForSeconds(10);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //check if robot shooting and hit player
        if (collision.gameObject.GetComponent<Player>() != null && collision.gameObject.tag == "Player" && !shotByPlayer)
        {
            collision.gameObject.GetComponent<Player>().TakeDamage(damage);
            Destroy(gameObject);
        }
        //check if player shooting and hit robot
        else if (shotByPlayer && collision.gameObject.GetComponent<Robot>() != null)
        {
            collision.gameObject.GetComponent<Robot>().TakeDamage(damage);
            Destroy(gameObject);
        }
        //destroying missile if it hits a wall
        else if (collision.gameObject.tag == "Wall")
        {
            Destroy(gameObject);
        }
    }
}
