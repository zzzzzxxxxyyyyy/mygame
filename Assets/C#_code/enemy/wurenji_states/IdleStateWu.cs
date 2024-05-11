using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleStateWu : IState
{

    private FSMBoomWu manager;
    private ParameterWu parameter;
    private float timer;    //����ʱ��

    public IdleStateWu(FSMBoomWu manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }
    public void OnEnter()
    {
        timer = parameter.idle_time;
        parameter.animator.SetBool("is_boom", parameter.is_boom);
        parameter.animator.SetBool("move", false);
    }

    public void OnExit()
    {
        
    }

    public void OnFixUpdate()
    {
        //����
        if (parameter.is_invincible) {
            manager.SwitchState(StateType.Death);
        }
        //����
        if (manager.is_up() && parameter.is_boom)
        {
            manager.UpMove(parameter.up_speed);
        }
        //Ѳ��
        if (parameter.is_boom)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                manager.EnemyFlip();
                manager.SwitchState(StateType.Patrol);
            }
        }
        //װ��
        if (!parameter.is_boom)
        {
            manager.SwitchState(StateType.Load);
        }
        //����
        if (parameter.is_boom && manager.AttackChen() && !manager.is_up() && timer <= 0f) {
            manager.SwitchState(StateType.Attack);
        }
        
    }

    public void OnUpdate()
    {
    }
}
