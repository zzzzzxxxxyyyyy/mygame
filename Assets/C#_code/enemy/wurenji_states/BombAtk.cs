using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombAtk : MonoBehaviour
{
    public Vector2 hit_power;
    public float hit_power2;
    public float damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("enemy"))
        {
            if (collision.GetComponent<FSM>() != null && !collision.GetComponent<FSM>().parameter.is_invincible)
            {
                collision.GetComponent<FSM>().SwitchState(StateType.Hit);
                collision.GetComponent<FSM>().parameter.chen_position = transform.position.x;
                collision.GetComponent<FSM>().parameter.hit_power = hit_power;
                collision.GetComponent<FSM>().HitFlip();
                collision.GetComponentInChildren<enemyHp>().ReduceHp(damage);
            }
            if (collision.GetComponent<FSMYuan>() != null && !collision.GetComponent<FSMYuan>().parameter.is_invincible)
            {
                collision.GetComponent<FSMYuan>().SwitchState(StateType.Hit);
                collision.GetComponent<FSMYuan>().parameter.chen_position = transform.position.x;
                collision.GetComponent<FSMYuan>().parameter.hit_power = hit_power;
                collision.GetComponent<FSMYuan>().HitFlip();
                collision.GetComponentInChildren<enemyHp>().ReduceHp(damage);
            }
        }
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<ChenUI>().ReduceHp(damage);
            collision.GetComponent<chenPlayer>().HitByEnemy(transform, hit_power2);
        }
    }
}
