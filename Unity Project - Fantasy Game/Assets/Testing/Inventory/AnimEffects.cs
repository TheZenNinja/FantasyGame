using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEffects : MonoBehaviour
{
    AudioSource source;

    [Header("Swords")]
    [SerializeField] List<ParticleSystem> lightningParticles;
    [SerializeField] List<AudioClip> swordAudioEffects;
    private void Start()
    {
        source = GetComponent<AudioSource>();
    }
    public void PlayEffectOnce(EquipmentType equipType, int stance, int index)
    {
        switch (equipType)
        {
            case EquipmentType.none:
                throw new Exception("Equipment is of type none");
            case EquipmentType.sword:
                switch (stance)
                {
                    case 4:
                        lightningParticles[0].Play();
                        break;
                    default:
                        throw new Exception("Not a valid Stance");
                }
                break;
            case EquipmentType.tool:
            case EquipmentType.bow:
            case EquipmentType.gun:
            default:
                throw new NotImplementedException();
        }
    }
    public void PlayAudioEffectOnce(EquipmentType equipType, int index)
    {
        switch (equipType)
        {
            case EquipmentType.none:
                throw new Exception("Equipment is of type none");
            case EquipmentType.sword:
                source.PlayOneShot(swordAudioEffects[index]);
                break;
            case EquipmentType.tool:
            case EquipmentType.bow:
            case EquipmentType.gun:
            default:
                throw new NotImplementedException();
        }
    }
}
