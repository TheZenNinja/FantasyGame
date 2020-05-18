using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacking : MonoBehaviour
{
    #region Singleton
    public static Attacking instance;
    private void Awake()
    {
        instance = this;
    }
    #endregion

    private int _cmbI;
    public int ComboIndex
    {
        get { return _cmbI; }
        set
        {
            _cmbI = value;
            anim.SetInteger("Combo", value);
        }
    }
    public bool isBreakingAttack;
    public bool isLiftingAttack;

    private bool _ci;
    public bool CanInterrupt
    {
        get { return _ci; }
        set
        {
            _ci = value;
            anim.SetBool("Can Interrupt", value);
        }
    }

    public bool canAttack = true;

    #region Dodgeing
    public bool invulnerable;
    Vector3 dodgeDir;
    #endregion

    [HideInInspector]
    public CCMovement move;
    public Animator anim;

    public WeaponBehavior weapon;

    [Header("Target Acc")]
    public EntityFinder fov;
    public Transform cam;
    public Transform forwardRef;
    public List<Entity> entities;
    public Entity closestEntity;
    public Entity currentEntity;

    void Start()
    {
        //Time.timeScale = 0.1f;
        move = GetComponent<CCMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canAttack || CanInterrupt)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                ChooseAttack(false);
            }
            else if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                ChooseAttack(true);
            }
        }
        if (!invulnerable && Input.GetKeyDown(KeyCode.LeftAlt))
            anim.SetTrigger("Dodge");

        anim.SetBool("Grounded", move.grounded);

        FindEntities();
        //HandleEntities();
    }
    private void FindEntities()
    {
        entities = fov.FindVisibleEntities();

        for (int i = 0; i < entities.Count; i++)
        {
            Vector3 dir = Vector3.Scale(cam.forward, new Vector3(1, 0, 1)).normalized;
            if (closestEntity == null)
                closestEntity = entities[i];
            else if (Vector3.Angle(dir, (entities[i].position - transform.position).normalized)
                   < Vector3.Angle(dir, (closestEntity.position - transform.position).normalized))
                closestEntity = entities[i];
        }

        currentEntity = closestEntity;

        if (entities.Count < 1)
            currentEntity = null;

        if (currentEntity)
            move.targetEntity = currentEntity;
        else
            move.targetEntity = null;
    }

    private void ChooseAttack(bool isHeavy)
    {
        anim.SetTrigger("Interrupt");
        anim.SetBool("Can Interrupt", true);

        if (Input.GetKey(KeyCode.LeftShift))
        {
            anim.SetBool("Dash Attack", true);
        }
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            anim.SetBool("Lift Attack", true);
        }
        else
        {
            anim.SetBool("Normal Attack", true);
        }

        if (isHeavy)
            anim.SetBool("Heavy Attack", true);
        else
            anim.SetBool("Light Attack", true);
    }

    #region Linking Methods


    public void SetVelocity(Vector3 velocity)
    {
        //move.Boost(velocity);
    }
    public void ToggleParticle(bool active)
    {
        weapon.ToggleTrail(active);
    }

    public void PlayEffect(int index)
    {
        weapon.PlayEffect(index);
    }

    public void PlaySound(int index)
    {
        weapon.PlaySound(index);
    }

    public void LaunchTarget(Vector3 force)
    {
        currentEntity.Launch(force);
        //StartCoroutine(LaunchEntitiy(force, duration));
    }

    //public void 
    #endregion

    public void Hit()
    {
        if (currentEntity)
            currentEntity.Hit();
    }
    
    #region Combo Timeout
    public void StartComboTimeout()
    {
        if (IsInvoking(nameof(ComboTimeout)))
            CancelInvoke(nameof(ComboTimeout));
        Invoke(nameof(ComboTimeout), 1f);
    }
    private void ComboTimeout()
    {
        ComboIndex = 0;
    }
    #endregion

    private void OnDrawGizmos()
    {

    }
}
