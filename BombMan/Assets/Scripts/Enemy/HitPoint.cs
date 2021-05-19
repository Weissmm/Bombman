using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPoint : MonoBehaviour
{
    public bool bombAvilable;
    int dir;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (transform.position.x > collision.transform.position.x)
        {
            dir = -1;
        }
        else
            dir = 1;
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<IDamagable>().GetHit(1);
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(dir, 1) * 3, ForceMode2D.Impulse);
        }
        if (collision.CompareTag("Bomb")&& bombAvilable)
        {
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(dir, 1)*3, ForceMode2D.Impulse);
        }
    }
}
