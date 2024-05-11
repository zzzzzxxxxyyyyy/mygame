using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitStateWu : IState
{
    private FSMBoomWu manager;
    private ParameterWu parameter;
    public float timer = 100f;

    public HitStateWu(FSMBoomWu manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;

    }
    public void OnEnter()
    {
        manager.SetGravity(1.5f);
        parameter.animator.SetBool("is_boom",parameter.is_boom);
        parameter.animator.SetBool("is_die",parameter.is_invincible);
    }

    public void OnExit()
    {
        timer = 100f;
    }

    public void OnFixUpdate()
    {
        timer -= Time.deltaTime;
        if (parameter.physicalCheck.is_ground && timer <= 0f && !parameter.is_invincible) {
                manager.SwitchState(StateType.Idle);
        }
        if (parameter.physicalCheck.is_ground && parameter.is_invincible)
        {
            manager.SwitchState(StateType.Death);
        }
    }

    public void OnUpdate()
    {

    }
}
