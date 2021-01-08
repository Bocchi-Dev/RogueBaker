using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardKnockback : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameController.instance.playerHurt();

            //knockback
            var player = collision.gameObject.GetComponent<PlayerController>();
            player.knockbackCount = player.knockbackLength;

            //check if hit from left or right
            if (collision.transform.position.x < transform.position.x)
            {
                player.knockbackFromRight = true;
            }
            else
            {
                player.knockbackFromRight = false;
            }
        }
    }
}
