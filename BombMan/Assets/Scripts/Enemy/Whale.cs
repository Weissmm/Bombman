using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whale : Enemy, IDamagable
{
    public Rigidbody2D rb;

    public float scale;
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

    public void Swalow()//Animation event
    {
        targetPoint.GetComponent<Bomb>().TurnOff();
        targetPoint.gameObject.SetActive(false);
        transform.localScale *= scale;
    }
}

