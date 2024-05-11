using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{

    private Rigidbody2D rigidBody2D;    //���ø���
    private Animator animator;          //���ö�����
    private CapsuleCollider2D capsuleCollider2D;//����2d������ײ��
    private PhysicalCheck physicalCheck;
    enemyHp enemyHp;

    public bool is_invincible;    //�Ƿ������޵�

    public bool gravity;   //��������
    [Header("�ƶ��ٶ�")]
    public float patrol_speed;      //Ѳ���ٶ�
    public float act_speed;         //׷���ٶ�
    public Vector3 face_dir;        //�泯����

    // Start is called before the first frame update
    void Awake()
    {
        //��ʼ�����
        rigidBody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        enemyHp = GetComponentInChildren<enemyHp>();
        physicalCheck = GetComponent<PhysicalCheck>();
    }

    // Update is called once per frame
    void Update()
    {
        ChangeHitAnimtion();//�л����򶯻�
        isInvincible();     //�ж��Ƿ��޵�
        face_dir = new Vector3(-transform.localScale.x, 0, 0);    //ʵʱ����泯����
        EnemyFlip();        //Ѳ���������·�ת
    }

    private void FixedUpdate()
    {
       // Move();         //�ƶ�
    }

    public void EnemyFlip()
    {
        if ((!physicalCheck.behind_ground && face_dir.x > 0) || (!physicalCheck.front_ground && face_dir.x < 0))
        {
            transform.localScale = new Vector3(face_dir.x, 1, 1);
        }
    }

    //���˵��ƶ�
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

    //�ܻ�
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
        //�ж�y���Ƿ���ֵ
        bool isYSpeed = Mathf.Abs(rigidBody2D.velocity.y) > 0.33f;
        animator.SetBool("hit_fly", isYSpeed);
        animator.SetFloat("hit_speed", rigidBody2D.velocity.y);
    }

    //�ܻ���ת
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
            //������ģʽΪ1����Ա�����
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

    //����������Ϻ��Զ���������
    private void DestoryThis()
    {
        Destroy(this.gameObject);
    }



}
