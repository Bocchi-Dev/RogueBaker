using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    private Animator anime;

    public int damage;

    public Transform hitPosition;
    public float attackRange;
    public LayerMask whatIsEnemy;
    // Start is called before the first frame update
    void Start()
    {
        anime = GetComponent<Animator>();
    }

    public void Attack()
    {
        anime.SetTrigger("Attack");
        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(hitPosition.position, attackRange, whatIsEnemy);
        for (int i = 0; i < enemiesToDamage.Length; i++)
        {
            if (enemiesToDamage[i].gameObject.tag == "Enemy")
            {
                FindObjectOfType<AudioManager>().Play("Hit");
                enemiesToDamage[i].GetComponent<Enemy>().TakeDamage(damage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(hitPosition.position, attackRange);
    }
}
