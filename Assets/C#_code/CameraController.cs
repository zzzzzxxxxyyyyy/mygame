using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

/**
 * ���
 */
public class CameraController : MonoBehaviour
{
    private static CameraController instance;
    public CinemachineImpulseSource cinemachineImp;

    private void Start()
    {

    }




    //������
    public static CameraController Instance
    {
        get
        {
            if (instance == null)
                instance = Transform.FindObjectOfType<CameraController>();
            return instance;
        }
    }

    //������ͣЭ��
    public void HitPause(int duration)
    {
        StartCoroutine(Pause(duration));
    }

    //��ͣЭ��
    IEnumerator Pause(int duration)
    {
        float pauseTime = duration / 60f;
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(pauseTime);
        Time.timeScale = 1;
    }


    //�����
    public void CameraShake(float shanke_force)
    {
        cinemachineImp.GenerateImpulseWithForce(shanke_force);
    }
}
