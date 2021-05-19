using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    private Animator anim;
    private Collider2D coll;
    private Rigidbody2D rb;

    public float startTime;
    public float waitTime;
    public float bombForce;

    [Header("Check")]
    public float radius;
    public LayerMask targetLayer;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        startTime = Time.time;
        coll = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Bomb_off"))
        {
            if (Time.time > startTime + waitTime)
            {
                anim.Play("Bomb_explotion");
            }
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    public void Explotion()
    {
        coll.enabled = false;

        Collider2D[] aroundObjects = Physics2D.OverlapCircleAll(transform.position, radius, targetLayer);

        rb.gravityScale = 0;

        foreach (var item in aroundObjects)
        {
            Vector3 pos = transform.position - item.transform.position;

            item.GetComponent<Rigidbody2D>().AddForce((-pos+Vector3.up) * bombForce, ForceMode2D.Impulse);

            if (item.CompareTag("Bomb")&& item.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Bomb_off"))
            {
                item.GetComponent<Bomb>().TurnOn();
            }

            if (item.CompareTag("Player"))
                item.GetComponent<IDamagable>().GetHit(1);
        }
    }

    public void DestroyThis()
    {
        Destroy(gameObject);
    }

    public void TurnOff()
    {
        anim.Play("Bomb_off");
        gameObject.layer = LayerMask.NameToLayer("NPC");
    }

    public void TurnOn()
    {
        startTime = Time.time;
        anim.Play("Bombon");
        gameObject.layer = LayerMask.NameToLayer("Bomb");
    }
}
