using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * ���ǿ���
 */
public class chenPlayer : MonoBehaviour
{
    [Header("��������")]
    public float max_hp;    //���Ѫ��
    public float max_ps;    //�������
    public float max_sp;    //�����
    public float curr_hp;  //��ǰѪ��
    public float curr_ps;  //��ǰ����
    public float curr_sp;  //��ǰ����

    [Header("����")]
    public float walk_speed;    //�����ٶ�
    public float run_speed;     //�����ٶ�
    private float now_speed;    //��ǰ�ٶ�

    [Header("���")]
    public float spurt_speed;   //����ٶ�
    public float spurt_ps;      //�����������
    public float ps_re;         //�����ָ��ٶ�
    public float spurt_time;    //��̳���ʱ��
    private float sprut_time_start;
    public bool spurt_status;          //���״̬

    [Header("��Ծ")]
    public float jump_speed;    //��Ծ�ٶ�
    public float jump_time;     //�Ϳ�ʱ��

    [Header("����")]
    public int act_status = 2;     //��ǰ�������� 1--ȭ�Ź���  2--��������
    public int combos = 0;     //��������
    private float interval = 0.75f;    //��������������ʱ��
    private float timer;    //��ʱ��
    public bool is_act;    //�Ƿ��ڹ���
    public bool is_skill;  //�Ƿ��ڼ���1
    public bool is_skill2;  //�Ƿ��ڼ���2
    public bool is_skill3;  //�Ƿ��ڼ���2
    public float skill2_up; //����2�����߶�
    public bool is_hit;    //�Ƿ񱻻�
    public float skill_use; //��������

    [Header("���")]
    public Rigidbody2D rigidBody2D;    //���ø���
    public Animator animator;          //���ö�����
    private BoxCollider2D boxCollider2D;    //����2d������ײ��
    private CapsuleCollider2D capsuleCollider2D;//����2d������ײ��
    private PhysicalCheck physicalCheck;
    private ChenUI chenUI;
    private Animator effectAnimator;



    // ��Ϸ��ʼʱ��ʼ��
    void Awake()
    {
        //��ʼ��
        curr_hp = max_hp;
        curr_ps = max_ps;
        curr_sp = max_sp;


        rigidBody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        physicalCheck = GetComponent<PhysicalCheck>();
        chenUI = GetComponent<ChenUI>();
        effectAnimator = transform.GetChild(2).GetChild(0).GetComponent<Animator>();
    }

    // ÿ֡��������
    void Update()
    {
        Flip();     //��ɫ��ת
        Run();      //����
        Spurt();    //���
        Jump();     //��Ծ
        Attack();   //����
        Skill1();   //����1
        Skill2();   //����2
        Skill3();   //����3
        ChangeJumpAnimtion();   //��Ծ�����л�
        ChangeActStatus();  //�л���������
        JumpTimeAdd();      //�ۼ��Ϳ�ʱ��
        StaminaRecovery();  //�����ָ�
        AddHp();
        RHp();
        QuitGame();
    }




    public void Flip()
    {
        if (!is_hit) {
            //�ж�x���Ƿ���ֵ
            bool isXSpeed = Mathf.Abs(rigidBody2D.velocity.x) > Mathf.Epsilon;
            if (isXSpeed)
            {
                if (rigidBody2D.velocity.x > 0.1)
                {
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                }
                else if (rigidBody2D.velocity.x < -0.1)
                {
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                }
            }
        }
    }

    public void Run() {
        if (!is_act && !is_skill && !is_skill2 && !is_skill3 && !is_hit) {
            //�ж�x���Ƿ���ֵ
            bool isXSpeed = Mathf.Abs(rigidBody2D.velocity.x) > Mathf.Epsilon;
            if (!isXSpeed) {
                now_speed = walk_speed;
            }
            //���ˮƽ����
            float walkDir = Input.GetAxis("Horizontal");
            //����x���ٶ�
            rigidBody2D.velocity = new Vector2(walkDir * now_speed, rigidBody2D.velocity.y);
            //���ö�������ǩ�е�walk������ֵ
            animator.SetFloat("speed", rigidBody2D.velocity.x);
            animator.SetBool("run", isXSpeed);
        }
    }

