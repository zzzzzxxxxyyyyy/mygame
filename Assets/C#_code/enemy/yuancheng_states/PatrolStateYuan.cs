using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolStateYuan : IState
{
    private FSMYuan manager;
    private ParameterYuan parameter;
    private AnimatorStateInfo info;

    public PatrolStateYuan(FSMYuan manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }
    public void OnEnter()
    {
        parameter.animator.SetBool("walk", true);
    }

    public void OnExit()
    {
        parameter.animator.SetBool("walk", false);
        
    }

    public void OnFixUpdate()
    {
        manager.Move(parameter.patrol_speed);
        manager.EnemyFlip();
        if (manager.FoundChen()) {
            manager.SwitchState(StateType.Chase);
        }
        if (manager.FoundChen() && !manager.SavePosition() && parameter.ready_atk)
        {
            manager.SwitchState(StateType.Attack);
        }
    }

    public void OnUpdate()
    {
    }

}
