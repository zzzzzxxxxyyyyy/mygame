using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState
{
    private FSM manager;
    private Parameter parameter;
    private AnimatorStateInfo info;

    public AttackState(FSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }
    public void OnEnter()
    {
        if ( parameter.physicalCheck.is_ground   ) {
            manager.Move(0f);
            parameter.animator.SetTrigger("atk");
            parameter.rigidBody2D.velocity = new(parameter.attack_speed, 0f);
        }
    }

    public void OnFixUpdate()
    {
        info = parameter.animator.GetCurrentAnimatorStateInfo(0);
        if (info.normalizedTime >= 0.95f)
        {
            manager.SwitchState(StateType.Chase);
        }
    }
    public void OnUpdate()
    {

    }

    public void OnExit()
    {
       
    }



}