    public void Spurt() {
        if (!spurt_status && physicalCheck.is_ground && curr_ps - spurt_ps >= 0)
        {
            if (Input.GetKeyDown(KeyCode.L) && Mathf.Abs(rigidBody2D.velocity.x) > 0)
            {
                spurt_status = true;
                //��������
                animator.SetBool("spurt", true);
                sprut_time_start = spurt_time;
                now_speed = run_speed;
                curr_ps -= spurt_ps;
                chenUI.ReducePs(curr_ps);
            }
        }
        else {
            sprut_time_start -= Time.deltaTime;
            if (sprut_time_start <= 0)
            {
                spurt_status = false;
                //�رն���
                animator.SetBool("spurt", false);
            }
            else {
                rigidBody2D.velocity = transform.right * spurt_speed;
            }
        }
    }


    public void Jump()
    {
        if (Input.GetKeyDown(KeyCode.K) && physicalCheck.is_ground && !spurt_status && !is_hit && !is_act && !is_skill)
        {
            animator.SetFloat("jump_speed", rigidBody2D.velocity.y);
            float walkDir = Input.GetAxis("Horizontal");
            rigidBody2D.velocity = new Vector2(walkDir * now_speed, jump_speed);

            VoiceManager.Jump();
        }
    }



    public void Attack() {
        if (Input.GetKeyDown(KeyCode.J) && !is_act && physicalCheck.is_ground && !is_skill && !spurt_status) {
            //��������״̬
            is_act = true;
            now_speed = walk_speed;
            combos++;
            if (combos > 3) {
                combos = 1;
            }
            timer = interval;
            if (act_status == 1)
            {
                animator.SetTrigger("p_act");
                animator.SetInteger("p_act_num", combos);
            }
            else {
                animator.SetTrigger("w_act");
                animator.SetInteger("w_act_num", combos);
                VoiceManager.Knife();
            }
        }

        if (timer != 0) {
            timer -= Time.deltaTime;
            if (timer <= 0) {
                timer = 0;
                combos = 0;
            }
        }
    }



    public void Skill1()
    {
        if (Input.GetKey(KeyCode.S) && Input.GetKeyDown(KeyCode.I) && !is_act && physicalCheck.is_ground && !is_skill && !is_skill2 && now_speed < 10f)
        {
            if (curr_sp - skill_use >= 0)
            {
                //����spUI
                chenUI.ReduceSp(skill_use);
                bool x_speed = Mathf.Abs(rigidBody2D.velocity.x) > 5;
                if (!x_speed)
                {
                    is_skill = true;
                    animator.SetTrigger("skill_1");
                }
                Skill1Effect();
            }
        }
    }

    public void Skill1Effect() {
        effectAnimator.SetTrigger("isSkill");
    }

    public void Skill2()
    {
        if (Input.GetKey(KeyCode.W) && Input.GetKeyDown(KeyCode.I) && !is_act && physicalCheck.is_ground && !is_skill && !is_skill2 && now_speed < 10f)
        {
            if (curr_sp - skill_use >= 0)
            {
                //����spUI
                chenUI.ReduceSp(skill_use);
                bool x_speed = Mathf.Abs(rigidBody2D.velocity.x) > 5;
                if (!x_speed)
                {
                    is_skill2 = true;
                    animator.SetTrigger("skill_2");
                }
            }
        }
    }

    //����2����  ��������
    public void Skill2Up() {
        bool isXSpeed = Mathf.Abs(rigidBody2D.velocity.x) > Mathf.Epsilon;
        float walkDir = Input.GetAxis("Horizontal");
        rigidBody2D.velocity = new Vector2(walkDir * now_speed, skill2_up);

    }

