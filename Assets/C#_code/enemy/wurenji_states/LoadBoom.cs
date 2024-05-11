using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadBoom : MonoBehaviour
{
    private FSMBoomWu fsm;

    void Awake()
    {
        fsm = GetComponentInParent<FSMBoomWu>(); //��ȡ�����е�c#�ű����
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("BoomBox"))
        {
            StartCoroutine(LoadFinish());
        }
    }

    private IEnumerator LoadFinish() {
        yield return new WaitForSeconds(3f);
        fsm.parameter.is_boom = true;
        fsm.SwitchState(StateType.Idle);
    }
}
