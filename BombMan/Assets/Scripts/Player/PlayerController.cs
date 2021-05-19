using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour,IDamagable
{
    private Rigidbody2D rb;
    private Animator anim;
    private FixedJoystick joystick;

    public float speed;
    public float jumpForce;

    [Header("Player State")]
    public float Health;
    public bool isDead;
    public bool isHurt;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask groundLayer;

    [Header("States Check")]
    public bool isGround;
    public bool canJump;
    public bool isJump;

    [Header("Jump FX")]
    public GameObject jumpFX;
    public GameObject landFX;

    [Header("Attack Settings")]
    public GameObject bombPrefab;
    public float nextAttack = 0;
    public float attackRate;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        joystick = FindObjectOfType<FixedJoystick>();

        GameManager.instance.IsPlayer(this);

        Health = GameManager.instance.LoadHealth();
        UIManager.instance.UpdateHealth(Health);
    }

    // Update is called once per frame
    void Update()
    {
        showRaycast();
        anim.SetBool("Dead",isDead);
        if (isDead)
            return;
        isHurt = anim.GetCurrentAnimatorStateInfo(1).IsName("Player_Hit");
        CheckInput();
    }

    public void showRaycast()
    {
        Vector2 pos = transform.position;
        Vector2 Setoff = new Vector2(-0.2f, 0);

        Physics2D.Raycast(pos + Setoff, Vector2.down, 10f);
        Debug.DrawRay(pos + Setoff, Vector2.down, Color.red);
    }

    private void FixedUpdate()
    {
        if (isDead)
        {
            rb.velocity = Vector2.zero;
            return;
        }
        Jump();
        PhysicsCheck();
        if (!isHurt)
        {
            Movement();
        }
    }

    void CheckInput()
    {
        if (Input.GetButtonDown("Jump") && isGround)
        {
            canJump = true;
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            Attack();
        }
    }

    void Movement()
    {
        //¼üÅÌ²Ù×÷
        //float horizontalInput = Input.GetAxisRaw("Horizontal");

        //²Ù×÷¸Ë
        float horizontalInput = joystick.Horizontal;

        rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);

        //if (horizontalInput != 0)
        //{
        //    transform.localScale = new Vector3(horizontalInput, 1, 1);
        //    //Debug.Log(rb.velocity.y);
        //}
        if (horizontalInput > 0)
            transform.eulerAngles = new Vector3(0, 0, 0);
        if (horizontalInput < 0)
            transform.eulerAngles = new Vector3(0, 180, 0);
    }

    void Jump()
    {
        if (canJump)
        {
            isJump = true;
            jumpFX.SetActive(true);
            jumpFX.transform.position = transform.position+new Vector3(0,-0.45f,0);
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            canJump = false;
        }
    }

    public void CanJump()
    {
        if (isGround)
        {
            canJump = true;
        }
    }

    public void Attack()
    {
        if (Time.time > nextAttack)
        {
            Instantiate(bombPrefab, transform.position, bombPrefab.transform.rotation);

            nextAttack = Time.time + attackRate;
        }
    }

    void PhysicsCheck()
    {
        isGround = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
        if (isGround)
        {
            rb.gravityScale = 1;
            canJump = false;
            isJump = false;
        }
        else
        {
            isJump = true;
            rb.gravityScale = 4;
        }
    }

    public void LandFX()
    {
        landFX.SetActive(true);
        landFX.transform.position = transform.position + new Vector3(0,-0.75f,0);
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
    }

    public void GetHit(float damage)
    {
        if (!anim.GetCurrentAnimatorStateInfo(1).IsName("Player_Hit"))
        {
            Health -= damage;
            if (Health < 1)
            {
                Health = 0;
                isDead = true;
            }
            anim.SetTrigger("Hit");

            UIManager.instance.UpdateHealth(Health);
        }
    }
}
