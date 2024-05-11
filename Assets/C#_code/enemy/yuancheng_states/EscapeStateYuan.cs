using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeStateYuan : IState
{
    private FSMYuan manager;
    private ParameterYuan parameter;
    private float run_time;

    public EscapeStateYuan(FSMYuan manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }

    public void OnEnter()
    {
        manager.EnemyFlip2();
        this.run_time = parameter.run_time;
        parameter.animator.SetBool("walk", true);
    }

    public void OnExit()
    {
        parameter.animator.SetBool("walk", false);
    }

    public void OnFixUpdate()
    {
        if ((!parameter.physicalCheck.behind_ground || !parameter.physicalCheck.front_ground) && parameter.ready_atk)
        {
            manager.SwitchState(StateType.Attack);
        }
        this.run_time -= Time.deltaTime;
        manager.Move(parameter.run_speed);
        if (this.run_time <= 0f)
        {
            manager.SwitchState(StateType.Chase);
        }

    }

    public void OnUpdate()
    {
    }
}
