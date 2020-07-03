using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public enum Faction
{
    none,
    player,
    enemy
}
public class Health : MonoBehaviour, IDamagableObject
{
    public Faction faction;
    public int currentHp;
    [SerializeField] int maxHp = 10;
    public int currentShield;
    [SerializeField] int maxShield = 10;

    public float shieldRechargeDelay = 2;
    private float shieldDelayTimer = 0;

    public int regenPerSecond = 2;
    private float regenTimer;

    public bool useHealthbar;
    public Slider hpBar;
    public Slider shieldBar;

    public bool useText;
    public TextMeshProUGUI shieldTxt;
    public TextMeshProUGUI healthTxt;

    public Action<int> Hurt;
    public Action Die;

    private EntityStatus status;

    private void Start()
    {
        status = GetComponent<EntityStatus>();

        currentHp = maxHp;
        currentShield = maxShield;

        if (useHealthbar)
        {
            hpBar.value = 1;
            shieldBar.value = 1;
        }
        if (useText)
        {
            shieldTxt.text = currentShield + "/" + maxShield;
            healthTxt.text = currentHp + "/" + maxHp;
        }
    }
    private void Update()
    {
        if (currentShield < maxShield)
        {
            if (shieldDelayTimer > 0)
                shieldDelayTimer -= Time.deltaTime;
            else
            {
                regenTimer += Time.deltaTime * regenPerSecond;
                if (regenTimer >= 1)
                {
                    currentShield++;
                    UpdateUI();
                    regenTimer = 0;
                }
                
            }
        }
    }
    public void DamageObj(int dmg, EntityStatus sender)
    {
        if (status.invuln)
        {
            Debug.Log(gameObject.name + " Dodged");
            return;
        }
        else if (status.parry)
        {
            if (sender != null)
                sender.GetComponent<Health>().DamageObj(dmg, GetComponent<EntityStatus>());

            FindObjectOfType<FirstPersonRigTest>().ResetParry();

            Debug.Log(gameObject.name + " Parried");

            return;
        }
        else if (status.block)
        {
            dmg = Mathf.RoundToInt((float)dmg / 4);
            Debug.Log(gameObject.name + " Blocked");
        }
        else
            Debug.Log(gameObject.name + " was hit");

        if (currentShield > 0)
        {
            currentShield -= dmg;
            if (currentShield <= 0)
                currentShield = 0;

            Hurt?.Invoke(dmg);
        }
        else
        {
            currentHp -= dmg;
            if (currentHp <= 0)
                currentHp = 0;


            Hurt?.Invoke(dmg);

        }
        UpdateUI();

        shieldDelayTimer = shieldRechargeDelay;

        if (currentHp <= 0)
            Die?.Invoke();
    }
    public bool Heal(int amt = 1)
    {
        if (currentHp >= maxHp)
            return false;

        currentHp += amt;
        if (currentHp > maxHp)
            currentHp = maxHp;

        UpdateUI();
            return true;
    }

    private void UpdateUI()
    {
        if (useHealthbar)
        {
            shieldBar.value = (float)currentShield / maxShield;
            hpBar.value = (float)currentHp / maxHp;
        }
        if (useText)
        {
            shieldTxt.text = currentShield + "/" + maxShield;
            healthTxt.text = currentHp + "/" + maxHp;
        }
    }
}
