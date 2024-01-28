using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManafer : MonoBehaviour
{
    public static AudioManafer instance;

    public void Awake()
    {
        instance = this; 
    }

    public AudioSource drawSoundEffect;
    public AudioSource playSoundEffect;
    public AudioSource attackSoundEffect;
    public AudioSource takeDamageSoundEffect;

    public void TriggerDraw()
    {
        drawSoundEffect.Play();
    }
    public void TriggerAttack()
    {
        attackSoundEffect.Play();
    }
    public void TriggerPlay()
    {
        playSoundEffect.Play();
    }
    public void TriggerTakeDamage()
    {
        takeDamageSoundEffect.Play();
    }
}
