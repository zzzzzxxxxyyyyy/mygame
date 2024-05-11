using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownStateYuan : IState
{
    private FSMYuan manager;
    private ParameterYuan parameter;
    private AnimatorStateInfo info;
    private float timer;

    public DownStateYuan(FSMYuan manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
        
    }
    public void OnEnter()
    {
        parameter.is_down = true;
        manager.Move(0f);
    }

    public void OnExit()
    {
        parameter.animator.StopPlayback();
        parameter.is_hit = false;
        parameter.is_down = false;
    }

    public void OnFixUpdate()
    {
        manager.Move(0f);
        if (parameter.physicalCheck.is_ground)
        {
            parameter.animator.Play("xxl_y_down");
            info = parameter.animator.GetCurrentAnimatorStateInfo(0);
            if (info.normalizedTime >= 0.95f)
            {
                manager.SwitchState(StateType.Patrol);
            }
        }
    }

    public void OnUpdate()
    {
    }
}
