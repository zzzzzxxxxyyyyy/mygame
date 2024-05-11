using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : IState
{
    private FSM manager;
    private Parameter parameter;
    float timer = 3f;

    public ChaseState(FSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }
    public void OnEnter()
    {
        manager.Move(parameter.chase_speed);

    }

    public void OnFixUpdate()
    {
        timer -= Time.deltaTime;
        if ( manager.AttackChen())
        {
            manager.SwitchState(StateType.Attack);
        }
        if (!manager.FoundChen() && timer <= 0)
        {
            manager.SwitchState(StateType.Patrol);
        }
        manager.Move(parameter.chase_speed);
        manager.EnemyFlip();
        manager.FaceChen();
    }

    public void OnUpdate()
    {

    }
    public void OnExit()
    {
        timer = 3f;
    }

    

}
