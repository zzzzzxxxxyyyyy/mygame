using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseStateYuan : IState
{
    private FSMYuan manager;
    private ParameterYuan parameter;
    private AnimatorStateInfo info;
    private float chase_time;

    public ChaseStateYuan(FSMYuan manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }

    public void OnEnter()
    {
        chase_time = parameter.chase_time;
        parameter.animator.SetBool("walk", true);
        manager.FaceChen();

    }

    public void OnExit()
    {
        parameter.animator.SetBool("walk", false);
    }

    public void OnFixUpdate()
    {
        chase_time -= Time.deltaTime;
        //处于攻击位置且在安全位置 攻击
        if (manager.AttackChen() && !manager.SavePosition() && parameter.ready_atk)
        {
            manager.SwitchState(StateType.Attack);
        }
        //不在安全位置  逃跑
        if (manager.AttackChen() && manager.SavePosition() && parameter.physicalCheck.behind_ground && parameter.physicalCheck.front_ground)
        {
            manager.SwitchState(StateType.Escape);
        }
        if (!manager.FoundChen() && chase_time <= 0)
        {
            manager.SwitchState(StateType.Patrol);
        }
        manager.Move(parameter.chase_speed);
    }

    public void OnUpdate()
    {

    }
}
