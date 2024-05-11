using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadState : IState
{
    private FSMBoomWu manager;
    private ParameterWu parameter;

    public LoadState(FSMBoomWu manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }
    public void OnEnter()
    {
        parameter.animator.SetBool("is_boom", parameter.is_boom);
        parameter.animator.SetBool("move", true);
    }

    public void OnExit()
    {
        parameter.animator.SetBool("move", false);
    }

    public void OnUpdate()
    {
    }

    public void OnFixUpdate()
    {
        manager.BlackBox(parameter.reload_speed);
    }

}
