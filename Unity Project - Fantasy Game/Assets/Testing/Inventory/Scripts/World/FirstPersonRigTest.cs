using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonRigTest : MonoBehaviour
{
    public EquipmentType currentEquipmentType;

    public bool canAttack;
    public bool blocking;
    public bool parry;
    private float parryTimer;

    public float parryCooldown = 1;
    private float parryCooldownTimer;

    public int stance;

    public RuntimeAnimatorController unequiped;
    public RuntimeAnimatorController tools;
    public RuntimeAnimatorController swords;

    [Header("Hotbox")]
    public int damage = 10;
    public float range = 2;

    [Header("Sound")]
    public float pitchVarience = 0.1f;


    AudioSource source;
    PlayerStatus status;
    HitboxManager hitbox;
    AnimEffects effects;
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        status = GetComponentInParent<PlayerStatus>();
        source = GetComponent<AudioSource>();
        effects = GetComponentInChildren<AnimEffects>();
        hitbox = GetComponentInChildren<HitboxManager>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        status.block = blocking;
        status.parry = parry;

        parry = parryTimer > 0;
        if (parry)
            parryTimer -= Time.deltaTime;

        if (InventoryManager.instance.inUI)
            return;

        switch (currentEquipmentType)
        {
            case EquipmentType.sword:
                {
                    if (Input.GetKeyDown(KeyCode.Q))
                    {
                        stance++;
                        if (stance > 2)
                            stance = 0;
                    }
                }
                break;
            case EquipmentType.none:
            case EquipmentType.tool:
            case EquipmentType.bow:
            case EquipmentType.gun:
            default:
                break;
        }

        if (parry || blocking)
        {
            if (Input.GetKeyUp(KeyCode.V))
            {
                anim.SetBool("Block", false);
                parryTimer = 0;
            }
        }
        else if (canAttack)
        {
            switch (currentEquipmentType)
            {
                case EquipmentType.sword:
                    {
                        if (stance != anim.GetInteger("Stance"))
                        {
                            anim.SetInteger("Stance", stance);
                            anim.SetTrigger("Change Stance");
                        }
                    }
                    break;
                case EquipmentType.none:
                case EquipmentType.tool:
                case EquipmentType.bow:
                case EquipmentType.gun:
                default:
                    break;
            }

            if (Input.GetKeyDown(KeyCode.Mouse0))
                anim.SetTrigger("Attack");
            else if (Input.GetKeyDown(KeyCode.Mouse1))
                anim.SetTrigger("Special");
            else if (Input.GetKeyDown(KeyCode.V))
                anim.SetBool("Block", true);
        }
        
    }
    public void SwitchItem(ModularItem item)
    {
        if (item == null)
        {
            currentEquipmentType = EquipmentType.none;
            damage = 2;
            range = 1;
        }
        else
        {
            currentEquipmentType = item.equipmentType;
            damage = item.attackDmg;
            range = item.swingRange;
        }

        switch (currentEquipmentType)
        {
            case EquipmentType.tool:
                anim.runtimeAnimatorController = tools;
                break;
            case EquipmentType.sword:
                anim.runtimeAnimatorController = swords;
                break;
            case EquipmentType.bow:
            case EquipmentType.gun:
            case EquipmentType.none:
            default:
                anim.runtimeAnimatorController = unequiped;
                break;
        }
        stance = 0;
    }

    public void StartParry(float duration)
    {
        parryTimer = duration;
        blocking = true;
    }
    public void ResetParry()
    {
        parryCooldownTimer = 0;
    }
    public void CanAttack()
    {
        canAttack = true;
    }
    public void SwingSound()
    {
        source.pitch = 1 + UnityEngine.Random.Range(-pitchVarience, pitchVarience);
        if (source.isPlaying)
        source.Stop();
        source.Play();
    }
    public void Attack(float multi)
    {
        hitbox.Hit(Mathf.RoundToInt(damage * multi));
    }
    /*public void EnableHitbox()
    {
        //hitbox.SetActive(true);
    }
    public void DisableHitbox()
    {
        //hitbox.SetActive(false);
    }*/

    /// <param name="ID">Format: (letter).(number).(number), S.0.0 (Sword, fire, effect 0) </param>
    public void StanceSpecialEffect(string ID)
    {
        string[] parts = ID.Split('.');
        EquipmentType equipType;
        int stance;
        int index;

        switch (parts[0].ToLower())
        {
            case "s":
                equipType = EquipmentType.sword;
                break;
            case "t":
                equipType = EquipmentType.tool;
                break;
            default:
                throw new Exception("Weapon Type Not Recognized: " + parts[0]);
        }
        if (!int.TryParse(parts[1], out stance))
            throw new Exception("Stance not recognized: " + parts[1]);
        if (!int.TryParse(parts[2], out index))
            throw new Exception("Index not recognized: " + parts[2]);

        effects.PlayEffectOnce(equipType, stance, index);
    }
    /// <param name="ID">Format: (letter).(number), S.0 (Sword, effect 0) </param>
    public void PlayAudioEffect(string ID)
    {
        string[] parts = ID.Split('.');
        EquipmentType equipType;
        int index;

        switch (parts[0].ToLower())
        {
            case "s":
                equipType = EquipmentType.sword;
                break;
            case "t":
                equipType = EquipmentType.tool;
                break;
            default:
                throw new Exception("Weapon Type Not Recognized: " + parts[0]);
        }
        if (!int.TryParse(parts[1], out index))
            throw new Exception("Index not recognized: " + parts[1]);

        effects.PlayAudioEffectOnce(equipType, index);
    }
}
