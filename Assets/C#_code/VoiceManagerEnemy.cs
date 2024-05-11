using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class VoiceManagerEnemy : MonoBehaviour
{
    public static VoiceManagerEnemy voiceManager;
    public static AudioSource audioSource;

    [Header("���˻�")]
    public AudioClip wurenji_walk;        //���˻�����
    public AudioClip bomb_imp;              //ը����ը




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
