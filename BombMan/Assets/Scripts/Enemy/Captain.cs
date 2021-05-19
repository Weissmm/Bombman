using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Captain : Enemy, IDamagable
{
    SpriteRenderer sprite;
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
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    public override void Update()
    {
        base.Update();
        if (animState == 0)
            sprite.flipX = false;

    }

    public override void SkillAction()
    {
        base.SkillAction();

        if (anim.GetCurrentAnimatorStateInfo(1).IsName("skill"))
        {
            sprite.flipX = true;
            if (transform.position.x > targetPoint.position.x)
            {
                transform.position = Vector2.MoveTowards(transform.position, transform.position + Vector3.right, speed * 2 * Time.deltaTime);
            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position, transform.position + Vector3.left, speed * 2 * Time.deltaTime);
            }
        }
        else
        {
            sprite.flipX = false;
        }
    }
}

