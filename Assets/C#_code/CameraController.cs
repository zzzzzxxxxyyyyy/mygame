using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

/**
 * 相机
 */
public class CameraController : MonoBehaviour
{
    private static CameraController instance;
    public CinemachineImpulseSource cinemachineImp;

    private void Start()
    {

    }




    //单例化
    public static CameraController Instance
    {
        get
        {
            if (instance == null)
                instance = Transform.FindObjectOfType<CameraController>();
            return instance;
        }
    }

    //调用暂停协程
    public void HitPause(int duration)
    {
        StartCoroutine(Pause(duration));
    }

    //暂停协程
    IEnumerator Pause(int duration)
    {
        float pauseTime = duration / 60f;
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(pauseTime);
        Time.timeScale = 1;
    }


    //相机震动
    public void CameraShake(float shanke_force)
    {
        cinemachineImp.GenerateImpulseWithForce(shanke_force);
    }
}
