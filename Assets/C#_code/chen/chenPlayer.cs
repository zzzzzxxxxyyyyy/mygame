using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * 主角控制
 */
public class chenPlayer : MonoBehaviour
{
    [Header("基础属性")]
    public float max_hp;    //最大血量
    public float max_ps;    //最大体力
    public float max_sp;    //最大技力
    public float curr_hp;  //当前血量
    public float curr_ps;  //当前体力
    public float curr_sp;  //当前技力

    [Header("行走")]
    public float walk_speed;    //行走速度
    public float run_speed;     //奔跑速度
    private float now_speed;    //当前速度

    [Header("冲刺")]
    public float spurt_speed;   //冲刺速度
    public float spurt_ps;      //冲刺消耗体力
    public float ps_re;         //体力恢复速度
    public float spurt_time;    //冲刺持续时间
    private float sprut_time_start;
    public bool spurt_status;          //冲刺状态

    [Header("跳跃")]
    public float jump_speed;    //跳跃速度
    public float jump_time;     //滞空时间

    [Header("攻击")]
    public int act_status = 2;     //当前攻击类型 1--拳脚攻击  2--武器攻击
    public int combos = 0;     //攻击计数
    private float interval = 0.75f;    //允许连续攻击的时间
    private float timer;    //计时器
    public bool is_act;    //是否处于攻击
    public bool is_skill;  //是否处于技能1
    public bool is_skill2;  //是否处于技能2
    public bool is_skill3;  //是否处于技能2
    public float skill2_up; //技能2上挑高度
    public bool is_hit;    //是否被击
    public float skill_use; //技能消耗

    [Header("组件")]
    public Rigidbody2D rigidBody2D;    //引用刚体
    public Animator animator;          //引用动画器
    private BoxCollider2D boxCollider2D;    //引用2d盒子碰撞器
    private CapsuleCollider2D capsuleCollider2D;//引用2d胶囊碰撞器
    private PhysicalCheck physicalCheck;
    private ChenUI chenUI;
    private Animator effectAnimator;



    // 游戏开始时初始化
    void Awake()
    {
        //初始化
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

    // 每帧动画调用
    void Update()
    {
        Flip();     //角色反转
        Run();      //行走
        Spurt();    //冲刺
        Jump();     //跳跃
        Attack();   //攻击
        Skill1();   //技能1
        Skill2();   //技能2
        Skill3();   //技能3
        ChangeJumpAnimtion();   //跳跃动画切换
        ChangeActStatus();  //切换攻击类型
        JumpTimeAdd();      //累计滞空时间
        StaminaRecovery();  //体力恢复
        AddHp();
        RHp();
        QuitGame();
    }




    public void Flip()
    {
        if (!is_hit) {
            //判断x上是否有值
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
            //判断x上是否有值
            bool isXSpeed = Mathf.Abs(rigidBody2D.velocity.x) > Mathf.Epsilon;
            if (!isXSpeed) {
                now_speed = walk_speed;
            }
            //获得水平方向
            float walkDir = Input.GetAxis("Horizontal");
            //设置x轴速度
            rigidBody2D.velocity = new Vector2(walkDir * now_speed, rigidBody2D.velocity.y);
            //调用动画器标签中的walk并设置值
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
                //启动动画
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
                //关闭动画
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
            //锁定攻击状态
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
                //减少spUI
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
                //减少spUI
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

    //技能2起跳  动画调用
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
                //减少spUI
                chenUI.ReduceSp(skill_use);
                is_skill3 = true;
                animator.SetTrigger("skill_3");
                rigidBody2D.velocity = new Vector2(walkDir * 7, 15);
            }
        }
    
    }



    //体力恢复
    public void StaminaRecovery() {
        if (!spurt_status && curr_ps < max_ps) {
            curr_ps += (Time.deltaTime * ps_re);
            if (curr_ps >= max_ps) {
                curr_ps = max_ps;
            }
            chenUI.AddPs(curr_ps);
        }
    }

    //技力恢复
    public void SpRecovery(float info_sp) {
        if (curr_sp < max_sp) {
            curr_sp += info_sp;
            //当前技力大于等于最大技力时，赋予最大技力
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
        //判断y上是否有值
        bool isYSpeed = Mathf.Abs(rigidBody2D.velocity.y) > Mathf.Epsilon;
        animator.SetBool("jump", isYSpeed);
        animator.SetFloat("jump_speed", rigidBody2D.velocity.y);
        //获取陈的跳跃动画状态，做到播放一次落地声音
        AnimatorStateInfo animatorState = animator.GetCurrentAnimatorStateInfo(0);
        float info = animator.GetFloat("jump_speed");
        if (info == 0f && (animatorState.normalizedTime >= 1f) && animatorState.IsName("jump")) {
            //根据滞空时间切换落地声音
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

    //被敌人攻击
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
        //受击完成恢复普通状态
        is_hit = false;
        is_act = false;
        is_skill = false;
        is_skill2 = false;
        is_skill3 = false;
    }

    //播放走路声音
    public void PlayWalkVoice()
    {
        VoiceManager.Walk();
    }
    //播放跑步声音
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
    //攻击结束
    public void AttacktPass()
    {
        is_act = false;
        is_skill = false;
        is_skill2 = false;
        is_skill3 = false;
    }

    //测试回血
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
