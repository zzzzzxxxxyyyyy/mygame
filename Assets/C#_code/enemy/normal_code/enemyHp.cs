using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class enemyHp : MonoBehaviour
{
    //Ѫ��UI
    public Image hpImage;
    public Image effectImage;

    public float maxHp;         //���Ѫ��
    public float nowHp;         //��ǰѪ��
    public float invincibleTime;    //�޵�ʱ��
    public bool invincible = false;     //�޵�״̬
    public float effectTime = 0.5f;

    private Coroutine coroutine;
    enemy enemy;
    FSM fsm;
    FSMBoomWu fsm_w;
    FSMYuan fsm_y;

    private void Awake()
    {
        nowHp = maxHp;
        UpdateHp();
        enemy = GetComponentInParent<enemy>() ;
        fsm = GetComponent<FSM>();
        fsm_w = GetComponent<FSMBoomWu>();
        fsm_y = GetComponent<FSMYuan>();
    }


    //Ѫ���仯
    public void SetHp(float hp) {
        nowHp = Mathf.Clamp(hp,0f,maxHp);
        //Ѫ����ʾ�仯
        UpdateHp();
        if (nowHp <= 0) {
            DieInvincible();
        }
    }

    //�����޵�
    public void DieInvincible() {
        if (fsm != null) {
            fsm.parameter.is_invincible = true;
        }
        if (fsm_w != null)
        {
            fsm_w.parameter.is_invincible = true;
        }
        if (fsm_y != null)
        {
            fsm_y.parameter.is_invincible = true;
        }
    }

    //��Ѫ
    public void AddHp(float heath) {
        SetHp(nowHp + heath);
    }
    //��Ѫ
    public void ReduceHp(float damage) {
        SetHp(nowHp - damage);
    }

    //ʹ��Э�̸���Ѫ��
    private void UpdateHp() {
        hpImage.fillAmount = nowHp / maxHp;
        //���Э�̲�Ϊ�գ���ͣ��Э�̣�������Э��
        if (coroutine != null) {
            StopCoroutine(coroutine);
        }
        coroutine = StartCoroutine(UpdateEffect());
    }

    private IEnumerator UpdateEffect() {
        float effectL = effectImage.fillAmount - hpImage.fillAmount;
        float effectT = 0f;
        while (effectT < effectTime && effectL != 0) {
            effectT += Time.deltaTime;
            effectImage.fillAmount = Mathf.Lerp(hpImage.fillAmount + effectL, hpImage.fillAmount, effectT / effectTime);
            yield return null;
        }
        effectImage.fillAmount = hpImage.fillAmount;
    }
}
