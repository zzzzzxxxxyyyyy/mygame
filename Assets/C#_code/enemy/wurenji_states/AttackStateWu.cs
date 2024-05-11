using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackStateWu : IState
{
    private FSMBoomWu manager;
    private ParameterWu parameter;
    private AnimatorStateInfo info;


    public AttackStateWu(FSMBoomWu manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;

    }
    public void OnEnter()
    {
        parameter.animator.SetBool("move", false);
        parameter.animator.SetBool("is_boom", parameter.is_boom);
        parameter.animator.SetTrigger("atk");
        parameter.is_boom = false;
    }

    public void OnExit()
    {
    }

    public void OnFixUpdate()
    {
        info = parameter.animator.GetCurrentAnimatorStateInfo(0);
        if (info.normalizedTime >= 0.9f)
        {
            manager.ShootBomb();
            manager.SwitchState(StateType.Idle);
        }
    }

    public void OnUpdate()
    {
    }
}
