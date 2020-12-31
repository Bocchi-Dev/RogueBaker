using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed;
    public float lifeTime;

    public GameObject bulletDestroyEffect;

    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<AudioManager>().Play("Shoot");
        Invoke("DestroyBullet", lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    void DestroyBullet()
    {
        Instantiate(bulletDestroyEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameController.instance.playerHurt();

            //knockback
            var player = collision.GetComponent<PlayerController>();
            player.knockbackCount = player.knockbackLength;

            //check if hit from left or right
            if(collision.transform.position.x < transform.position.x)
            {
                player.knockbackFromRight = true;
            }
            else
            {
                player.knockbackFromRight = false;
            }
            DestroyBullet();
        }

        if (collision.gameObject.tag == "Weapon")
        {
            DestroyBullet();
        }
    }
}
