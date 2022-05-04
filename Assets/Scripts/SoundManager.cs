using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static AudioClip OriFly, OriHold;
    static AudioSource audioSrc;

    void Start ()
    {
        OriFly = Resources.Load<AudioClip>("Fly");
        OriHold = Resources.Load<AudioClip>("Hold");

        audioSrc = GetComponent<AudioSource>();

    }

    public static void PlaySound (string clip)
    {
        switch (clip) 
        {
            case "Fly":
                audioSrc.PlayOneShot(OriFly);
                break;
             case "Hold":
                audioSrc.PlayOneShot(OriHold);
                break;
        }

    }
}
