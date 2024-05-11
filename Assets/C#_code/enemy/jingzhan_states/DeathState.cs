using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathState : IState
{
    private FSM manager;
    private Parameter parameter;


    public DeathState(FSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }
    public void OnEnter()
    {
        parameter.animator.SetBool("is_die", true);
    }

    public void OnFixUpdate()
    {

    }

    public void OnUpdate()
    {

    }
    public void OnExit()
    {
       
    }

    
}