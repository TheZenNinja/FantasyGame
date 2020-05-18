using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBehavior : MonoBehaviour
{
    public ParticleSystem trailParticle;
    public TrailRenderer trailRenderer;
    public List<ParticleSystem> effects;
    public List<AudioClip> sounds;
    public AudioSource source;

    private void Start()
    {
        ToggleTrail(false);
    }

    public void ToggleTrail(bool active)
    {
        if (active)
        {
            if (!trailParticle.isPlaying)
                trailParticle.Play();
        }
        else
        {
            if (trailParticle.isPlaying)
                trailParticle.Stop();
        }
        trailRenderer.emitting = active;
    }
    public void PlayEffect(int index)
    {
        effects[index].Play();
        Debug.Log("Playing effect " + index);
    }
    public void StopEffect(int index)
    {
        effects[index].Stop();
    }
    public void StopAllEffects()
    {
        foreach (ParticleSystem ps in effects)
        {
            ps.Stop();
        }
    }

    public void PlaySound(int index)
    {
        if (sounds.Count > index)
        {
            source.PlayOneShot(sounds[index]);
        }
    }

    public void Attack(Entity e)
    {

    }
}
