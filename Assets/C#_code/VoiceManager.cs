using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class VoiceManager : MonoBehaviour
{
    public static VoiceManager voiceManager;
    public static AudioSource audioSource;

    [Header("³Â")]
    public AudioClip skill1;        //³Â¼¼ÄÜ1
    public AudioClip skill_chen;    //³Â-¼¼ÄÜÓïÒô
    public AudioClip skill_chen_2;  //³Â-¼¼ÄÜÓïÒô2
    public AudioClip chen_hit;      //³Â-ÊÜÉËÓïÒô
    public AudioClip fall;          //ÂäÏÂ
    public AudioClip fall2;          //ÂäÏÂ
    public AudioClip kick;          //³Â--Ìß»÷
    public AudioClip punch;         //³Â--È­»÷
    public AudioClip knife;         //³Â--³öµ¶
    public AudioClip knife_hit;     //³Â--µ¶»÷
    public AudioClip scabbard;      //³Â--µ¶ÇÊ
    public AudioClip walk;          //³Â--²½ÐÐ
    public AudioClip run;          //³Â--ÅÜ²½
    public AudioClip jump;          //³Â--ÌøÔ¾




    // Start is called before the first frame update
    void Start()
    {
        voiceManager = this;

        audioSource = GetComponent<AudioSource>();

    }



    public static void Skill1()
    {
        audioSource.PlayOneShot(voiceManager.skill1);
    }
    public static void SkillChen()
    {
        audioSource.PlayOneShot(voiceManager.skill_chen);
    }
    public static void SkillChen2()
    {
        audioSource.PlayOneShot(voiceManager.skill_chen_2);
    }
    public static void ChenHit()
    {
        audioSource.PlayOneShot(voiceManager.chen_hit);
    }
    public static void Fall()
    {
        audioSource.PlayOneShot(voiceManager.fall);
    } 
    public static void Fall2()
    {
        audioSource.PlayOneShot(voiceManager.fall2);
    }
    public static void Kick()
    {
        audioSource.PlayOneShot(voiceManager.kick);
    }
    public static void Punch()
    {
        audioSource.PlayOneShot(voiceManager.punch);
    }
    public static void Knife()
    {
        audioSource.PlayOneShot(voiceManager.knife);
    }
    public static void KnifeHit()
    {
        audioSource.PlayOneShot(voiceManager.knife_hit);
    }
    public static void Scabbard()
    {
        audioSource.PlayOneShot(voiceManager.scabbard);
    }
    public static void Walk()
    {
        audioSource.PlayOneShot(voiceManager.walk);
    }
    public static void Run()
    {
        audioSource.PlayOneShot(voiceManager.run);
    }
    public static void Jump()
    {
        audioSource.PlayOneShot(voiceManager.jump);
    }




}
