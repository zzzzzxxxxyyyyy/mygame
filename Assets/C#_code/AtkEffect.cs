using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtkEffect : MonoBehaviour
{
    private Animator animator;
    private AnimatorStateInfo animatorStateInfo;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        DestroyThis();
    }

    public void DestroyThis()
    {
        animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (animatorStateInfo.normalizedTime >= 0.95f)
        {
            ObjectPool.Instance.PushGameObject(this.gameObject);
        }
    }
}
