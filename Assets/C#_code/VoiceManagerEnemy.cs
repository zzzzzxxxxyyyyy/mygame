using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class VoiceManagerEnemy : MonoBehaviour
{
    public static VoiceManagerEnemy voiceManager;
    public static AudioSource audioSource;

    [Header("无人机")]
    public AudioClip wurenji_walk;        //无人机飞行
    public AudioClip bomb_imp;              //炸弹爆炸




    // Start is called before the first frame update
    void Start()
    {
        voiceManager = this;

        audioSource = GetComponent<AudioSource>();

    }



    public static void FlyVoice()
    {
        audioSource.PlayOneShot(voiceManager.wurenji_walk);
    }
    public static void BoomVoice()
    {
        audioSource.PlayOneShot(voiceManager.bomb_imp);
    }




}
