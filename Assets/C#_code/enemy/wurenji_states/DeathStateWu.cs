using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathStateWu : IState
{
    private FSMBoomWu manager;
    private ParameterWu parameter;

    public DeathStateWu(FSMBoomWu manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }
    public void OnEnter()
    {
        parameter.animator.SetBool("is_boom", parameter.is_boom);
        parameter.animator.SetBool("is_die", parameter.is_invincible);
    }

    public void OnExit()
    {
    }

    public void OnFixUpdate()
    {
        if (parameter.is_boom) {
            manager.ShootBomb();
        }
        manager.DestroyThis();
    }

    public void OnUpdate()
    {
    }
}
