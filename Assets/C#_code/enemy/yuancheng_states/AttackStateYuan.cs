using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackStateYuan : IState
{
    private FSMYuan manager;
    private ParameterYuan parameter;
    private AnimatorStateInfo info;

    public AttackStateYuan(FSMYuan manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }

    public void OnEnter()
    {
        manager.Move(0f);
        parameter.animator.SetTrigger("atk");
        manager.FaceChen();
        

    }

    public void OnExit()
    {
        parameter.temp_time = parameter.reload_time;
        parameter.ready_atk = false;
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

    private IEnumerator ATKStart()
    {
        manager.Move(0f);
        yield return new WaitForSeconds(1f);
        parameter.animator.SetTrigger("atk");
    }
}
