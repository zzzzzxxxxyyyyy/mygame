using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class enemyHp : MonoBehaviour
{
    //血量UI
    public Image hpImage;
    public Image effectImage;

    public float maxHp;         //最大血量
    public float nowHp;         //当前血量
    public float invincibleTime;    //无敌时间
    public bool invincible = false;     //无敌状态
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


    //血量变化
    public void SetHp(float hp) {
        nowHp = Mathf.Clamp(hp,0f,maxHp);
        //血条显示变化
        UpdateHp();
        if (nowHp <= 0) {
            DieInvincible();
        }
    }

    //死亡无敌
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

    //加血
    public void AddHp(float heath) {
        SetHp(nowHp + heath);
    }
    //扣血
    public void ReduceHp(float damage) {
        SetHp(nowHp - damage);
    }

    //使用协程更新血量
    private void UpdateHp() {
        hpImage.fillAmount = nowHp / maxHp;
        //如果协程不为空，暂停改协程，开启新协程
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
