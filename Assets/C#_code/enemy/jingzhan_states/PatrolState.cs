using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IState
{
    private FSM manager;
    private Parameter parameter;
    public PatrolState(FSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }
    public void OnEnter()
    {
        
    }
    public void OnFixUpdate()
    {
        if (manager.FoundChen()) {
            manager.SwitchState(StateType.Chase);
        }
        parameter.animator.SetBool("walk", true);
        manager.Move(parameter.patrol_speed);
        manager.EnemyFlip();
        if (parameter.is_hit) {
            manager.SwitchState(StateType.Hit);
        }
        if (parameter.is_invincible) {
            manager.SwitchState(StateType.Death);
        }
    }

    public void OnUpdate()
    {

    }
    public void OnExit()
    {
        parameter.animator.SetBool("walk", false);
    }


    
}
