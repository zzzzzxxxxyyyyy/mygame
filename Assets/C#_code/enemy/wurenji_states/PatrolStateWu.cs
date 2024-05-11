using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolStateWu : IState
{
    private FSMBoomWu manager;
    private ParameterWu parameter;

    private float timer;    //≤‚ ‘ ±º‰

    public PatrolStateWu(FSMBoomWu manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }

    public void OnEnter()
    {
        timer = parameter.pat_time;
        parameter.animator.SetBool("is_boom", parameter.is_boom);
        parameter.animator.SetBool("move", true);
    }

    public void OnExit()
    {

    }

    public void OnFixUpdate()
    {
        timer -= Time.deltaTime;
        manager.Move(parameter.patrol_speed);
        //π•ª˜≤‚ ‘
        if (parameter.is_boom && manager.AttackChen())
        {
            manager.SwitchState(StateType.Idle);
        }
        if (timer <= 0f)
        {
            manager.SwitchState(StateType.Idle);
        }
    }

    public void OnUpdate()
    {
    }
}
