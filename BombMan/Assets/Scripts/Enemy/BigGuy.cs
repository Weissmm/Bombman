using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigGuy : Enemy, IDamagable
{
    public Rigidbody2D rb;
    public Transform pickupPoint;
    public float power;

    public void GetHit(float damage)
    {
        health -= damage;
        if (health < 1)
        {
            health = 0;
            isDead = true;
        }
        anim.SetTrigger("Hit");
    }

    public override void Init()
    {
        base.Init();
        rb = GetComponent<Rigidbody2D>();
    }

    public void PickUpBomb()//Animation event
    {
        if (targetPoint.CompareTag("Bomb")&&!hasBomb)
        {
            targetPoint.gameObject.transform.position = pickupPoint.position;

            targetPoint.SetParent(pickupPoint);

            targetPoint.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;

            hasBomb = true;
        }
    }

    public void ThrowAway()//Animation event
    {
        if (hasBomb)
        {
            targetPoint.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            targetPoint.SetParent(transform.parent.parent);

            if (FindObjectOfType<PlayerController>().gameObject.transform.position.x - transform.position.x < 0)
            {
                targetPoint.GetComponent<Rigidbody2D>().AddForce(new Vector2(-1, 1) * power, ForceMode2D.Impulse);
            }
            else
                targetPoint.GetComponent<Rigidbody2D>().AddForce(new Vector2(1, 1) * power, ForceMode2D.Impulse);
            
        }
        hasBomb = false;
    }
}
