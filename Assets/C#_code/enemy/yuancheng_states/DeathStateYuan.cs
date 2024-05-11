using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathStateYuan : IState
{
    private FSMYuan manager;
    private ParameterYuan parameter;

    public DeathStateYuan(FSMYuan manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;

    }
    public void OnEnter()
    {
        parameter.animator.SetBool("is_die", true);
        parameter.animator.SetBool("is_ground", parameter.physicalCheck.is_ground);
    }

    public void OnExit()
    {
    }

    public void OnFixUpdate()
    {
    }

    public void OnUpdate()
    {
    }
}
