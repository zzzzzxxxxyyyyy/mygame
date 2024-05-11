using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChenUI : MonoBehaviour
{
    //UI
    public Image skill_image;
    public Image lgd_image;
    public Image wq_image;
    public Image ps_image;
    public Image curr_hp_image;
    public Image eff_hp_image;

    public float effectTime = 0.5f;
    private Coroutine coroutine;

    //���
    chenPlayer chen;   



    // Start is called before the first frame update
    void Start()
    {
        chen = GetComponent<chenPlayer>();
    }

    //��sp
    public void AddSp(float sp)
    {
        SetSp(sp);
    }
    //��sp
    public void ReduceSp(float sp)
    {
        SetSp(chen.curr_sp - sp);
    }
    //sp�仯
    public void SetSp(float sp)
    {
        chen.curr_sp = Mathf.Clamp(sp, 0f, chen.max_sp);
        UpdateSp();
    }
    //����sp
    private void UpdateSp() {
        skill_image.fillAmount = chen.curr_sp / chen.max_sp;
    }

    //�л�����ui
    public void ChangeWq() {
        if (chen.act_status == 1)
        {
            lgd_image.gameObject.SetActive(true);
            wq_image.gameObject.SetActive(false);
        }
        else {
            lgd_image.gameObject.SetActive(false);
            wq_image.gameObject.SetActive(true);
        }
    }


    //������
    public void AddPs(float ps)
    {
        SetPs(ps);
    }
    //������
    public void ReducePs(float ps)
    {
        SetPs(chen.curr_ps);
    }
    //�����仯
    public void SetPs(float ps)
    {
        chen.curr_ps = Mathf.Clamp(ps, 0f, chen.max_ps);
        UpdatePs();
    }
    //����sp
    private void UpdatePs()
    {
        ps_image.fillAmount = chen.curr_ps / chen.max_ps;
    }


    //��hp
    public void AddHp(float hp)
    {
        AddSetHp(chen.curr_hp + hp);
    }
    //��hp
    public void ReduceHp(float hp)
    {
        SetHp(chen.curr_hp - hp);
    }
    //hp�仯
    public void SetHp(float hp)
    {
        chen.curr_hp = Mathf.Clamp(hp, 0f, chen.max_hp);
        //Ѫ����ʾ�仯
        UpdateHp();
        if (chen.curr_hp < 1f)
        {
            Debug.Log("�±�����");
            return;
        }
    }
    public void AddSetHp(float hp)
    {
        chen.curr_hp = Mathf.Clamp(hp, 0f, chen.max_hp);
        //Ѫ����ʾ�仯
        AddUpdateHp();
        if (chen.curr_hp >= 100f)
        {
            Debug.Log("����Ѫ��");
            return;
        }
    }
    //ʹ��Э�̸���Ѫ��
    private void UpdateHp()
    {
        curr_hp_image.fillAmount = chen.curr_hp / chen.max_hp;
        //���Э�̲�Ϊ�գ���ͣ��Э�̣�������Э��
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = StartCoroutine(UpdateEffect());
    }
    private IEnumerator UpdateEffect()
    {
        float effectL = eff_hp_image.fillAmount - curr_hp_image.fillAmount;
        float effectT = 0f;
        while (effectT < effectTime && effectL != 0)
        {
            effectT += Time.deltaTime;
            eff_hp_image.fillAmount = Mathf.Lerp(curr_hp_image.fillAmount + effectL, curr_hp_image.fillAmount, effectT / effectTime);
            yield return null;
        }
        eff_hp_image.fillAmount = curr_hp_image.fillAmount;
    }

    private void AddUpdateHp()
    {
        eff_hp_image.fillAmount = chen.curr_hp / chen.max_hp;
        //���Э�̲�Ϊ�գ���ͣ��Э�̣�������Э��
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = StartCoroutine(AddUpdateEffect());
    }
    private IEnumerator AddUpdateEffect()
    {
        float effectL = eff_hp_image.fillAmount - curr_hp_image.fillAmount;
        float effectT = 0f;
        while (effectT < effectTime && effectL != 0)
        {
            effectT += Time.deltaTime;
            curr_hp_image.fillAmount = Mathf.Lerp(eff_hp_image.fillAmount - effectL, eff_hp_image.fillAmount , effectT / effectTime);
            yield return null;
        }
        curr_hp_image.fillAmount = eff_hp_image.fillAmount;
    }

}
