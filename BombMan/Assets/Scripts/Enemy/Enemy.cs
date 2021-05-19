using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    EnemyBaseState currentState;

    public Animator anim;
    public int animState;

    private GameObject alarmSign;


    [Header("Base State")]
    public float health;
    public bool isDead;
    public bool hasBomb;
    public bool isBoss;

    [Header("Movement")]
    public float speed;
    public Transform pointA, pointB;
    public Transform targetPoint;

    [Header("Attack Setting")]
    private float nextAttack = 0;
    public float attackRate;
    public float attackRange, skillRange;

    public List<Transform> attackList = new List<Transform>();

    public potrolState potrolstate = new potrolState();
    public attackState attackstate = new attackState();

    public virtual void Init()
    {
        anim = GetComponent<Animator>();
        alarmSign = transform.GetChild(0).gameObject;

    }

    public void Awake()
    {
        Init();
    }

    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.IsEnemy(this);
        TransitionToState(potrolstate);
        if (isBoss)
            UIManager.instance.SetBossHealth(health);
    }

    // Update is called once per frame
    public virtual void Update()
    {
        anim.SetBool("Dead", isDead);
        if (isBoss)
            UIManager.instance.UpdateBossHealth(health);
        if (isDead)
        {
            GameManager.instance.EnemyDead(this);
            return;
        }
        currentState.OnUpdate(this);
        anim.SetInteger("State", animState);
    }

    public void TransitionToState(EnemyBaseState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }

    public void MoveToTarget()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);
        FilpDirection();
    }

    public void AttackAction()//攻击玩家
    {
        if (Vector2.Distance(transform.position, targetPoint.position) < attackRange)
        {
            if (Time.time > nextAttack)
            {
                //播放攻击动画
                anim.SetTrigger("Attack");
                nextAttack = Time.time + attackRate;
            }
        }
    }

    public virtual void SkillAction()//对炸弹使用技能
    {
        if (Vector2.Distance(transform.position, targetPoint.position) < skillRange)
        {
            if (Time.time > nextAttack)
            {
                //播放攻击动画
                anim.SetTrigger("Skill");
                nextAttack = Time.time + attackRate;
            }
        }
    }

    public void FilpDirection()
    {
        if (transform.position.x < targetPoint.position.x)
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        else
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
    }

    public void SwitchPoint()
    {
        if (Mathf.Abs(pointA.position.x - transform.position.x) > Mathf.Abs(pointB.position.x - transform.position.x))
        {
            targetPoint = pointA;
        }
        else
        {
            targetPoint = pointB;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(!attackList.Contains(collision.transform)&&!hasBomb&& !isDead&&! GameManager.instance.gameOver)
            attackList.Add(collision.transform);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        attackList.Remove(collision.transform);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!GameManager.instance.gameOver&&!isDead)
            StartCoroutine(onAlarm());
        else
        {
            collision.GetComponent<PlayerController>().gameObject.layer = LayerMask.NameToLayer("NPC");
        }
    }

    IEnumerator onAlarm()
    {
        alarmSign.SetActive(true);
        yield return new WaitForSeconds(alarmSign.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.length);
        alarmSign.SetActive(false);
    }
}
