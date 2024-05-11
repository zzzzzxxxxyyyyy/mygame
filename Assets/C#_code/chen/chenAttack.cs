using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class chenAttack : MonoBehaviour
{

    chenPlayer chen;   //����

    public float hit_up;    //�����ܻ������ϵ���
    public float hit_back;  //�����ܻ���������
    public float hit_up_w;      //�����ܻ������ϵ��� ��������
    public float hit_back_w;    //�����ܻ��������� ��������
    public float skill_back;      //�����ܻ��������� �����ܣ�
    public float skill_up;      //�����ܻ������ϵ��� �����ܣ�
    public float skill2_up;      //�����ܻ������ϵ��� ������2��
    public float skill3_up;      //�����ܻ������ϵ��� ������3��
    public float skill3_back;      //�����ܻ��������� ������3��



    [Header("�����˺�")]
    public float skill1_dam;
    public float skill2_dam;
    public float skill3_dam;
    public float k_dam;

    [Header("��������")]
    public float k_power;
    public float k_power2;
    public float skill1_power;

    [Header("�����ָ�")]
    public float sp_r1;
    public float sp_r2;

    [Header("����ͣ��")]
    public int hit_pause_w;     //�����ܻ�����ͣʱ�� ��������
    public int hit_pause_skill1;     //�����ܻ�����ͣʱ�� ������1��
    public int hit_pause_skill2;     //�����ܻ�����ͣʱ�� ������2��
    public int hit_pause_skill3;     //�����ܻ�����ͣʱ�� ������3��

    private float damage;   //�˺�
    private float hit_up_p; //����
    private int animation_x;    //��������
    private float shake_force;  //֣������
    private int hit_pause;  //ֹͣʱ��


    // Start is called before the first frame update
    void Awake()
    {
        chen = GetComponentInParent<chenPlayer>(); //��ȡ�����е�c#�ű����
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //�ɹ���������ִ��
    private void OnTriggerEnter2D(Collider2D collision)
    {
        hit_up_p = 0f;
        animation_x = 0;
        shake_force = 0f;
        hit_pause = 0;

        if (collision.CompareTag("enemy"))
        {
            //��ȡ��������Ʒλ������
            float position_x = collision.transform.position.x - chen.transform.position.x;
            if (position_x >= 0)
            {
                position_x = 1;
            }
            else
            {
                position_x = -1;
            }
            //���մ�������
            Vector2 v = new Vector2();
            if (chen.is_act)
            {
                if (chen.act_status == 1)
                {
                    //�����ָ�
                    chen.SpRecovery(sp_r1);
                    damage = 10f;
                    if (chen.combos == 3)
                    {

                    }
                    v = new Vector2(position_x * hit_back, hit_up);

                }
                else
                {
                    //�����ָ�
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
             * ���õ��˺�������
             */
            //��ͨ�������
            if (collision.GetComponent<FSM>() != null && !collision.GetComponent<FSM>().parameter.is_invincible && !collision.GetComponent<FSM>().parameter.is_down)
            {
                chen.PlayChenAtkVoice();
                collision.GetComponent<FSM>().HitEffect(animation_x);
                collision.GetComponent<FSM>().SwitchState(StateType.Hit);
                //���õ��˵��ܻ������Ϳ۳�Ѫ��
                //���µ�λ����Ϣ�͹������ȴ�������
                collision.GetComponent<FSM>().parameter.chen_position = chen.transform.position.x;
                collision.GetComponent<FSM>().parameter.hit_power = v;
                collision.GetComponent<FSM>().HitFlip();
                //collision.GetComponent<FSM>().PlayHitAnimation(animation_x);
                collision.GetComponentInChildren<enemyHp>().ReduceHp(damage);
                //����Ļ��ǿ�����
                CameraController.Instance.CameraShake(shake_force);
                //���ù���ͣ��
                CameraController.Instance.HitPause(hit_pause);
            }
            //���˻�
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
                //����Ļ��ǿ�����
                CameraController.Instance.CameraShake(shake_force);
                //���ù���ͣ��
                CameraController.Instance.HitPause(hit_pause);
            }
            //Զ�̵���
            if (collision.GetComponent<FSMYuan>() != null && !collision.GetComponent<FSMYuan>().parameter.is_invincible && !collision.GetComponent<FSMYuan>().parameter.is_down)
            {
                chen.PlayChenAtkVoice();
                collision.GetComponent<FSMYuan>().HitEffect(animation_x);
                collision.GetComponent<FSMYuan>().SwitchState(StateType.Hit);
                collision.GetComponent<FSMYuan>().parameter.chen_position = chen.transform.position.x;
                collision.GetComponent<FSMYuan>().parameter.hit_power = v;
                collision.GetComponent<FSMYuan>().HitFlip();
                collision.GetComponentInChildren<enemyHp>().ReduceHp(damage);
                //����Ļ��ǿ�����
                CameraController.Instance.CameraShake(shake_force);
                //���ù���ͣ��
                CameraController.Instance.HitPause(hit_pause);
            }
        }
    }
    
}
