using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cucumber : Enemy,IDamagable
{
    public Rigidbody2D rb;

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

    public void SetOff()//Animator Event
    {
        targetPoint.GetComponent<Bomb>().TurnOff();
    }
}
