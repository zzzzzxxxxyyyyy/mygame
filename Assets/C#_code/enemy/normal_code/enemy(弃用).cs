using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{

    private Rigidbody2D rigidBody2D;    //引用刚体
    private Animator animator;          //引用动画器
    private CapsuleCollider2D capsuleCollider2D;//引用2d胶囊碰撞器
    private PhysicalCheck physicalCheck;
    enemyHp enemyHp;

    public bool is_invincible;    //是否死亡无敌

    public bool gravity;   //敌人重量
    [Header("移动速度")]
    public float patrol_speed;      //巡逻速度
    public float act_speed;         //追击速度
    public Vector3 face_dir;        //面朝方向

    // Start is called before the first frame update
    void Awake()
    {
        //初始化组件
        rigidBody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        enemyHp = GetComponentInChildren<enemyHp>();
        physicalCheck = GetComponent<PhysicalCheck>();
    }

    // Update is called once per frame
    void Update()
    {
        ChangeHitAnimtion();//切换挨打动画
        isInvincible();     //判断是否无敌
        face_dir = new Vector3(-transform.localScale.x, 0, 0);    //实时获得面朝方向
        EnemyFlip();        //巡逻遇到悬崖反转
    }

    private void FixedUpdate()
    {
       // Move();         //移动
    }

    public void EnemyFlip()
    {
        if ((!physicalCheck.behind_ground && face_dir.x > 0) || (!physicalCheck.front_ground && face_dir.x < 0))
        {
            transform.localScale = new Vector3(face_dir.x, 1, 1);
        }
    }

    //敌人的移动
    public void Move()
    {
        rigidBody2D.velocity = new Vector2(patrol_speed * face_dir.x * Time.deltaTime, rigidBody2D.velocity.y);
        animator.SetBool("walk", true);
    }

    public void isInvincible()
    {
        if (enemyHp.invincible)
        {
            is_invincible = true;
        }
    }

    //受击
    public void GetHit()
    {
        if (!is_invincible)
        {
            if (physicalCheck.is_ground)
            {
                animator.SetTrigger("hit");
            }
        }
    }
    private void ChangeHitAnimtion()
    {
        //判断y上是否有值
        bool isYSpeed = Mathf.Abs(rigidBody2D.velocity.y) > 0.33f;
        animator.SetBool("hit_fly", isYSpeed);
        animator.SetFloat("hit_speed", rigidBody2D.velocity.y);
    }

    //受击反转
    public void HitFlip(float x,Vector2 v) {
        bool isBack = x - rigidBody2D.velocity.x > 0;
        if (!is_invincible) {
            if (!isBack)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            //若重力模式为1则可以被击飞
            if (gravity) {
                StartCoroutine(HitFly(v));
            }
        }
    }
    private IEnumerator HitFly(Vector2 v)
    {
        //parameter.rigidBody2D.velocity = v;
        rigidBody2D.AddForce(v, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.45f);
    }


    public void Die()
    {
        animator.SetBool("is_die", true);
    }

    //动画播放完毕后自动调用销毁
    private void DestoryThis()
    {
        Destroy(this.gameObject);
    }



}