    public void Skill3() {
        if (!physicalCheck.is_ground && Input.GetKeyDown(KeyCode.I) && !is_skill3 && !is_skill2) {
            if (curr_sp - skill_use >= 0)
            {
                float walkDir = Input.GetAxis("Horizontal");
                //����spUI
                chenUI.ReduceSp(skill_use);
                is_skill3 = true;
                animator.SetTrigger("skill_3");
                rigidBody2D.velocity = new Vector2(walkDir * 7, 15);
            }
        }
    
    }



    //�����ָ�
    public void StaminaRecovery() {
        if (!spurt_status && curr_ps < max_ps) {
            curr_ps += (Time.deltaTime * ps_re);
            if (curr_ps >= max_ps) {
                curr_ps = max_ps;
            }
            chenUI.AddPs(curr_ps);
        }
    }

    //�����ָ�
    public void SpRecovery(float info_sp) {
        if (curr_sp < max_sp) {
            curr_sp += info_sp;
            //��ǰ�������ڵ��������ʱ�����������
            if (curr_sp >= max_sp) {
                curr_sp = max_sp;
            }
            chenUI.AddSp(curr_sp);
        }
    }



    public void ChangeActStatus() {
        if (Input.GetKeyDown(KeyCode.Q) && !is_act) {
            if (act_status == 1)
            {
                act_status = 2;
            }
            else {
                act_status = 1;
            }
            chenUI.ChangeWq();
        }
    }


    private void ChangeJumpAnimtion()
    {
        //�ж�y���Ƿ���ֵ
        bool isYSpeed = Mathf.Abs(rigidBody2D.velocity.y) > Mathf.Epsilon;
        animator.SetBool("jump", isYSpeed);
        animator.SetFloat("jump_speed", rigidBody2D.velocity.y);
        //��ȡ�µ���Ծ����״̬����������һ���������
        AnimatorStateInfo animatorState = animator.GetCurrentAnimatorStateInfo(0);
        float info = animator.GetFloat("jump_speed");
        if (info == 0f && (animatorState.normalizedTime >= 1f) && animatorState.IsName("jump")) {
            //�����Ϳ�ʱ���л��������
            if (jump_time > 1.5f) {
                VoiceManager.Fall();
            } else if (jump_time <= 1.5f) { 
                VoiceManager.Fall2();
            }
            jump_time = 0f;
        }
    }

    private void JumpTimeAdd() {
        if (!physicalCheck.is_ground) {
            jump_time += Time.deltaTime;
        }
    }

    //�����˹���
    public void HitByEnemy(Transform t, float p) {
        animator.SetTrigger("hit");
        is_hit = true;
        Vector2 vec = new Vector2(transform.position.x - t.position.x, 0f).normalized;
        StartCoroutine(IsHit(vec, p));
    }
    private IEnumerator IsHit(Vector2 vector, float power)
    {
        rigidBody2D.AddForce(vector * power, ForceMode2D.Force);
        yield return new WaitForSeconds(0.5f);
        //�ܻ���ɻָ���ͨ״̬
        is_hit = false;
        is_act = false;
        is_skill = false;
        is_skill2 = false;
        is_skill3 = false;
    }

    //������·����
    public void PlayWalkVoice()
    {
        VoiceManager.Walk();
    }
    //�����ܲ�����
    public void PlayRunVoice()
    {
        VoiceManager.Run();
    }
    public void PlaySkillVoice() {
        VoiceManager.Skill1();
    }
    public void PlauChenVoice() {
        VoiceManager.SkillChen();
    }
    public void PlayChenAtkVoice() {
        if (act_status == 1)
        {
            if (combos != 3)
            {
                VoiceManager.Punch();
            }
            else
            {
                VoiceManager.Kick();
            }
        }
        else
        {
            if (combos != 3)
            {
                VoiceManager.KnifeHit();
            }
            else
            {
                VoiceManager.Scabbard();
            }
        }
    }
    //��������
    public void AttacktPass()
    {
        is_act = false;
        is_skill = false;
        is_skill2 = false;
        is_skill3 = false;
    }

    //���Ի�Ѫ
    public void AddHp() {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            chenUI.AddHp(10f);
        }
    }
    public void RHp()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            chenUI.ReduceHp(100f);
        }
    }




    public void QuitGame() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }
    }


}
