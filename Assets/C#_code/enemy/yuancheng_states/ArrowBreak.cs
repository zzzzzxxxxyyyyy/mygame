using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowBreak : MonoBehaviour
{
    private Animator animator;
    private AnimatorStateInfo animatorStateInfo;
    public Vector2 vector;
    // Start is called before the first frame update
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        DestroyThis();
    }

    private void OnEnable()
    {
        transform.right = vector;
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
