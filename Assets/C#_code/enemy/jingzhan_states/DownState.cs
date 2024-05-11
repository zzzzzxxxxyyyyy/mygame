using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownState : IState
{
    private FSM manager;
    private Parameter parameter;
    public AnimatorStateInfo info;
    public DownState(FSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }

    public void OnEnter()
    {
        parameter.is_down = true;    
    }

    public void OnFixUpdate()
    {
        if (parameter.physicalCheck.is_ground) {
            parameter.animator.Play("down");
            info = parameter.animator.GetCurrentAnimatorStateInfo(0);
            if(info.normalizedTime >= 0.95f)
            {
                manager.SwitchState(StateType.Patrol);
            }
        }
    }

    public void OnUpdate() { 
    
    }

    public void OnExit()
    {
        parameter.animator.StopPlayback();
        parameter.is_hit = false;
        parameter.is_down = false;
    }

}
