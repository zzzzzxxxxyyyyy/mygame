using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class chenAttack : MonoBehaviour
{

    chenPlayer chen;   //父类

    public float hit_up;    //敌人受击后向上的力
    public float hit_back;  //敌人受击后向后的力
    public float hit_up_w;      //敌人受击后向上的力 （武器）
    public float hit_back_w;    //敌人受击后向后的力 （武器）
    public float skill_back;      //敌人受击后向后的力 （技能）
    public float skill_up;      //敌人受击后向上的力 （技能）
    public float skill2_up;      //敌人受击后向上的力 （技能2）
    public float skill3_up;      //敌人受击后向上的力 （技能3）
    public float skill3_back;      //敌人受击后向后的力 （技能3）



    [Header("攻击伤害")]
    public float skill1_dam;
    public float skill2_dam;
    public float skill3_dam;
    public float k_dam;

    [Header("攻击力度")]
    public float k_power;
    public float k_power2;
    public float skill1_power;

    [Header("技力恢复")]
    public float sp_r1;
    public float sp_r2;

    [Header("攻击停顿")]
    public int hit_pause_w;     //敌人受击后暂停时间 （武器）
    public int hit_pause_skill1;     //敌人受击后暂停时间 （技能1）
    public int hit_pause_skill2;     //敌人受击后暂停时间 （技能2）
    public int hit_pause_skill3;     //敌人受击后暂停时间 （技能3）

    private float damage;   //伤害
    private float hit_up_p; //击飞
    private int animation_x;    //动画播放
    private float shake_force;  //郑董力度
    private int hit_pause;  //停止时间


    // Start is called before the first frame update
    void Awake()
    {
        chen = GetComponentInParent<chenPlayer>(); //获取父类中的c#脚本组件
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //成功攻击敌人执行
    private void OnTriggerEnter2D(Collider2D collision)
    {
        hit_up_p = 0f;
        animation_x = 0;
        shake_force = 0f;
        hit_pause = 0;

        if (collision.CompareTag("enemy"))
        {
            //获取人物与物品位置向量
            float position_x = collision.transform.position.x - chen.transform.position.x;
            if (position_x >= 0)
            {
                position_x = 1;
            }
            else
            {
                position_x = -1;
            }
            //最终传递向量
            Vector2 v = new Vector2();
            if (chen.is_act)
            {
                if (chen.act_status == 1)
                {
                    //技力恢复
                    chen.SpRecovery(sp_r1);
                    damage = 10f;
                    if (chen.combos == 3)
                    {

                    }
                    v = new Vector2(position_x * hit_back, hit_up);

                }
                else
                {
                    //技力恢复
                    chen.SpRecovery(sp_r2);
                    damage = k_dam;
                    shake_force = k_power;
                    if (chen.combos == 3)
                    {

                        hit_up_p = hit_up_w;
                        shake_force = k_power2;
                        hit_pause = hit_pause_w;

                    }
                    v = new Vector2(position_x * hit_back_w, hit_up_p);
                    animation_x = chen.combos - 1;
                }
            }
            else if (chen.is_skill)
            {
                damage = skill1_dam;
                hit_pause = hit_pause_skill1;
                shake_force = skill1_power;
                animation_x = 3;
                v = new Vector2(position_x * skill_back, skill_up);
            }
            else if (chen.is_skill2)
            {
                damage = skill2_dam;
                hit_pause = hit_pause_skill2;
                animation_x = 4;
                v = new Vector2(0f, skill2_up);
            }
            else if (chen.is_skill3)
            {
                damage = skill3_dam;
                hit_pause = hit_pause_skill3;
                animation_x = 5;
                v = new Vector2(skill3_back, skill3_up);
            }

            /**
             * 调用敌人函数部分
             */
            //普通地面敌人
            if (collision.GetComponent<FSM>() != null && !collision.GetComponent<FSM>().parameter.is_invincible && !collision.GetComponent<FSM>().parameter.is_down)
            {
                chen.PlayChenAtkVoice();
                collision.GetComponent<FSM>().HitEffect(animation_x);
                collision.GetComponent<FSM>().SwitchState(StateType.Hit);
                //调用敌人的受击函数和扣除血量
                //将陈的位置信息和攻击力度传给敌人
                collision.GetComponent<FSM>().parameter.chen_position = chen.transform.position.x;
                collision.GetComponent<FSM>().parameter.hit_power = v;
                collision.GetComponent<FSM>().HitFlip();
                //collision.GetComponent<FSM>().PlayHitAnimation(animation_x);
                collision.GetComponentInChildren<enemyHp>().ReduceHp(damage);
                //震动屏幕增强打击感
                CameraController.Instance.CameraShake(shake_force);
                //调用攻击停顿
                CameraController.Instance.HitPause(hit_pause);
            }
            //无人机
            if (collision.GetComponent<FSMBoomWu>() != null && !collision.GetComponent<FSMBoomWu>().parameter.is_invincible)
            {
                chen.PlayChenAtkVoice();
                collision.GetComponent<FSMBoomWu>().HitEffect(animation_x);
                collision.GetComponent<FSMBoomWu>().SwitchState(StateType.Hit);
                collision.GetComponent<FSMBoomWu>().parameter.chen_position = chen.transform.position.x;
                collision.GetComponent<FSMBoomWu>().parameter.hit_power = v;
                collision.GetComponent<FSMBoomWu>().HitFlip();
                //collision.GetComponent<FSMBoomWu>().PlayHitAnimation(animation_x);
                collision.GetComponentInChildren<enemyHp>().ReduceHp(damage);
                //震动屏幕增强打击感
                CameraController.Instance.CameraShake(shake_force);
                //调用攻击停顿
                CameraController.Instance.HitPause(hit_pause);
            }
            //远程敌人
            if (collision.GetComponent<FSMYuan>() != null && !collision.GetComponent<FSMYuan>().parameter.is_invincible && !collision.GetComponent<FSMYuan>().parameter.is_down)
            {
                chen.PlayChenAtkVoice();
                collision.GetComponent<FSMYuan>().HitEffect(animation_x);
                collision.GetComponent<FSMYuan>().SwitchState(StateType.Hit);
                collision.GetComponent<FSMYuan>().parameter.chen_position = chen.transform.position.x;
                collision.GetComponent<FSMYuan>().parameter.hit_power = v;
                collision.GetComponent<FSMYuan>().HitFlip();
                collision.GetComponentInChildren<enemyHp>().ReduceHp(damage);
                //震动屏幕增强打击感
                CameraController.Instance.CameraShake(shake_force);
                //调用攻击停顿
                CameraController.Instance.HitPause(hit_pause);
            }
        }
    }
    
}
