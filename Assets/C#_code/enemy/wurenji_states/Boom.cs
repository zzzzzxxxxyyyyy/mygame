using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boom : MonoBehaviour
{
    private Animator animator;
    private AnimatorStateInfo animatorStateInfo;
    public float shake_froce;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        CameraController.Instance.CameraShake(shake_froce);
        VoiceManagerEnemy.BoomVoice();
       
    }

    // Update is called once per frame
    void Update()
    {
        DestroyThis();
    }

    public void DestroyThis() {
        animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (animatorStateInfo.normalizedTime >= 0.95f) {
            ObjectPool.Instance.PushGameObject(this.gameObject);
        }
    }
}
