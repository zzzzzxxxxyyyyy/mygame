using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitStateYuan : IState
{
    private FSMYuan manager;
    private ParameterYuan parameter;
    private AnimatorStateInfo info;
    float timer = 0.5f;


    public HitStateYuan(FSMYuan manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }


    public void OnEnter()
    {
        parameter.is_hit = true;
    }

    public void OnExit()
    {
        timer = 0.5f;
        parameter.is_hit = false;
        if (parameter.gravity)
        {
            parameter.animator.SetBool("hit_fly", false);
            parameter.hit_y = 0f;
        }
    }

    public void OnFixUpdate()
    {

    }

    public void OnUpdate()
    {
        info = parameter.animator.GetCurrentAnimatorStateInfo(0);
        if (parameter.physicalCheck.is_ground)
        {
            parameter.animator.SetBool("hit", true);
        }
        else if (parameter.gravity)
        {
            parameter.animator.SetBool("hit", false);
            parameter.animator.SetBool("hit_fly", true);
            parameter.animator.SetFloat("hit_speed", parameter.hit_y);
        }

        timer -= Time.deltaTime;
        if (info.normalizedTime >= 0.95f && info.IsName("xxl_y_hit_fly") && parameter.physicalCheck.is_ground && parameter.gravity)
        {
            parameter.animator.SetBool("hit", false);
            manager.SwitchState(StateType.Down);
        }
        if (info.normalizedTime >= 0.95f && timer <= 0 && parameter.physicalCheck.is_ground)
        {
            parameter.animator.SetBool("hit", false);
            manager.SwitchState(StateType.Patrol);
        }
        if (parameter.is_invincible)
        {
            parameter.animator.SetBool("hit", false);
            manager.SwitchState(StateType.Death);
        }
    }
}
